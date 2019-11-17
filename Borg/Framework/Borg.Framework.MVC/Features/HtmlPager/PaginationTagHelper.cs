using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Features.HtmlPager
{
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        private const string ControllerKey = "controller";
        private const string ActionKey = "action";
        private const string AreaKey = "area";

        private readonly LinkGenerator LinkGenerator;
        private readonly IOptionsMonitor<PaginationConfiguration> options;

        private PaginationConfiguration Options => options.CurrentValue;

        public PaginationTagHelper(IOptionsMonitor<PaginationConfiguration> options, LinkGenerator linkGenerator)

        {
            this.options = Preconditions.NotNull(options, nameof(options));

            LinkGenerator = Preconditions.NotNull(linkGenerator, nameof(linkGenerator));
        }

        [HtmlAttributeName("borg-model")]
        public IPagedResult Model { get; set; }

        [HtmlAttributeName("borg-behaviour")]
        public PaginationConfigurationBehaviour Behaviour { get; set; }

        [HtmlAttributeName("borg-behaviour-style")]
        public DisplayStyle BehaviourStyle { get; set; } = DisplayStyle.Undefined;

        [HtmlAttributeName("borg-behaviour-override-settings")]
        public bool BehaviourStyleOverrideSettings { get; set; } = true;

        [HtmlAttributeName("borg-query")]
        public QueryString Query { get; set; } = new QueryString(null);

        [HtmlAttributeName("borg-url-generator")]
        public Func<int, string> GeneratePageUrl { get; set; } = null;

        [HtmlAttributeName("borg-page-variable")]
        public string PageVariable { get; set; } = "p";

        [HtmlAttributeName("borg-count-variable")]
        public string RowsVariable { get; set; } = "r";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!Query.HasValue)
            {
                Query = ViewContext.HttpContext.Request.QueryString;
            }

            var existsingClass = context.AllAttributes.ContainsName("class")
                ? context.AllAttributes["class"].Value.ToString()
                    : string.Empty;
            if (Model == null) throw new ArgumentNullException(nameof(Model));

            var behaviour = DefineBehaviour();

            if (GeneratePageUrl == null)
            {
                var descriptor = ViewContext.ActionDescriptor;
                var urlHelper = new UrlHelper(ViewContext);
                GeneratePageUrl = i => UrlFromViewContext(urlHelper, descriptor, i, behaviour.PageVariable);
            }
            var content = Pagination.GetHtmlPager(Model, GeneratePageUrl, Query.ToDictionary(), Options, behaviour, null);
            var trimstart = content.IndexOf('>') + 1;
            var trimend = content.Length - content.LastIndexOf('<') - 1;
            var trimmed = content.Substring(trimstart, content.Length - trimend - trimstart - 1);
            output.Content.Clear();
            output.TagName = Options.OutputTagElement;
            output.Attributes.SetAttribute("class", $"{Options.ElementClass} {existsingClass}");
            output.Content.SetHtmlContent(trimmed);
            var s = (await output.GetChildContentAsync()).GetContent();
        }

        private PaginationConfigurationBehaviour DefineBehaviour()
        {
            var behaviour = Options.Behaviour;

            if (BehaviourStyle != DisplayStyle.Undefined)
            {
                behaviour = GetBehaviour(BehaviourStyleOverrideSettings, behaviour);
            }

            if (BehaviourStyleOverrideSettings && Behaviour != null)
            {
                foreach (var prop in typeof(PaginationConfigurationBehaviour).GetProperties())
                {
                    var local = behaviour;
                    var val = typeof(PaginationConfigurationBehaviour).GetProperty(prop.Name).GetValue(Behaviour);
                    if (val != null)
                    {
                        typeof(PaginationConfigurationBehaviour).GetProperty(prop.Name).SetValue(local, val);
                    }
                    behaviour = local;
                }
            }
            return behaviour;
        }

        private string UrlFromViewContext(UrlHelper urlHelper, ActionDescriptor descriptor, int i, string pageVariable)
        {
            var action = descriptor.RouteValues.ContainsKey(ActionKey)
                    ? descriptor.RouteValues[ActionKey]
                    : string.Empty;
            var controller = descriptor.RouteValues.ContainsKey(ControllerKey)
                ? descriptor.RouteValues[ControllerKey]
                : string.Empty;

            var area = descriptor.RouteValues.ContainsKey(AreaKey)
               ? descriptor.RouteValues[AreaKey]
               : string.Empty;

            var raw = LinkGenerator.GetPathByAction(action, controller, new { Area = area });

            return $"{raw}?{pageVariable}={i}";
        }

        private PaginationConfigurationBehaviour GetBehaviour(bool overrideSettings, PaginationConfigurationBehaviour behaviour)
        {
            PaginationConfigurationBehaviour resut = default;
            if (overrideSettings)
            {
                switch (BehaviourStyle)
                {
                    case DisplayStyle.DefaultPager:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            DisplayLinkToIndividualPages = true,
                            MaximumPageNumbersToDisplay = Options.Behaviour.MaximumPageNumbersToDisplay,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.MinimalWithItemCountText:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayItemSliceAndTotal = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.DefaultPlusFirstAndLast:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToFirstPage = true,
                            DisplayLinkToLastPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.Minimal:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.MinimalWithPageCountText:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.MinimalWithPages:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToFirstPage = false,
                            DisplayLinkToLastPage = false,
                            DisplayLinkToPreviousPage = true,
                            DisplayLinkToNextPage = true,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                            DisplayPageCountAndCurrentLocation = false,
                            PagerInChunks = false,
                            DisplayLinkToIndividualPages = true,
                            MaximumPageNumbersToDisplay = Options.Behaviour.MaximumPageNumbersToDisplay,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.PageNumbersOnly:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToFirstPage = false,
                            DisplayLinkToLastPage = false,
                            DisplayLinkToPreviousPage = false,
                            DisplayLinkToNextPage = false,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                            DisplayLinkToIndividualPages = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    case DisplayStyle.PagerInChucks:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToFirstPage = false,
                            DisplayLinkToLastPage = false,
                            DisplayLinkToPreviousPage = true,
                            DisplayLinkToNextPage = true,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                            PagerInChunks = true,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;

                    default:
                        resut = new PaginationConfigurationBehaviour
                        {
                            DisplayLinkToNextPage = true,
                            DisplayLinkToPreviousPage = true,
                            DisplayPageCountAndCurrentLocation = true,
                            DisplayLinkToIndividualPages = true,
                            MaximumPageNumbersToDisplay = 10,
                            DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                            PagerInChunks = false,
                            ActiveItemClassOperation = ActiveItemClassOperation.CurrentClass
                        };
                        break;
                }
            }
            else
            {
                switch (BehaviourStyle)
                {
                    case DisplayStyle.DefaultPager:
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.DisplayPageCountAndCurrentLocation = true;
                        behaviour.DisplayLinkToIndividualPages = true;
                        break;

                    case DisplayStyle.MinimalWithItemCountText:
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.DisplayItemSliceAndTotal = true;
                        behaviour.PagerInChunks = false;
                        break;

                    case DisplayStyle.DefaultPlusFirstAndLast:
                        behaviour.DisplayLinkToFirstPage = true;
                        behaviour.DisplayLinkToLastPage = true;
                        behaviour.DisplayPageCountAndCurrentLocation = true;
                        behaviour.PagerInChunks = false;
                        break;

                    case DisplayStyle.Minimal:
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.PagerInChunks = false;
                        break;

                    case DisplayStyle.MinimalWithPageCountText:
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.DisplayPageCountAndCurrentLocation = true;
                        behaviour.PagerInChunks = false;

                        break;

                    case DisplayStyle.MinimalWithPages:
                        behaviour.DisplayLinkToFirstPage = false;
                        behaviour.DisplayLinkToLastPage = false;
                        behaviour.DisplayPageCountAndCurrentLocation = true;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayEllipsesWhenNotShowingAllPageNumbers = false;
                        behaviour.DisplayPageCountAndCurrentLocation = false;
                        behaviour.PagerInChunks = false;
                        behaviour.DisplayLinkToIndividualPages = true;

                        break;

                    case DisplayStyle.PageNumbersOnly:
                        behaviour.DisplayLinkToFirstPage = false;
                        behaviour.DisplayLinkToLastPage = false;
                        behaviour.DisplayLinkToPreviousPage = false;
                        behaviour.DisplayLinkToNextPage = false;
                        behaviour.DisplayEllipsesWhenNotShowingAllPageNumbers = false;
                        behaviour.DisplayLinkToIndividualPages = true;
                        behaviour.PagerInChunks = false;

                        break;

                    case DisplayStyle.PagerInChucks:
                        behaviour.DisplayLinkToFirstPage = false;
                        behaviour.DisplayLinkToLastPage = false;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayEllipsesWhenNotShowingAllPageNumbers = false;
                        behaviour.PagerInChunks = true;

                        break;

                    default:
                        behaviour.DisplayLinkToNextPage = true;
                        behaviour.DisplayLinkToPreviousPage = true;
                        behaviour.DisplayPageCountAndCurrentLocation = true;
                        behaviour.DisplayLinkToIndividualPages = true;
                        behaviour.DisplayEllipsesWhenNotShowingAllPageNumbers = true;
                        behaviour.PagerInChunks = false;

                        break;
                }
                resut = behaviour;
            }
            if (!string.IsNullOrWhiteSpace(PageVariable)) resut.PageVariable = PageVariable;
            return resut;
        }
    }
}
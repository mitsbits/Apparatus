using Borg.Infrastructure.Core.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
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

        private readonly IPaginationSettingsProvider _provider;
        private readonly LinkGenerator _linkGenerator;

        public PaginationTagHelper(IPaginationSettingsProvider provider, LinkGenerator linkGenerator)

        {
            _provider = provider;
            _linkGenerator = linkGenerator;
        }

        [HtmlAttributeName("borg-model")]
        public IPagedResult Model { get; set; }

        [HtmlAttributeName("borg-settings")]
        public Pagination.PaginationInfo Settings { get; set; } = new Pagination.PaginationInfo();

        [HtmlAttributeName("borg-display-style")]
        public DisplayStyle DisplayStyle { get; set; } = DisplayStyle.Minimal;

        [HtmlAttributeName("borg-query")]
        public QueryString Query { get; set; } = new QueryString(null);

        [HtmlAttributeName("borg-url-generator")]
        public Func<int, string> GeneratePageUrl { get; set; } = null;

        [HtmlAttributeName("borg-page-variable")]
        public string PageVariable { get; set; } = "p";
        [HtmlAttributeName("borg-count-variable")]
        public string CountVariable { get; set; } = "r";

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (_provider != null)
            {
                Settings = GetSettings();
            }
            if (!Query.HasValue)
            {
                Query = ViewContext.HttpContext.Request.QueryString;
            }
            if (GeneratePageUrl == null)
            {
                var descriptor = ViewContext.ActionDescriptor;
                var urlHelper = new UrlHelper(ViewContext);
                GeneratePageUrl = i => UrlFromViewContext(urlHelper, descriptor, i);
            }
            var existsingClass = context.AllAttributes.ContainsName("class")
                ? context.AllAttributes["class"].Value.ToString()
                    : string.Empty;
            if (Model == null) throw new ArgumentNullException(nameof(Model));
            var content = Pagination.GetHtmlPager(Model, GeneratePageUrl, Query.ToDictionary(), Settings, null);
            var trimstart = content.IndexOf('>') + 1;
            var trimend = content.Length - content.LastIndexOf('<')-1;
            var trimmed = content.Substring(trimstart, content.Length - trimend - trimstart-1);
            output.Content.Clear();
            output.TagName = Settings.OutputTagElement;

            //output.Content.AppendHtml(trimmed);
            output.Attributes.SetAttribute("class", $"{Settings.ElementClass} {existsingClass}");
            output.Content.SetHtmlContent(trimmed);
            var s = (await output.GetChildContentAsync()).GetContent();
        }

        private string UrlFromViewContext(UrlHelper urlHelper, ActionDescriptor descriptor, int i)
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

            var raw = _linkGenerator.GetPathByAction(action, controller, new { Area = area });

            return $"{raw}?{Settings.PageVariableName}={i}";
        }

        private Pagination.PaginationInfo GetSettings()
        {
            Pagination.PaginationInfo resut;
            switch (DisplayStyle)
            {
                case DisplayStyle.DefaultPager:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        DisplayLinkToIndividualPages = true,
                        MaximumPageNumbersToDisplay = 10,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                        PagerInChunks = false,
                    };
                    break;

                case DisplayStyle.MinimalWithItemCountText:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayItemSliceAndTotal = true,
                        PagerInChunks = false,
                    };
                    break;

                case DisplayStyle.DefaultPlusFirstAndLast:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = true,
                        DisplayLinkToLastPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        PagerInChunks = false,
                    };
                    break;

                case DisplayStyle.Minimal:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        PagerInChunks = false,
                    };
                    break;

                case DisplayStyle.MinimalWithPageCountText:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        PagerInChunks = false,
                    };
                    break;

                case DisplayStyle.MinimalWithPages:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = false,
                        DisplayLinkToLastPage = false,
                        DisplayLinkToPreviousPage = true,
                        DisplayLinkToNextPage = true,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                        DisplayPageCountAndCurrentLocation = false,
                        PagerInChunks = false,
                        DisplayLinkToIndividualPages = true,
                        MaximumPageNumbersToDisplay = 10,
                    };
                    break;

                case DisplayStyle.PageNumbersOnly:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = false,
                        DisplayLinkToLastPage = false,
                        DisplayLinkToPreviousPage = false,
                        DisplayLinkToNextPage = false,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                        DisplayLinkToIndividualPages = true,
                        PagerInChunks = false,
                    };
                    break;

                case DisplayStyle.PagerInChucks:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToFirstPage = false,
                        DisplayLinkToLastPage = false,
                        DisplayLinkToPreviousPage = true,
                        DisplayLinkToNextPage = true,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                        PagerInChunks = true,
                    };
                    break;

                default:
                    resut = new Pagination.PaginationInfo(_provider)
                    {
                        DisplayLinkToNextPage = true,
                        DisplayLinkToPreviousPage = true,
                        DisplayPageCountAndCurrentLocation = true,
                        DisplayLinkToIndividualPages = true,
                        MaximumPageNumbersToDisplay = 10,
                        DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                        PagerInChunks = false,
                    };
                    break;
            }

            if (!string.IsNullOrWhiteSpace(PageVariable)) resut.PageVariableName = PageVariable;
            return resut;
        }
    }
}
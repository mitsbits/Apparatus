using Borg.Infrastructure.Core.Collections;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace Borg.Framework.MVC.Features.HtmlPager
{
    public static partial class Pagination
    {
        public static HtmlString HtmlPager<T>(
            this IHtmlHelper helper,
            IPagedResult<T> metaData,
            Func<int, string> generatePageUrl,
            QueryString query,
            PaginationConfiguration settings = default,
            PaginationConfigurationBehaviour behaviour = default,
            object htmlAttributes = null)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData), "A navigation collection is mandatory.");
            if (!metaData.Any()) return HtmlString.Empty;
            if (settings == null) settings = new PaginationConfiguration();
            return
                new HtmlString(GetHtmlPager(metaData, generatePageUrl, query.ToDictionary(), settings, behaviour, htmlAttributes));
        }

        public static HtmlString HtmlPager<T>(
            this IHtmlHelper helper,
            IPagedResult<T> metaData,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues = null,
            PaginationConfiguration settings = default,
            PaginationConfigurationBehaviour behaviour = default,
            object htmlAttributes = null)
        {
            if (metaData == null)
                throw new ArgumentNullException(nameof(metaData), "A navigation collection is mandatory.");
            if (!metaData.Any()) return HtmlString.Empty;
            if (settings == null) settings = new PaginationConfiguration();
            return new HtmlString(GetHtmlPager(metaData, generatePageUrl, routedValues, settings, behaviour, htmlAttributes));
        }

        internal static string GetHtmlPager(
            IPagedResult metaData,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues,
            PaginationConfiguration settings,
            PaginationConfigurationBehaviour behaviour,
            object htmlAttributes)
        {
            var listItemLinks = new List<TagBuilder>();

            //first

            if (behaviour.DisplayLinkToFirstPage)
                listItemLinks.Add(First(metaData, generatePageUrl, routedValues, settings, behaviour));

            if (behaviour.DisplayLinkToPreviousPage)
                listItemLinks.Add(Previous(metaData, generatePageUrl, routedValues, settings, behaviour));

            //text
            if (behaviour.DisplayPageCountAndCurrentLocation)
                listItemLinks.Add(PageCountAndLocationText(metaData, settings));

            //text
            if (behaviour.DisplayItemSliceAndTotal)
                listItemLinks.Add(ItemSliceAndTotalText(metaData, settings));

            //page
            if (!behaviour.PagerInChunks)
            {
                if (behaviour.DisplayLinkToIndividualPages)
                {
                    //calculate start and end of range of page numbers
                    var start = 1;
                    var end = metaData.TotalPages;
                    if (metaData.TotalPages > behaviour.MaximumPageNumbersToDisplay)
                    {
                        var maxPageNumbersToDisplay = behaviour.MaximumPageNumbersToDisplay;
                        start = metaData.Page - maxPageNumbersToDisplay / 2;
                        if (start < 1)
                            start = 1;
                        end = maxPageNumbersToDisplay;
                        if ((start + end - 1) > metaData.TotalPages)
                            start = metaData.TotalPages - maxPageNumbersToDisplay + 1;
                    }

                    //if there are previous page numbers not displayed, show an ellipsis
                    if (behaviour.DisplayEllipsesWhenNotShowingAllPageNumbers && start > 1)
                        listItemLinks.Add(EllipsesPrevious(metaData, generatePageUrl, routedValues, settings, behaviour));

                    foreach (var i in Enumerable.Range(start, end))
                    {
                        //show page number link
                        listItemLinks.Add(Page(i, metaData, generatePageUrl, routedValues, settings, behaviour));
                    }

                    //if there are subsequent page numbers not displayed, show an ellipsis
                    if (behaviour.DisplayEllipsesWhenNotShowingAllPageNumbers && (start + end - 1) < metaData.TotalPages)
                        listItemLinks.Add(EllipsesNext(metaData, generatePageUrl, routedValues, settings, behaviour));
                }
            }
            else //show page links in chunks
            {
                int current = metaData.Page;

                int chunckStart = current;

                if (current % behaviour.ChunkCount != 0)
                {
                    while (chunckStart % behaviour.ChunkCount != 0)
                    {
                        chunckStart -= 1;
                    }
                }
                else
                {
                    chunckStart = current - behaviour.ChunkCount;
                }
                foreach (var i in Enumerable.Range(chunckStart + 1, behaviour.ChunkCount))
                {
                    //show page number link
                    listItemLinks.Add(Page(i, metaData, generatePageUrl, routedValues, settings, behaviour));
                }
            }

            //next
            if (behaviour.DisplayLinkToNextPage)
                listItemLinks.Add(Next(metaData, generatePageUrl, routedValues, settings, behaviour));

            //last
            if (behaviour.DisplayLinkToLastPage)
                listItemLinks.Add(Last(metaData, generatePageUrl, routedValues, settings, behaviour));

            //collapse all of the list items into one big string
            string listItemLinksString = null;

            var builder = new StringBuilder();
            foreach (var item in listItemLinks)
            {
                using (var writer = new System.IO.StringWriter())
                {
                    item.WriteTo(writer, HtmlEncoder.Default);
                    builder.Append(writer.ToString());
                }
            }
            listItemLinksString = builder.ToString();

            var ul = new TagBuilder("ul");
            ul.InnerHtml.AppendHtml(listItemLinksString);

            ul.AddCssClass(settings.ElementClass);
            if (htmlAttributes != null) ul.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            using (var writer = new System.IO.StringWriter())
            {
                ul.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        private static string GetRoutedValues(IDictionary<string, string[]> routedValues, string pageVariable)
        {
            StringBuilder paramBuilder = new StringBuilder(string.Empty);

            if (routedValues.Count > 0)
            {
                foreach (string key in routedValues.Keys)
                {
                    foreach (var val in routedValues[key])
                    {
                        if (!key.Equals(pageVariable, StringComparison.OrdinalIgnoreCase))
                            paramBuilder.Append($"&{key}={val}");
                    }
                }
            }

            return paramBuilder.ToString();
        }

        private static TagBuilder Next(IPagedResult metadata,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues,
            PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ArrowClass + " next");
            item.AddCssClass(settings.ItemClass);
            var targetPageNumber = metadata.Page + 1;
            var next = new TagBuilder("a");
            next.AddCssClass(settings.LinkClass);
            next.InnerHtml.Append(settings.NextDisplay);
            if (metadata.HasNextPage)
            {
                next.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }
            else
            {
                item.AddCssClass(settings.UnavailableClass);
                next.MergeAttribute("href",
                    string.Empty, true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(next);
            return item;
        }

        private static TagBuilder Last(IPagedResult metadata,
            Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues,
            PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ArrowClass);
            var targetPageNumber = metadata.TotalPages;
            var last = new TagBuilder("a");
            last.AddCssClass(settings.LinkClass);
            last.InnerHtml.Append(settings.LastDisplay);
            if (metadata.Page == metadata.TotalPages)
            {
                item.AddCssClass(settings.UnavailableClass);
                last.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                last.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(last);
            return item;
        }

        private static TagBuilder PageCountAndLocationText(IPagedResult metadata, PaginationConfiguration settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.UnavailableClass);
            item.AddCssClass(settings.ItemClass);
            var text = new TagBuilder("a");
            text.AddCssClass(settings.LinkClass);
            text.InnerHtml.AppendHtml(string.Format(settings.PageCountAndLocationFormat, metadata.Page,
                metadata.TotalPages));
            text.MergeAttribute("href",
                string.Empty, true);
            var htmlContentBuilder = item.InnerHtml.AppendHtml(text);
            return item;
        }

        private static TagBuilder ItemSliceAndTotalText(IPagedResult metadata, PaginationConfiguration settings)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.UnavailableClass);
            item.AddCssClass(settings.ItemClass);
            int FirstItemOnPage = (metadata.Page - 1) * metadata.PageSize + 1;
            var numberOfLastItemOnPage = FirstItemOnPage + metadata.PageSize - 1;
            int LastItemOnPage = numberOfLastItemOnPage > metadata.TotalRecords
                ? metadata.TotalRecords
                : numberOfLastItemOnPage;

            var text = new TagBuilder("a");
            text.AddCssClass(settings.LinkClass);
            text.InnerHtml.AppendHtml(string.Format(settings.ItemSliceAndTotalFormat, FirstItemOnPage, LastItemOnPage,
                metadata.TotalRecords));
            text.MergeAttribute("href",
                string.Empty, true);
            item.InnerHtml.AppendHtml(text);
            return item;
        }

        private static TagBuilder EllipsesPrevious(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var targetPageNumber = metaData.Page - (settings.Behaviour.MaximumPageNumbersToDisplay);
            if (targetPageNumber < 1) targetPageNumber = 1;

            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ItemClass);
            var a = new TagBuilder("a");
            a.AddCssClass(settings.LinkClass);
            a.InnerHtml.Append(settings.Ellipses);
            if (targetPageNumber == metaData.Page)
            {
                item.AddCssClass(settings.UnavailableClass);
                a.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                a.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(a);
            return item;
        }

        private static TagBuilder EllipsesNext(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var targetPageNumber = metaData.Page + (settings.Behaviour.MaximumPageNumbersToDisplay);
            if (targetPageNumber > metaData.TotalPages) targetPageNumber = metaData.TotalPages;

            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ItemClass);
            var a = new TagBuilder("a");
            a.AddCssClass(settings.LinkClass);
            a.InnerHtml.Append(settings.Ellipses);
            if (targetPageNumber == metaData.Page)
            {
                item.AddCssClass(settings.UnavailableClass);
                a.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                a.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }
            var htmlContentBuilder = item.InnerHtml.AppendHtml(a);
            return item;
        }

        private static TagBuilder Page(int i, IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ItemClass);
            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.AddCssClass(settings.LinkClass);
            page.InnerHtml.AppendHtml(string.Format(settings.PageDisplayFormat, i));

            if (metaData.Page == i)
            {
                switch (behaviour.ActiveItemClassOperation)
                {
                    case ActiveItemClassOperation.Both:
                        item.AddCssClass(settings.CurrentClass);
                        item.AddCssClass(settings.UnavailableClass);
                        break;

                    case ActiveItemClassOperation.CurrentClass:
                        item.AddCssClass(settings.CurrentClass);
                        break;

                    case ActiveItemClassOperation.UnavailableClass:
                        item.AddCssClass(settings.UnavailableClass);
                        break;
                }

                page.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                page.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }

            var htmlContentBuilder = item.InnerHtml.AppendHtml(page);
            return item;
        }

        private static TagBuilder Previous(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ItemClass);
            item.AddCssClass(settings.ArrowClass + " previous");
            var targetPageNumber = metaData.Page - 1;
            var previous = new TagBuilder("a");
            previous.AddCssClass(settings.LinkClass);
            previous.InnerHtml.Append(settings.PreviousDisplay);
            if (targetPageNumber < 1)
            {
                item.AddCssClass(settings.UnavailableClass);
                previous.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                previous.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }

            var htmlContentBuilder = item.InnerHtml.AppendHtml(previous);
            return item;
        }

        private static TagBuilder First(IPagedResult metaData, Func<int, string> generatePageUrl,
            IDictionary<string, string[]> routedValues, PaginationConfiguration settings, PaginationConfigurationBehaviour behaviour)
        {
            var item = new TagBuilder(settings.OutputItemTagElement);
            item.AddCssClass(settings.ItemClass);
            const int targetPageNumber = 1;
            var first = new TagBuilder("a");
            first.AddCssClass(settings.LinkClass);
            first.InnerHtml.Append(settings.FirstDisplay);
            if (metaData.Page == targetPageNumber)
            {
                item.AddCssClass(settings.UnavailableClass);
                first.MergeAttribute("href",
                    string.Empty, true);
            }
            else
            {
                first.MergeAttribute("href",
                    generatePageUrl(targetPageNumber)
                    + GetRoutedValues(routedValues,
                        behaviour.PageVariable), true);
            }

            var htmlContentBuilder = item.InnerHtml.AppendHtml(first);
            return item;
        }
    }
}
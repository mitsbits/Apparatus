using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.MVC.Features.HtmlPager
{
  public  class PaginationBehaviour
    {
        #region List Render options

        ///<summary>
        /// When true, includes a hyperlink to the first page of the list.
        ///</summary>
        public bool DisplayLinkToFirstPage { get; set; }

        ///<summary>
        /// When true, includes a hyperlink to the last page of the list.
        ///</summary>
        public bool DisplayLinkToLastPage { get; set; }

        ///<summary>
        /// When true, includes a hyperlink to the previous page of the list.
        ///</summary>
        public bool DisplayLinkToPreviousPage { get; set; }

        ///<summary>
        /// When true, includes a hyperlink to the next page of the list.
        ///</summary>
        public bool DisplayLinkToNextPage { get; set; }

        ///<summary>
        /// When true, includes hyperlinks for each page in the list.
        ///</summary>
        public bool DisplayLinkToIndividualPages { get; set; }

        ///<summary>
        /// When true, shows the current page number and the total number of pages in the list.
        ///</summary>
        ///<example>
        /// "Page 3 of 8."
        ///</example>
        public bool DisplayPageCountAndCurrentLocation { get; set; }

        ///<summary>
        /// When true, shows the one-based index of the first and last items on the page, and the total number of items in the list.
        ///</summary>
        ///<example>
        /// "Showing items 75 through 100 of 183."
        ///</example>
        public bool DisplayItemSliceAndTotal { get; set; }

        ///<summary>
        /// The maximum number of page numbers to display. Null displays all page numbers.
        ///</summary>
        public int? MaximumPageNumbersToDisplay { get; set; }

        ///<summary>
        /// If true, adds an ellipsis where not all page numbers are being displayed.
        ///</summary>
        ///<example>
        /// "1 2 3 4 5 ...",
        /// "... 6 7 8 9 10 ...",
        /// "... 11 12 13 14 15"
        ///</example>
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }

        public bool PagerInChunks { get; set; }

        #endregion List Render options
    }
}

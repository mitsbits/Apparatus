using System;
using System.ComponentModel;

namespace Borg.Framework.MVC.Features.HtmlPager
{

    public class PaginationConfigurationBehaviour
    {
        [DefaultValue(10)]
        public int ChunkCount { get; set; } = 10;

        [DefaultValue(false)]
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; } = false;

        [DefaultValue(false)]
        public bool DisplayLinkToNextPage { get; set; } = false;

        [DefaultValue(false)]
        public bool DisplayLinkToPreviousPage { get; set; } = false;

        [DefaultValue(false)]
        public bool DisplayLinkToLastPage { get; set; } = false;

        [DefaultValue(false)]
        public bool DisplayLinkToFirstPage { get; set; } = false;

        [DefaultValue(false)]
        public bool DisplayPageCountAndCurrentLocation { get; set; } = false;

        [DefaultValue(10)]
        public int MaximumPageNumbersToDisplay { get; set; } = 10;

        [DefaultValue(false)]
        public bool DisplayLinkToIndividualPages { get; set; } = false;

        [DefaultValue(false)]
        public bool DisplayItemSliceAndTotal { get; set; } = false;

        [DefaultValue(false)]
        public bool PagerInChunks { get; set; } = false;

        [DefaultValue("p")]
        public string PageVariable { get; set; } = "p";

        [DefaultValue("r")]
        public string RowsVariable { get; set; } = "r";

        [DefaultValue(ActiveItemClassOperation.Both)]
        public ActiveItemClassOperation ActiveItemClassOperation { get; set; } = ActiveItemClassOperation.Both;
    }
}
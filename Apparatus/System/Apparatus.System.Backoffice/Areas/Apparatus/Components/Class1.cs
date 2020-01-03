using System.Collections.Generic;

namespace Apparatus.System.Backoffice
{
    public sealed class menu
    {
        public List<section> sections { get; set; } = new List<section>();
    }



    public sealed class section
    {
        public List<ul> uls { get; set; } = new List<ul>();
    }

    public class ul
    {
        public List<li> lis { get; set; } = new List<li>();
    }

    public class li 
    {
        public LiType liType { get; set; }
        public ul ul { get; set; } = null;

    }

    public enum LiType
    {
        Href,
        RouteData,
        Header,
        Label,
        ScriptTrigger
    }

    public sealed class header : label 
    {

        public header()
        {
            liType = LiType.Header;
        }
    
    }

    public  class label : li
    {
        public label()
        {
            liType = LiType.Label;
        }

        public string display { get; set; }
        public string tooltip { get; set; }

    }

    public  class anchor : label
    {
        public anchor()
        {
            liType = LiType.Href;
        }

        public string href { get; set; }
        public string _target { get; set; }
    }
}
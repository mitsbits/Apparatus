using System.Collections.Generic;

namespace Apparatus.System.Backoffice
{

    public sealed class menu
    {
        public List<header> headers { get; set; }
        public List<section> sections { get; set; }

    }

    public sealed class header : displayer { }

    public sealed class section {

        public List<ul> uls { get; set; }

    }

    public class ul
    {
        public List<li> lis { get; set; }
    }

    public class li : displayer
    {
        public LiType liType { get; set; }
        public string area { get; set; }
        public string controler { get; set; }
        public string action { get; set; }
        public header header { get; set; }
    }

    public enum LiType
    {
        Href,
        RouteData,
        Header,
        ScriptTrigger
    }

    public abstract class displayer
    {
        public string display { get; set; }
        public string tooltip { get; set; }
        public string _target { get; set; }
    }
}
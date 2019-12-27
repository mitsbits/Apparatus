using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.Services.Backoffice.Navigation
{
    abstract class Item
    {
        public abstract string Icon { get; protected set; } 
        public abstract string Display { get; protected set; } 
        public abstract string Href { get; protected set; }
        public  ICollection<Item> Children { get; protected set; } = new HashSet<Item>();
    }
}

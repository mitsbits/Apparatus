
/* 
Licensed under the Apache License, Version 2.0

http://www.apache.org/licenses/LICENSE-2.0
*/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml2CSharp
{
	[XmlRoot(ElementName = "header")]
	public class Header
	{
		[XmlAttribute(AttributeName = "display")]
		public string Display { get; set; }
		[XmlAttribute(AttributeName = "tooltip")]
		public string Tooltip { get; set; }
	}

	[XmlRoot(ElementName = "li")]
	public class Li
	{
		[XmlAttribute(AttributeName = "area")]
		public string Area { get; set; }
		[XmlAttribute(AttributeName = "controller")]
		public string Controller { get; set; }
		[XmlAttribute(AttributeName = "action")]
		public string Action { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "display")]
		public string Display { get; set; }
		[XmlAttribute(AttributeName = "target")]
		public string Target { get; set; }
		[XmlAttribute(AttributeName = "tooltip")]
		public string Tooltip { get; set; }
		[XmlElement(ElementName = "header")]
		public Header Header { get; set; }
		[XmlElement(ElementName = "ul")]
		public Ul Ul { get; set; }
	}

	[XmlRoot(ElementName = "ul")]
	public class Ul
	{
		[XmlElement(ElementName = "li")]
		public List<Li> Li { get; set; }
	}

	[XmlRoot(ElementName = "section")]
	public class Section
	{
		[XmlElement(ElementName = "header")]
		public Header Header { get; set; }
		[XmlElement(ElementName = "ul")]
		public List<Ul> Ul { get; set; }
	}

	[XmlRoot(ElementName = "menu")]
	public class Menu
	{
		[XmlElement(ElementName = "header")]
		public Header Header { get; set; }
		[XmlElement(ElementName = "section")]
		public List<Section> Section { get; set; }
	}

}

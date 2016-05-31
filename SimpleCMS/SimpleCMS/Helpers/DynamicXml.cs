using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Linq;
using RestSharp.Serializers;

namespace SimpleCMS.SimpleCMS.Helpers
{
    public class DynamicXml : DynamicObject
    {
        XElement _root;
        private DynamicXml(XElement root)
        {
            _root = root;
        }

        public static DynamicXml Create(Object o)
        {
            var serializer = new XmlSerializer();
            var result = serializer.Serialize(o);
            return Parse(result);
        }

        public static DynamicXml Parse(string xmlString)
        {
            return new DynamicXml(XDocument.Parse(xmlString).Root);
        }

        public static DynamicXml Load(string filename)
        {
            return new DynamicXml(XDocument.Load(filename).Root);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            var att = _root.Attribute(binder.Name);
            if (att != null)
            {
                result = att.Value;
                return true;
            }
            var nodes = _root.Elements(binder.Name);
            if (nodes.Count() > 1)
            {
                result = nodes.Select(n => new DynamicXml(n)).ToList();
                return true;
            }
            var node = _root.Element(binder.Name);
            if (node != null)
            {
                if (node.HasElements)
                {
                    result = new List<DynamicXml>() { new DynamicXml(node) };
                    //result = new DynamicXml(node);
                }
                else
                {
                    result = node.Value;
                }
                return true;
            }
            return true;
        }
    }
}
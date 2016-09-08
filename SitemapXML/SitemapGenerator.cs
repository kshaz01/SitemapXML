using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SitemapXML
{
    public class SitemapGenerator
    {
        public Item RootItem
        {
            get;
            private set;
        }
        private string Site
        {
            get;
            set;
        }
        private bool AddAspxExtension
        {
            get;
            set;
        }
        public SitemapGenerator(Item startItem, string siteName, bool addAspxExtension)
        {
            this.RootItem = startItem;
            this.Site = siteName;
            this.AddAspxExtension = addAspxExtension;
        }
        public void Generate(string filename)
        {
            filename = FileUtil.MapPath(filename);
            XmlTextWriter textWriter = new XmlTextWriter(filename, Encoding.UTF8)
            {
                Formatting = Formatting.Indented
            };
            this.Generate(textWriter);
        }
        public void Generate(TextWriter writer)
        {
            XmlTextWriter textWriter = new XmlTextWriter(writer)
            {
                Formatting = Formatting.Indented
            };
            this.Generate(textWriter);
        }
        public void Generate(XmlTextWriter textWriter)
        {
            Error.AssertItem(this.RootItem, "Sitemap start item");
            new SitemapWorker(this, textWriter, this.Site, this.AddAspxExtension).Generate();
            textWriter.Close();
        }
    }
}

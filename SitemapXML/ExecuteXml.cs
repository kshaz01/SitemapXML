using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitemapXML
{
    public class ExecuteXml
    {
        public string url { get; set; }

        public void Run()
        {
            Database database = Factory.GetDatabase("web");
            Item item = database.GetItem(url);//itemArray[0];
            Log.Info("XML Sitemap started", this);
            string text = item["CurrentWebsite"];
            var addAspxExtension = item["AddAspxExtension"] == "1";
            if (string.IsNullOrEmpty(text))
            {
                text = "website";
            }
            SitemapGenerator sitemapGenerator = new SitemapGenerator(database.Items[ID.Parse(item["RootNode"]), Language.Parse(Factory.GetSite(text).Language)], text, addAspxExtension);
            sitemapGenerator.Generate(item["XmlFilePath"]);
            Log.Info("XML Sitemap ended", this);
        }

        public void BuildNewXml(Item[] itemArray, CommandItem commandItem, ScheduleItem scheduledItem)
        {
            Item item = itemArray[0];
            Database database = Factory.GetDatabase("web");
            Log.Info("XML Sitemap started", this);
            string text = item["CurrentWebsite"];
            var addAspxExtension = item["AddAspxExtension"] == "1";
            if (string.IsNullOrEmpty(text))
            {
                text = "website";
            }
            SitemapGenerator sitemapGenerator = new SitemapGenerator(database.Items[ID.Parse(item["RootNode"]), Language.Parse(Factory.GetSite(text).Language)], text, addAspxExtension);
            sitemapGenerator.Generate(item["XmlFilePath"]);
            Log.Info("XML Sitemap ended", this);
        }
      
    }
}

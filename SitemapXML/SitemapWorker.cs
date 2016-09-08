using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using Sitecore.Data;
namespace SitemapXML
{
    internal class SitemapWorker
    {
        private SitemapGenerator _Generator;
        private XmlTextWriter _Writer;
        private UrlOptions _UrlOptions;

        public SitemapWorker(SitemapGenerator generator, XmlTextWriter writer, string site, bool addAspxExtension)
        {
            this._Generator = generator;
            this._Writer = writer;
            this._UrlOptions = SitemapWorker.GetUrlOptions(site, addAspxExtension);
          
        }
        public void Generate()
        {
            this._Writer.WriteStartDocument();
            this._Writer.WriteStartElement("urlset");
            this._Writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            this.GenerateInternal(this._Generator.RootItem);
            this._Writer.WriteEndElement();
            this._Writer.Flush();
        }
        private void GenerateInternal(Item parent)
        {
            this.WriteItem(parent);
            foreach (Item parent2 in parent.Children)
            {
                this.GenerateInternal(parent2);
            }
        }
        private void WriteItem(Item item)
        {
            if (SitemapWorker.ShouldWrite(item))
            {
                this._Writer.WriteStartElement("url");
    
                string text = LinkManager.GetItemUrl(item, this._UrlOptions);
                
                if (text.IndexOf("http://") <= -1)
                {
                    text = text.Replace("://", "http://");
                }
                this._Writer.WriteElementString("loc", HttpUtility.UrlPathEncode(text));
                this._Writer.WriteElementString("lastmod", SitemapWorker.ToWtcDate(item.Statistics.Updated));
                if (!string.IsNullOrEmpty(SitemapWorker.CalculateChangeFreq(item)))
                {
                    this._Writer.WriteElementString("changefreq", SitemapWorker.CalculateChangeFreq(item));
                }
                this._Writer.WriteElementString("priority", SitemapWorker.CalculatePriority(item));
                this._Writer.WriteEndElement();
            }
        }
        private static bool ShouldWrite(Item item)
        {
            return item.Visualization.Layout != null && item["Include"] == "1";
        }
        private static string CalculateChangeFreq(Item item)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(item["changefreq"]))
            {
                result = Factory.GetDatabase("web").GetItem(new ID(item["changefreq"])).Name;
            }
            return result;
        }
        private static string CalculatePriority(Item item)
        {
            return string.IsNullOrEmpty(item["priority"]) ? "0.5" : item["priority"];
        }
        private static string ToWtcDate(DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy-MM-dd");
        }
        private static UrlOptions GetUrlOptions(string site, bool addAspxExtension)
        {
            return new UrlOptions
            {
                AlwaysIncludeServerUrl = true,
                AddAspxExtension = addAspxExtension,
                ShortenUrls = true,
                Site = Factory.GetSite(site),
                LanguageEmbedding = LanguageEmbedding.Never
            };
        }
    }

}
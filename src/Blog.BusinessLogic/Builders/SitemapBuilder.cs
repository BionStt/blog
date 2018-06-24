using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Blog.Core.Entities;
using Blog.Core.Enums;

namespace Blog.BusinessLogic.Builders
{
    public class SitemapBuilder
    {
        private readonly XNamespace _ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        private readonly List<SitemapItem> _urls;

        public SitemapBuilder()
        {
            _urls = new List<SitemapItem>();
        }

        public void AddUrl(String url,
                           DateTime? modified = null,
                           ChangeFrequency? changeFrequency = null,
                           Double? priority = null)
        {
            _urls.Add(new SitemapItem()
                      {
                          Url = url,
                          Modified = modified,
                          ChangeFrequency = changeFrequency,
                          Priority = priority,
                      });
        }

        public void AddUrl(SitemapItem sitemapItem)
        {
            _urls.Add(sitemapItem);
        }

        public override String ToString()
        {
            var sitemap = new XDocument(
                                        new XDeclaration("1.0", "utf-8", "yes"),
                                        new XElement(_ns + "urlset", _urls.Select(CreateItemElement))
                                       );

            return sitemap.ToString();
        }

        private XElement CreateItemElement(SitemapItem url)
        {
            var itemElement = new XElement(_ns + "url", new XElement(_ns + "loc", url.Url.ToLower()));

            if (url.Modified.HasValue)
            {
                itemElement.Add(new XElement(_ns + "lastmod", url.Modified.Value.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
            }

            if (url.ChangeFrequency.HasValue)
            {
                itemElement.Add(new XElement(_ns + "changefreq", url.ChangeFrequency.Value.ToString()
                                                                                          .ToLower()));
            }

            if (url.Priority.HasValue)
            {
                itemElement.Add(new XElement(_ns + "priority", url.Priority.Value.ToString("N1")));
            }

            return itemElement;
        }
    }
}
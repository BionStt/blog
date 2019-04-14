using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;

namespace Blog.Website.ActionResults
{
    public class RssStoriesFeedResult : IActionResult
    {
        private readonly Page<BlogStory> _page;
        
        public RssStoriesFeedResult(Page<BlogStory> page)
        {
            _page = page;
        }

        public async  Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            
            response.ContentType = "application/xml";
            
            var host = request.Scheme + "://" + request.Host;
            using (var xmlWriter = XmlWriter.Create(response.Body, new XmlWriterSettings() { Async = true, Indent = true, Encoding = new UTF8Encoding(false) }))
            {
                var rss = new RssFeedWriter(xmlWriter);
                await rss.WriteTitle("d2funlife | Блог о программировании");
                await rss.WriteDescription("Описание");
                await rss.WriteGenerator("генрен");
                await rss.WriteValue("link", host);
                
                foreach (var post in _page.Items)
                {
                    var item = new AtomEntry
                    {
                        Title = post.Title,
                        Description = post.Content.Replace("data-src", "src"),
                        Id = post.Id.ToString(),
                        Published = post.PublishedDate.Value,
                        LastUpdated = post.ModifiedDate,
                        ContentType = "html",
                    };

                    foreach (var tag in post.BlogStoryTags)
                    {
                        item.AddCategory(new SyndicationCategory(tag.Tag.Name));
                    }

                    item.AddContributor(new SyndicationPerson("Danil Pavlov", "d2funlife@gmail.com (Danil Pavlov)","author"));
                    item.AddLink(new SyndicationLink(new Uri($"{host}/{post.Alias}")));

                    await rss.Write(item);
                }
            }
        }
    }
}
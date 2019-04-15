using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Website.Core.ConfigurationOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;

namespace Blog.Website.ActionResults
{
    public class RssStoriesFeedResult : IActionResult
    {
        private readonly Page<BlogStory> _page;
        private readonly IOptions<FeedOptions> _options;
        
        public RssStoriesFeedResult(Page<BlogStory> page,
                                    IOptions<FeedOptions> options)
        {
            _page = page;
            _options = options;
        }

        public async  Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;
            
            response.ContentType = "application/xml";
            
            var host = request.Scheme + "://" + request.Host;
            using (var xmlWriter = XmlWriter.Create(response.Body, new XmlWriterSettings() { Async = true, Indent = true, Encoding = new UTF8Encoding(true) }))
            {
                var rss = new RssFeedWriter(xmlWriter);
                await rss.WriteTitle(_options.Value.Title);
                await rss.WriteDescription(_options.Value.Description);
                await rss.WriteGenerator(_options.Value.GeneratorDescription);
                await rss.WriteValue("link", host);
                
                foreach (var post in _page.Items)
                {
                    var item = new AtomEntry
                    {
                        Title = post.Title,
                        Description = post.GetContentWithoutDataSrc(),
                        Id = post.Id.ToString(),
                        Published = post.PublishedDate.Value,
                        LastUpdated = post.ModifiedDate,
                        ContentType = "html",
                    };

                    foreach (var tag in post.BlogStoryTags)
                    {
                        item.AddCategory(new SyndicationCategory(tag.Tag.Name));
                    }

                    item.AddContributor(new SyndicationPerson(_options.Value.AuthorName, _options.Value.FullEmail, "author"));
                    item.AddLink(new SyndicationLink(new Uri($"{host}/{post.Alias}")));

                    await rss.Write(item);
                }
            }
        }
    }
}
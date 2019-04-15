using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;

namespace Blog.Website.ActionResults
{
    public class AtomStoriesResult : IActionResult
    {
        private readonly Page<BlogStory> _page;

        public AtomStoriesResult(Page<BlogStory> page)
        {
            _page = page;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            var request = context.HttpContext.Request;

            response.ContentType = "application/atom+xml";

            var host = request.Scheme + "://" + request.Host + "/";
            using (var xmlWriter = XmlWriter.Create(response.Body, new XmlWriterSettings() {Async = true, Indent = true, Encoding = new UTF8Encoding(false)}))
            {
                var atom = new AtomFeedWriter(xmlWriter);
                
                await atom.WriteTitle("d2funlife | Блог о программировании");
                await atom.WriteId(host);
                await atom.WriteSubtitle("Описание");
                
                await atom.WriteRaw($"\n<link rel=\"self\" type=\"application/atom+xml\" href=\"{host}feed/atom\"/>");
                
                await atom.WriteGenerator("Miniblog.Core", "https://github.com/madskristensen/Miniblog.Core", "1.0");
                var lastPost = _page.Items.FirstOrDefault();
                if(lastPost != null)
                {
                    await atom.WriteValue("updated", lastPost.PublishedDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                }


                foreach (var post in _page.Items)
                {
                    var item = new AtomEntry
                    {
                        Title = post.Title,
                        Description = post.Content.Replace("data-src", "src"),
                        Id = $"{host}/{post.Alias}",
                        Published = post.PublishedDate.Value,
                        LastUpdated = post.ModifiedDate,
                        ContentType = "html",
                    };

                    foreach (var tag in post.BlogStoryTags)
                    {
                        item.AddCategory(new SyndicationCategory(tag.Tag.Name));
                    }

                    item.AddContributor(new SyndicationPerson("Danil Pavlov", "d2funlife@gmail.com", "author"));
//                    item.AddLink(new SyndicationLink(new Uri($"{host}/{post.Alias}")));

                    await atom.Write(item);
                }
            }
        }
    }
}
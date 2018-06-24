using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Entities;
using Blog.Core.Exceptions;
using Blog.Data.Contracts.Repositories;

namespace Blog.BusinessLogic.Managers
{
    public class TagManager : ITagManager
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogStoryRepository _blogStoryRepository;
        private readonly IBlogStoryTagRepository _blogStoryTagRepository;

        public TagManager(ITagRepository tagRepository,
                          IBlogStoryRepository blogStoryRepository,
                          IBlogStoryTagRepository blogStoryTagRepository)
        {
            _tagRepository = tagRepository;
            _blogStoryRepository = blogStoryRepository;
            _blogStoryTagRepository = blogStoryTagRepository;
        }

        public Task<Tag> GetAsync(Int32 id, CancellationToken cancel = default)
        {
            if (id <= 0)
            {
                return Task.FromResult<Tag>(null);
            }

            return _tagRepository.GetAsync(id, cancel);
        }

        public Task<List<Tag>> GetAllAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllAsync(cancel);
        }

        public Task<List<Tag>> GetAsync(IEnumerable<Int32> ids, CancellationToken cancel = default)
        {
            return _tagRepository.WhereAsync(x => ids.Contains(x.Id), cancel);
        }

        public Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllOrderedByUseAsync(cancel);
        }

        public async Task DeleteAsync(Int32 tagId, CancellationToken cancel = default)
        {
            if (tagId <= 0)
            {
                throw new ArgumentException(nameof(tagId));
            }

            var originalTag = await _tagRepository.GetAsync(tagId, cancel);
            if (originalTag == null)
            {
                throw new EntityNotFoundException($"Can't find tag with tag id {tagId.ToString()}");
            }

            await _tagRepository.DeleteAsync(originalTag, cancel);
        }

        public Task<Int32> CountAsync(CancellationToken cancel = default)
        {
            return _tagRepository.CountAsync(cancel);
        }

        public async Task AssignToBlogStoryAsync(IEnumerable<Int32> tagIds,
                                                 BlogStory story,
                                                 CancellationToken cancel = default)
        {
            var ids = tagIds as Int32[] ?? tagIds.ToArray();
            if (!ids.Any())
            {
                throw new ArgumentException(nameof(tagIds));
            }

            if (story == null)
            {
                throw new ArgumentNullException(nameof(story));
            }

            var tagsForRemove = await _blogStoryTagRepository.WhereAsync(x => x.BlogStoryId == story.Id, cancel);
            if (tagsForRemove.Any())
            {
                await _blogStoryTagRepository.DeleteRangeAsync(tagsForRemove, cancel);
            }

            var blogStoryTags = ids.Select(x => new BlogStoryTag(story.Id, x)).ToList();
            await _blogStoryTagRepository.AddRangeAsync(blogStoryTags, cancel);
        }

        public async Task<Tag> CreateTagAsync(String name, CancellationToken cancel = default)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var tag = new Tag(name);
            await _tagRepository.AddAsync(tag, cancel);
            return tag;
        }

        public async Task<BlogStory> AssignTagToBlogStoryAsync(Int32 tagId,
                                                               Int32 blogStoryId,
                                                               CancellationToken cancel = default)
        {
            if (tagId <= 0)
            {
                throw new ArgumentException(nameof(tagId));
            }

            if (blogStoryId <= 0)
            {
                throw new ArgumentException(nameof(blogStoryId));
            }

            var blogStory = await _blogStoryRepository.GetWithBlogStoryTagsAsync(blogStoryId, cancel);
            if (blogStory == null)
            {
                throw new EntityNotFoundException();
            }

            return await AssignTagToBlogStory(tagId, blogStory, cancel);
        }

        public async Task<BlogStory> AssignTagToBlogStory(Int32 tagId, BlogStory blogStory,
                                                          CancellationToken cancel = default)
        {
            if (tagId <= 0)
            {
                throw new ArgumentException(nameof(tagId));
            }

            if (blogStory == null)
            {
                throw new ArgumentNullException(nameof(blogStory));
            }

            if (blogStory.BlogStoryTags.Count >= 3)
            {
                throw new EntityRelationshipException($"Blog story already have reach relationship limit (3 entities)");
            }

            await _blogStoryTagRepository.AddAsync(new BlogStoryTag(blogStory.Id, tagId), cancel);
            return blogStory;
        }

        public async Task<BlogStory> UnassignTagFromBlogStoryAsync(Int32 tagId, Int32 blogStoryId,
                                                                   CancellationToken cancel = default)
        {
            if (tagId <= 0)
            {
                throw new ArgumentException(nameof(tagId));
            }

            if (blogStoryId <= 0)
            {
                throw new ArgumentException(nameof(blogStoryId));
            }

            var blogStoryWithTags = await _blogStoryRepository.GetWithBlogStoryTagsAsync(blogStoryId, cancel);
            if (blogStoryWithTags == null)
            {
                throw new EntityNotFoundException($"Can't find blog story with id : {blogStoryId} for unassign tag");
            }

            if (blogStoryWithTags.BlogStoryTags.All(x => x.TagId != tagId))
            {
                throw new EntityRelationshipException($"Tag with id : {tagId} have not belong to blog story with id : {blogStoryId.ToString()}");
            }

            await _blogStoryTagRepository.DeleteAsync(blogStoryWithTags.BlogStoryTags.First(x => x.TagId == tagId),
                                                      cancel);

            return blogStoryWithTags;
        }

        public Task<Tag> GetTagWithBlogStoryTagsAsync(String alias, CancellationToken cancel = default)
        {
            return String.IsNullOrWhiteSpace(alias) 
                       ? null 
                       : _tagRepository.GetTagWithBlogStoryTagsAsync(alias, cancel);
        }

        public async Task<Tag> UpdateAsync(Tag tag, CancellationToken cancel = default)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            var exitingTag = await _tagRepository.GetAsync(tag.Id, cancel);
            if (exitingTag == null)
            {
                throw new EntityNotFoundException();
            }

            exitingTag.Update(tag);
            await _tagRepository.UpdateAsync(exitingTag, cancel);
            return exitingTag;
        }

        public Task<Int32> GetStoriesCountAsync(Int32 tagId, CancellationToken cancel = default)
        {
            return _blogStoryTagRepository.CountAsync(x => x.TagId == tagId, cancel);
        }
    }
}
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

        public Task<Tag> GetAsync(Guid id,
                                  CancellationToken cancel = default)
        {
            return id == Guid.Empty
                ? Task.FromResult<Tag>(null)
                : _tagRepository.GetAsync(id, cancel);
        }

        public Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllPublishedAsync(cancel);
        }

        public Task<List<Tag>> GetAllAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllAsync(cancel);
        }

        public Task<List<Tag>> GetAsync(IEnumerable<Guid> ids,
                                        CancellationToken cancel = default)
        {
            return _tagRepository.WhereAsync(x => ids.Contains(x.Id), cancel);
        }

        public Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllOrderedByUseAsync(cancel);
        }

        public async Task DeleteAsync(Guid tagId,
                                      CancellationToken cancel = default)
        {
            var originalTag = await _tagRepository.GetAsync(tagId, cancel);
            if(originalTag == null)
            {
                throw new EntityNotFoundException($"Can't find tag with tag id {tagId.ToString()}");
            }

            await _tagRepository.DeleteAsync(originalTag, cancel);
        }

        public Task<Int32> CountAsync(CancellationToken cancel = default)
        {
            return _tagRepository.CountAsync(cancel);
        }

        public async Task UpdateBlogStoryTagsAsync(List<Guid> tagIds,
                                                   BlogStory story,
                                                   CancellationToken cancel = default)
        {
            if(story == null)
            {
                throw new ArgumentNullException(nameof(story));
            }

            var existTags = await _blogStoryTagRepository.WhereAsync(x => x.BlogStoryId == story.Id, cancel);
            var isAllTagsRemoved = existTags.Any() && (tagIds == null || !tagIds.Any());
            if(isAllTagsRemoved)
            {
                await _blogStoryTagRepository.DeleteRangeAsync(existTags, cancel);
            }
            else if(tagIds != null)
            {
                var existTagIds = existTags.Select(x => x.TagId).ToList();

                var tagIdsToRemove = existTagIds.Except(tagIds).ToList();
                if(tagIdsToRemove.Any())
                {
                    await _blogStoryTagRepository.DeleteRangeAsync(existTags.Where(x => tagIdsToRemove.Contains(x.TagId)), cancel);
                }

                var blogStoryTagsForCreate = tagIds.Except(existTagIds).Select(x => new BlogStoryTag(story.Id, x)).ToList();
                if(blogStoryTagsForCreate.Any())
                {
                    await _blogStoryTagRepository.AddRangeAsync(blogStoryTagsForCreate, cancel);
                }
            }
        }

        public async Task<Tag> CreateTagAsync(String name,
                                              CancellationToken cancel = default)
        {
            if(String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }

            var tag = new Tag(name);
            await _tagRepository.AddAsync(tag, cancel);
            return tag;
        }

        public async Task<BlogStory> AssignTagToBlogStoryAsync(Guid tagId,
                                                               Guid blogStoryId,
                                                               CancellationToken cancel = default)
        {
            var blogStory = await _blogStoryRepository.GetWithBlogStoryTagsAsync(blogStoryId, cancel);
            if(blogStory == null)
            {
                throw new EntityNotFoundException();
            }

            return await AssignTagToBlogStory(tagId, blogStory, cancel);
        }

        public async Task<BlogStory> AssignTagToBlogStory(Guid tagId,
                                                          BlogStory blogStory,
                                                          CancellationToken cancel = default)
        {
            if(blogStory == null)
            {
                throw new ArgumentNullException(nameof(blogStory));
            }

            if(blogStory.BlogStoryTags.Count >= 3)
            {
                throw new EntityRelationshipException("Blog story already have reach relationship limit (3 entities)");
            }

            await _blogStoryTagRepository.AddAsync(new BlogStoryTag(blogStory.Id, tagId), cancel);
            return blogStory;
        }

        public async Task<BlogStory> UnassignTagFromBlogStoryAsync(Guid tagId,
                                                                   Guid blogStoryId,
                                                                   CancellationToken cancel = default)
        {
            var blogStoryWithTags = await _blogStoryRepository.GetWithBlogStoryTagsAsync(blogStoryId, cancel);
            if(blogStoryWithTags == null)
            {
                throw new EntityNotFoundException($"Can't find blog story with id : {blogStoryId} for unassign tag");
            }

            if(blogStoryWithTags.BlogStoryTags.All(x => x.TagId != tagId))
            {
                throw new EntityRelationshipException($"Tag with id : {tagId} have not belong to blog story with id : {blogStoryId.ToString()}");
            }

            await _blogStoryTagRepository.DeleteAsync(blogStoryWithTags.BlogStoryTags.First(x => x.TagId == tagId),
                                                      cancel);

            return blogStoryWithTags;
        }

        public Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                                      CancellationToken cancel = default)
        {
            return String.IsNullOrWhiteSpace(alias)
                ? null
                : _tagRepository.GetTagWithBlogStoryTagsAsync(alias, cancel);
        }

        public async Task<Tag> UpdateAsync(Tag tag,
                                           CancellationToken cancel = default)
        {
            if(tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            var exitingTag = await _tagRepository.GetAsync(tag.Id, cancel);
            if(exitingTag == null)
            {
                throw new EntityNotFoundException();
            }

            exitingTag.Update(tag);
            await _tagRepository.UpdateAsync(exitingTag, cancel);
            return exitingTag;
        }

        public Task<Int32> GetStoriesCountAsync(Guid tagId,
                                                CancellationToken cancel = default)
        {
            return _blogStoryTagRepository.CountAsync(x => x.TagId == tagId, cancel);
        }
    }
}
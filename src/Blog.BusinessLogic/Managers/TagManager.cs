using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Contracts.Managers;
using Blog.Core.Entities;
using Blog.Core.Exceptions;
using Blog.Core.Helpers;
using Blog.Core.Queries;
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


        public Task<Page<Tag>> GetAsync(TagsQuery query,
                                        CancellationToken cancel = default)
        {
            return _tagRepository.GetPageAsync(query, cancel);
        }

        public Task<Tag> GetAsync(Guid id,
                                  CancellationToken cancel = default)
        {
            return id == Guid.Empty
                ? Task.FromResult<Tag>(null)
                : _tagRepository.GetAsync(id, cancel);
        }

        public Task<Tag> GetAsync(String alias,
                                  CancellationToken cancel = default)
        {
            return _tagRepository.GetAsync(alias, cancel);
        }

        public Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetAllPublishedAsync(cancel);
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

        public async Task UpdateBlogStoryTagsAsync(BlogStory story,
                                                   CancellationToken cancel = default)
        {
            if(story == null)
            {
                throw new ArgumentNullException(nameof(story));
            }

            var existTagIds = await _blogStoryTagRepository.GetIdsByStoryIdAsync(story.Id, cancel);
            var isAddingNewSet = existTagIds.IsEmpty() &&
                                 story.BlogStoryTags.IsNotEmpty();
            if(isAddingNewSet)
            {
                await _blogStoryTagRepository.AddRangeAsync(story.BlogStoryTags, cancel);
                return;
            }

            
            if(story.BlogStoryTags.IsEmpty())
            {
                if(existTagIds.IsNotEmpty())
                {
                    await _blogStoryTagRepository.DeleteRangeAsync(existTagIds, story.Id, cancel);
                }

                return;
            }

            var targetTagIds = story.BlogStoryTags.Select(x => x.TagId).ToList();
            var tagsForRemove = existTagIds.Except(targetTagIds).ToList();
            var tagsForAdding = targetTagIds.Except(existTagIds).ToList();

            if(tagsForAdding.IsNotEmpty())
            {
                await _blogStoryTagRepository.AddRangeAsync(story.BlogStoryTags.Where(x => tagsForAdding.Contains(x.TagId)).ToList(), cancel);
            }

            if(tagsForRemove.IsNotEmpty())
            {
                await _blogStoryTagRepository.DeleteRangeAsync(tagsForRemove, story.Id, cancel);
            }
        }

        public async Task<Tag> CreateTagAsync(Tag tag,
                                              CancellationToken cancel = default)
        {
            if(tag == null)
            {
                throw new ArgumentException(nameof(tag));
            }

            await _tagRepository.AddAsync(tag, cancel);
            return tag;
        }

        public async Task<BlogStory> AssignTagToBlogStoryAsync(Guid tagId,
                                                               Guid blogStoryId,
                                                               CancellationToken cancel = default)
        {
            var blogStory = await _blogStoryRepository.GetWithTagsAsync(blogStoryId, cancel);
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
            var blogStoryWithTags = await _blogStoryRepository.GetWithTagsAsync(blogStoryId, cancel);
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

        public Task<List<Tag>> GetTopPublishedAsync(CancellationToken cancel = default)
        {
            return _tagRepository.GetTopPublishedAsync(cancel);
        }

        public Task<Guid> GetTagIdAsync(String alias,
                                        CancellationToken cancel = default)
        {
            return _tagRepository.GetTagIdAsync(alias, cancel);
        }
    }
}
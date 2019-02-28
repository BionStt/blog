﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Core.Queries;

namespace Blog.Core.Contracts.Managers
{
    public interface IBlogStoryManager
    {
        Task<Page<BlogStory>> GetAsync(StoriesQuery query,
                                       CancellationToken cancel = default);

        Task<BlogStory> GetAsync(String alias,
                                 CancellationToken cancel = default);
        
        Task<BlogStory> GetWithTagsAsync(Guid id,
                                         CancellationToken cancel = default);
        
        Task<BlogStory> GetWithTagsAsync(String alias,
                                         CancellationToken cancel = default);
        
        Task<Tuple<Tag, List<BlogStory>>> GetTagStoriesByAliasAsync(String alias,
                                                                    Int32 skip,
                                                                    Int32 top,
                                                                    StorySort sort,
                                                                    StoryFilter filter,
                                                                    CancellationToken cancel = default);

        Task<BlogStory> CreateOrUpdateAsync(BlogStory story,
                                            CancellationToken cancel = default);

        Task<BlogStory> UpdateAccessTokenAsync(Guid id,
                                               CancellationToken cancel = default);

        Task<BlogStory> ChangeAvailabilityAsync(Guid id,
                                                Boolean isPublished,
                                                CancellationToken cancel = default);
        
        Task DeleteAsync(String alias,
                         CancellationToken cancel = default);

        Task<String> GetSiteMapXmlAsync(String baseUrl,
                                        CancellationToken cancel = default);

        Task<Int32> CountPublishedAsync(CancellationToken cancel = default);
        
        Task<Int32> CountStoriesForTagAsync(Guid tagId,
                                            CancellationToken cancel = default);

        Task RemoveAccessTokenAsync(Guid storyId, CancellationToken cancel);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;

namespace Blog.Core.Contracts.Managers
{
    public interface IBlogStoryManager : IEntityManager<BlogStory>
    {
        /// <summary>
        /// Get blog story
        /// </summary>
        /// <param name="alias">Blog alias</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog story entity</returns>
        Task<BlogStory> GetAsync(String alias,
                                 CancellationToken cancel = default);
        
        /// <summary>
        /// Get blog story with associated tags
        /// </summary>
        /// <param name="id">Blog story id</param>
        /// <param name="cancel">Cancellation token</param>
        Task<BlogStory> GetWithTagsAsync(Int32 id,
                                         CancellationToken cancel = default);
        
        /// <summary>
        /// Get blog story with associated tags
        /// </summary>
        /// <param name="alias">Blog story alias</param>
        /// <param name="cancel">Cancellation token</param>
        Task<BlogStory> GetWithTagsAsync(String alias,
                                         CancellationToken cancel = default);
        
        /// <summary>
        /// Get blog stories collection
        /// </summary>
        /// <param name="skip">Amount stories for skip</param>
        /// <param name="top">Amount stories for take</param>
        /// <param name="sort">Sort type</param>
        /// <param name="filter">Filter type</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog stories collection</returns>
        Task<List<BlogStory>> GetAsync(Int32 skip,
                                       Int32 top,
                                       StorySort sort,
                                       StoryFilter filter,
                                       CancellationToken cancel = default);

        /// <summary>
        /// Get blog stories collection associated with tag
        /// </summary>
        /// <param name="alias">Tag alias</param>
        /// <param name="skip">Amount stories for skip</param>
        /// <param name="top">Amount stories for take</param>
        /// <param name="sort">Sort type</param>
        /// <param name="filter">Filter type</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tuple for tag and stories collection</returns>
        Task<Tuple<Tag, List<BlogStory>>> GetTagStoriesByAliasAsync(String alias,
                                                                    Int32 skip,
                                                                    Int32 top,
                                                                    StorySort sort,
                                                                    StoryFilter filter,
                                                                    CancellationToken cancel = default);

        /// <summary>
        /// Create or update blog story
        /// </summary>
        /// <param name="story">Blog story entity</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog story entity</returns>
        Task<BlogStory> CreateOrUpdateAsync(BlogStory story,
                                            CancellationToken cancel = default);

        /// <summary>
        /// Create access token for blog story
        /// </summary>
        /// <param name="id">Blog story id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Single blog story</returns>
        Task<BlogStory> UpdateAccessTokenAsync(Int32 id,
                                               CancellationToken cancel = default);

        /// <summary>
        /// Change blog story published status
        /// </summary>
        /// <param name="id">Blog story id</param>
        /// <param name="isPublished">Published status value</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog story entity</returns>
        Task<BlogStory> ChangeAvailabilityAsync(Int32 id,
                                                Boolean isPublished,
                                                CancellationToken cancel = default);
        
        /// <summary>
        /// Delete blog story
        /// </summary>
        /// <param name="alias">Blog story alias</param>
        /// <param name="cancel">Cancellation token</param>
        Task DeleteAsync(String alias,
                         CancellationToken cancel = default);

        /// <summary>
        /// Get site map xml file content
        /// </summary>
        /// <param name="baseUrl">Base site url</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Site map content</returns>
        Task<String> GetSiteMapXmlAsync(String baseUrl,
                                        CancellationToken cancel = default);

        /// <summary>
        /// Get published blog stories count
        /// </summary>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Count of blog stories</returns>
        Task<Int32> CountPublishedAsync(CancellationToken cancel = default);
        
        /// <summary>
        /// Get stories coung for tag
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Count of blog stories</returns>
        Task<Int32> CountStoriesForTagAsync(Int32 tagId,
                                            CancellationToken cancel = default);

        /// <summary>
        /// Remove story access token
        /// </summary>
        /// <param name="storyId">Blog story id</param>
        /// <param name="cancel">Cancellation token</param>
        Task RemoveAccessTokenAsync(Int32 storyId, CancellationToken cancel);
    }
}
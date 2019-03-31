using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Blog.Core.Queries;

namespace Blog.Core.Contracts.Managers
{
    public interface ITagManager
    {
        Task<Page<Tag>> GetAsync(TagsQuery query, CancellationToken cancel = default);

        /// <summary>
        /// Get tag
        /// </summary>
        /// <param name="id">Tag id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entities</returns>
        Task<Tag> GetAsync(Guid id,
                           CancellationToken cancel = default);

        /// <summary>
        /// Get tag
        /// </summary>
        /// <param name="alias">Tag alias</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entities</returns>
        Task<Tag> GetAsync(String alias,
                           CancellationToken cancel = default);

        /// <summary>
        /// Get all published
        /// </summary>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entities</returns>
        Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default);
        
        /// <summary>
        /// Delete tag
        /// </summary>
        /// <param name="id">Tag id</param>
        /// <param name="cancel">Cancellation token</param>
        Task DeleteAsync(Guid id,
                         CancellationToken cancel = default);

        /// <summary>
        /// Assign tag to blog story
        /// </summary>
        /// <param name="tagIds">Tag id</param>
        /// <param name="story">Blog story entity</param>
        /// <param name="cancel">Cancellation token</param>
        Task UpdateBlogStoryTagsAsync(List<Guid> tagIds,
                                      BlogStory story,
                                      CancellationToken cancel = default);

        /// <summary>
        /// Create tag
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entity</returns>
        Task<Tag> CreateTagAsync(String name,
                                 CancellationToken cancel = default);

        /// <summary>
        /// Assign tag to blog story
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="blogStoryId">Blog story id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog story entity</returns>
        Task<BlogStory> AssignTagToBlogStoryAsync(Guid tagId,
                                                  Guid blogStoryId,
                                                  CancellationToken cancel = default);

        /// <summary>
        /// Unassign tag from blog story
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="blogStoryId">Blog story id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog story entity</returns>
        Task<BlogStory> UnassignTagFromBlogStoryAsync(Guid tagId,
                                                      Guid blogStoryId,
                                                      CancellationToken cancel = default);

        /// <summary>
        /// Update tag
        /// </summary>
        /// <param name="tag">Tag entity</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entity</returns>
        Task<Tag> UpdateAsync(Tag tag,
                              CancellationToken cancel = default);

        Task<List<Tag>> GetTopPublishedAsync(CancellationToken cancel = default);

        Task<Guid> GetTagIdAsync(String alias,
                                 CancellationToken cancel = default);
    }
}
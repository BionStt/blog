using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Core.Contracts.Managers
{
    public interface ITagManager : IEntityManager<Tag>
    {
        /// <summary>
        /// Get tag
        /// </summary>
        /// <param name="id">Tag id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entities</returns>
        Task<Tag> GetAsync(Int32 id,
                           CancellationToken cancel = default);

        /// <summary>
        /// Get all published
        /// </summary>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entities</returns>
        Task<List<Tag>> GetAllPublishedAsync(CancellationToken cancel = default);

        /// <summary>
        /// Get tags collection
        /// </summary>
        /// <param name="ids">Tag ids for getting</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tags collection</returns>
        Task<List<Tag>> GetAsync(IEnumerable<Int32> ids,
                                 CancellationToken cancel = default);

        /// <summary>
        /// Get tags ordered by usage
        /// </summary>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tags collection</returns>
        Task<List<Tag>> GetAllOrderedByUseAsync(CancellationToken cancel = default);

        /// <summary>
        /// Delete tag
        /// </summary>
        /// <param name="id">Tag id</param>
        /// <param name="cancel">Cancellation token</param>
        Task DeleteAsync(Int32 id,
                         CancellationToken cancel = default);

        /// <summary>
        /// Assign tag to blog story
        /// </summary>
        /// <param name="tagIds">Tag id</param>
        /// <param name="story">Blog story entity</param>
        /// <param name="cancel">Cancellation token</param>
        Task UpdateBlogStoryTagsAsync(List<Int32> tagIds,
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
        Task<BlogStory> AssignTagToBlogStoryAsync(Int32 tagId,
                                                  Int32 blogStoryId,
                                                  CancellationToken cancel = default);

        /// <summary>
        /// Unassign tag from blog story
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="blogStoryId">Blog story id</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Blog story entity</returns>
        Task<BlogStory> UnassignTagFromBlogStoryAsync(Int32 tagId,
                                                      Int32 blogStoryId,
                                                      CancellationToken cancel = default);

        /// <summary>
        /// Get tag with assigned blog stories
        /// </summary>
        /// <param name="alias">Tag alias</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entity</returns>
        Task<Tag> GetTagWithBlogStoryTagsAsync(String alias,
                                               CancellationToken cancel = default);

        /// <summary>
        /// Update tag
        /// </summary>
        /// <param name="tag">Tag entity</param>
        /// <param name="cancel">Cancellation token</param>
        /// <returns>Tag entity</returns>
        Task<Tag> UpdateAsync(Tag tag,
                              CancellationToken cancel = default);

        /// <summary>
        /// Get blog stories count assigned for tag
        /// </summary>
        /// <param name="tagId">Tag id</param>
        /// <param name="cancel">Cancellation tocken</param>
        /// <returns>Count of blog stories</returns>
        Task<Int32> GetStoriesCountAsync(Int32 tagId,
                                         CancellationToken cancel = default);
    }
}
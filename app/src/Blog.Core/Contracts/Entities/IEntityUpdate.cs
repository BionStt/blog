namespace Blog.Core.Contracts.Entities
{
    public interface IEntityUpdate<T>
    {
        /// <summary>
        /// Update entity
        /// </summary>
        void Update(T targetEntity);
    }
}
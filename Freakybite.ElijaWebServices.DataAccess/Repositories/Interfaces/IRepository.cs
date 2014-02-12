using System;
using System.Linq;

namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces
{
    using System.Linq.Expressions;

    public interface IRepository<T> : IDisposable where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Add(T entity);

        /// <summary>
        /// The count.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int Count();

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Delete(T entity);

        /// <summary>
        /// The find all by.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <param name="include">
        /// The include.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate, string[] include);

        /// <summary>
        /// The find first by.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T FindFirstBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// The max entity.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T MaxEntity();

        /// <summary>
        /// The refresh.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Refresh(T entity);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        void Update(T entity);

        #endregion
    }
}

using System;
using System.Linq;

namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations
{
    using System.Data;
    using System.Data.Objects;
    using System.Linq.Expressions;

    using Freakybite.ElijaWebServices.DataAccess.Model;
    using Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces;

    public abstract class Repository<T> : IRepository<T>
        where T : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="contextFactory">
        /// The database context factory.
        /// </param>
        protected Repository(IDbContextFactory contextFactory)
        {
            Context = contextFactory.GetDbContext();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ElijaEntities Context{ get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Add(T entity)
        {
            Context.CreateObjectSet<T>().AddObject(entity);
        }

        /// <summary>
        /// The count.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int Count()
        {
            return Context.CreateObjectSet<T>().Count<T>();
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Delete(T entity)
        {
            Context.CreateObjectSet<T>().DeleteObject(entity);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

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
        public IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate, string[] include)
        {
            var objset = Context.CreateObjectSet<T>();
            var query = objset as ObjectQuery<T>;
            if (include != null)
            {
                query = include.Aggregate(query, (current, navprop) => current.Include(navprop));
            }

            return query.Where(predicate).AsQueryable<T>();
        }

        /// <summary>
        /// The find first by.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T FindFirstBy(Expression<Func<T, bool>> predicate)
        {
            return Context.CreateObjectSet<T>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<T> GetAll()
        {
            return Context.CreateObjectSet<T>().AsQueryable<T>();
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="include">
        /// The include.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<T> GetAll(string[] include)
        {
            var objset = Context.CreateObjectSet<T>();
            var query = objset as ObjectQuery<T>;
            if (include != null)
            {
                query = include.Aggregate(query, (current, navprop) => current.Include(navprop));
            }

            return query.AsQueryable();
        }

        /// <summary>
        /// The max entity.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T MaxEntity()
        {
            try
            {
                return Context.CreateObjectSet<T>().ToList().Last();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// The refresh.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Refresh(T entity)
        {
            Context.Refresh(RefreshMode.StoreWins, entity);
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int Save()
        {
            return Context.SaveChanges();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Update(T entity)
        {
            Context.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }

        #endregion
    }
}

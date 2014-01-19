using System;
using Freakybite.ElijaWebServices.DataAccess.Model;
using Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces;

namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations
{
    public class DbContextFactory : IDbContextFactory
    {
        #region Fields

        /// <summary>
        ///     The _context.
        /// </summary>
        private readonly ElijaEntities context;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContextFactory" /> class.
        /// </summary>
        public DbContextFactory()
        {
            context = new ElijaEntities();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        /// Finalize context.
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        ///     The get database context.
        /// </summary>
        /// <returns>
        ///     The <see cref="ElijaContext" />.
        /// </returns>
        public ElijaEntities GetDbContext()
        {
            return context;
        }

        #endregion
    }
}
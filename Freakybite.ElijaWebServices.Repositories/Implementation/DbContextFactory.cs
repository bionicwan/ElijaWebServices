using System;
using Freakybite.ElijaWebServices.DataAccess;
using Freakybite.ElijaWebServices.Repositories.Interfaces;

namespace Freakybite.ElijaWebServices.Repositories.Implementation
{
    public class DbContextFactory : IDbContextFactory
    {
        #region Fields

        /// <summary>
        /// The _context.
        /// </summary>
        private readonly ElijaContext context;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        public DbContextFactory()
        {
            this.context = new ElijaContext();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose.
        /// </summary>
        /// Finalize context.
        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// The get database context.
        /// </summary>
        /// <returns>
        /// The <see cref="MobileGammingPlatformConn"/>.
        /// </returns>
        public ElijaContext GetDbContext()
        {
            return this.context;
        }

        #endregion
    }
}

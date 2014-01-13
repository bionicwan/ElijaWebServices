using System;

namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces
{
    using Freakybite.ElijaWebServices.DataAccess.Model;

    public interface IDbContextFactory : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        /// The database context.
        /// </summary>
        /// <returns>
        /// The database context instance.
        /// </returns>
        ElijaEntities GetDbContext();

        #endregion
    }
}

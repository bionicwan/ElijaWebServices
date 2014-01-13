using System;
using Freakybite.ElijaWebServices.DataAccess;

namespace Freakybite.ElijaWebServices.Repositories.Interfaces
{
    public interface IDbContextFactory : IDisposable
    {
        #region Public Methods and Operators

        /// <summary>
        /// The database context.
        /// </summary>
        /// <returns>
        /// The database context instance.
        /// </returns>
        ElijaContext GetDbContext();

        #endregion
    }
}

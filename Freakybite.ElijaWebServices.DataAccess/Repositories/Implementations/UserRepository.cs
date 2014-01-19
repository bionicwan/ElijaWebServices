using Freakybite.ElijaWebServices.DataAccess.Model;
using Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces;

namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations
{
    public class UserRepository : Repository<User>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserRepository" /> class.
        /// </summary>
        /// <param name="contextFactory">
        ///     The database context factory.
        /// </param>
        public UserRepository(IDbContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        #endregion
    }
}
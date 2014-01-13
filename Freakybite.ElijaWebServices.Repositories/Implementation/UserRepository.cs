using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freakybite.ElijaWebServices.Repositories.Implementation
{
    public class UserRepository : Repository<User>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class. 
        /// Initializes a new instance of the <see cref="GameRepository"/> class.
        /// </summary>
        /// <param name="contextFactory">
        /// The database context factory.
        /// </param>
        public UserRepository(IDbContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        #endregion
    }
}

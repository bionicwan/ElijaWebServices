using System;
using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;

namespace Freakybite.ElijaWebServices.Processing.Helpers
{
    public class ValidationHelper
    {
        #region Fields

        private readonly DbContextFactory context = new DbContextFactory();

        private UserRepository userRepository;

        #endregion

        #region Public Properties

        public UserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(context);
                }

                return userRepository;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool ValidateToken(string userToken)
        {
            var token = new Guid();

            if (!Guid.TryParse(userToken, out token))
            {
                return false;
            }
            
            var user = UserRepository.FindFirstBy(e => e.Token == token);

            return user != null;
        }

        #endregion
    }
}

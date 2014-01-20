using System;
using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;

namespace Freakybite.ElijaWebServices.Processing.Helpers
{
    public class ValidationHelper
    {
        private readonly DbContextFactory context = new DbContextFactory();

        private UserRepository userRepository;

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
    }
}

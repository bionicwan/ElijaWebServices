namespace Freakybite.ElijaWebServices.Processing.Helpers
{
    using System;

    using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;

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
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository(this.context);
                }

                return this.userRepository;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool ValidateToken(string token)
        {
            var userToken = new Guid(token);
            var user = this.UserRepository.FindFirstBy(e => e.Token == userToken);

            return user != null;
        }

        #endregion
    }
}

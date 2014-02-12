namespace Freakybite.ElijaWebServices.Processing.Helpers
{
    using System;

    using Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations;

    public class ValidationHelper
    {
        #region Fields

        private readonly UnitOfWork unitOfWork = new UnitOfWork();

        #endregion

        #region Public Methods and Operators

        public bool ValidateToken(string token)
        {
            var userToken = new Guid(token);
            var user = this.unitOfWork.UserRepository.FindFirstBy(e => e.Token == userToken);

            return user != null;
        }

        #endregion
    }
}

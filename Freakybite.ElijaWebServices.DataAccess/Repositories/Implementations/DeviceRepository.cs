namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations
{
    using Freakybite.ElijaWebServices.DataAccess.Model;
    using Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces;

    public class DeviceRepository : Repository<Device>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class. 
        /// </summary>
        /// <param name="contextFactory">
        /// The database context factory.
        /// </param>
        public DeviceRepository(IDbContextFactory contextFactory)
            : base(contextFactory)
        {
        }
    }
}

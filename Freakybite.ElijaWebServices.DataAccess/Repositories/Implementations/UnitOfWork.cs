using System;

namespace Freakybite.ElijaWebServices.DataAccess.Repositories.Implementations
{
    using Freakybite.ElijaWebServices.DataAccess.Model;
    using Freakybite.ElijaWebServices.DataAccess.Repositories.Interfaces;

    public class UnitOfWork : IDisposable
    {

        private ElijaEntities Context { get; set; }

        private readonly DbContextFactory dbContextFactory = new DbContextFactory();

        private Repository<User> userRepository;

        private Repository<UserDevice> userDeviceRepository;

        private Repository<Device> deviceRepository;

        public UnitOfWork()
        {
            this.Context = dbContextFactory.GetDbContext();
        }

        public Repository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new Repository<User>(this.Context);
                }
                return userRepository;
            }
        }

        public Repository<UserDevice> UserDeviceRepository
        {
            get
            {
                if (this.userDeviceRepository == null)
                {
                    this.userDeviceRepository = new Repository<UserDevice>(this.Context);
                }
                return userDeviceRepository;
            }
        }

        public Repository<Device> DeviceRepository
        {
            get
            {
                if (this.deviceRepository == null)
                {
                    this.deviceRepository = new Repository<Device>(this.Context);
                }
                return deviceRepository;
            }
        }

        public void Save()
        {
            this.Context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

namespace BLogicLicense.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private BLogicLicenseDbContext dbContext;

        public BLogicLicenseDbContext Init()
        {
            return dbContext ?? (dbContext = new BLogicLicenseDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
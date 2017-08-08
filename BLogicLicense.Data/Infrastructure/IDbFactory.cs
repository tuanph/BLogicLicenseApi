using System;

namespace BLogicLicense.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BLogicLicenseDbContext Init();
    }
}
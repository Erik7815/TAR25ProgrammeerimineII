using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ShopTARpe25.RealEstateTest.Mock
{
    public class MockIHostEnvironment : IHostEnvironment
    {
        string IHostEnvironment.EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IHostEnvironment.ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IHostEnvironment.ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        IFileProvider IHostEnvironment.ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

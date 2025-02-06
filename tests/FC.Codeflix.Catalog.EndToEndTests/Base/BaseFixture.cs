using Bogus;
using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class BaseFixture
{
    protected Faker Faker { get; set; }
    public ApiClient ApiClient { get; set; }
    public HttpClient HttpClient { get; set; }
    public CustomWebApplicationFactory<Program> WebappFactory { get; set; }

    private readonly string _dbConnectionString;

    protected BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebappFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebappFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
        var configuration = WebappFactory.Services.GetService(typeof(IConfiguration));
        
        if (configuration is null)
            ArgumentNullException.ThrowIfNull(configuration);
        
        _dbConnectionString = ((IConfiguration)configuration).GetConnectionString("CatalogDB");
    }

    protected CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new CodeflixCatalogDbContext(
            new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString))
                .Options
        );
        return context;
    }

    public void CleanPersistence()
    {
        var context = CreateDbContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
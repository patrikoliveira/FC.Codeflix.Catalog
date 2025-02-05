using Bogus;
using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class BaseFixture
{
    protected Faker Faker { get; set; }
    public ApiClient ApiClient { get; set; }
    public HttpClient HttpClient { get; set; }
    public CustomWebApplicationFactory<Program> WebappFactory { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebappFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebappFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }
    
    public CodeflixCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new CodeflixCatalogDbContext(
            new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
                .UseInMemoryDatabase($"end2end-tests-db")
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
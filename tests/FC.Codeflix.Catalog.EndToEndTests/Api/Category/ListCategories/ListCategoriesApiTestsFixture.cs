using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories;

[CollectionDefinition(nameof(ListCategoriesApiTestsFixture))]
public class ListCategoriesApiTestsFixtureCollection : ICollectionFixture<ListCategoriesApiTestsFixture> {}

public class ListCategoriesApiTestsFixture : CategoryBaseFixture
{
    public List<Domain.Entity.Category> GetExampleCategoriesListWithNames(List<string> names) => names.Select(name =>
    {
        var category = GetExampleCategory();
        category.Update(name);
        return category;
    }).ToList();
    
    public List<Domain.Entity.Category> CloneCategoriesListOrdered(List<Domain.Entity.Category> categoriesList, string orderBy, SearchOrder order) 
    {
        var listClone = new List<Domain.Entity.Category>(categoriesList);
        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name),
        };
        return orderedEnumerable.ThenBy(x => x.CreatedAt).ToList();
    }
}
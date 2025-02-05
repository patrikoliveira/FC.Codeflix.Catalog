using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory;

[CollectionDefinition(nameof(GetCategoryApiTestsFixture))]
public class GetCategoryApiTestsFixtureCollection : ICollectionFixture<GetCategoryApiTestsFixture>{}

public class GetCategoryApiTestsFixture : CategoryBaseFixture
{
}
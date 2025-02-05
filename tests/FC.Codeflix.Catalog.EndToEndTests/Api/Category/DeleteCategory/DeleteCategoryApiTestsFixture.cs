using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[CollectionDefinition(nameof(DeleteCategoryApiTestsFixture))]
public class DeleteCategoryApiTestsFixtureCollection : ICollectionFixture<DeleteCategoryApiTestsFixture> {}

public class DeleteCategoryApiTestsFixture : CategoryBaseFixture
{
}
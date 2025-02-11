using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryApiTestsFixture))]
public class UpdateCategoryApiTestsFixtureCollection : ICollectionFixture<UpdateCategoryApiTestsFixture>{}

public class UpdateCategoryApiTestsFixture : CategoryBaseFixture
{
    public UpdateCategoryApiInput GetExampleInput() => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
}
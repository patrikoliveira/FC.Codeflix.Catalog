using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[CollectionDefinition(nameof(UpdateCategoryApiTestsFixture))]
public class UpdateCategoryApiTestsFixtureCollection : ICollectionFixture<UpdateCategoryApiTestsFixture>{}

public class UpdateCategoryApiTestsFixture : CategoryBaseFixture
{
    public UpdateCategoryInput GetExampleInput(Guid? id = null) => new(
        id ?? Guid.NewGuid(),
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );
}
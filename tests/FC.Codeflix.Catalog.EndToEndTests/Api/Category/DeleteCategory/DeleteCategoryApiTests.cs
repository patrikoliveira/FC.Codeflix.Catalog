using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory;

[Collection(nameof(DeleteCategoryApiTestsFixture))]
public class DeleteCategoryApiTests : IDisposable
{
    private readonly DeleteCategoryApiTestsFixture _fixture;

    public DeleteCategoryApiTests(DeleteCategoryApiTestsFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async void DeleteCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        
        var exampleCategory = exampleCategoriesList[10];

        var (response, output) = await _fixture.ApiClient.Delete<object>($"/api/categories/{exampleCategory.Id}");
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
        output.Should().BeNull();
        
        var persistedCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        persistedCategory.Should().BeNull();
    }
    
    [Fact(DisplayName = nameof(ErrorWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
    public async void ErrorWhenNotFound()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);

        var randomGuid = Guid.NewGuid();

        var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>($"/api/categories/{randomGuid}");
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output!.Type.Should().Be("NotFound");
        output!.Status.Should().Be(StatusCodes.Status404NotFound);
        output!.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }
    
    public void Dispose() => _fixture.CleanPersistence();
}
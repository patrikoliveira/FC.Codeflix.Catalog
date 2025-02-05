using System.Net;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryApiTestsFixture))]
public class UpdateCategoryApiTests : IDisposable
{
    private readonly UpdateCategoryApiTestsFixture _fixture;

    public UpdateCategoryApiTests(UpdateCategoryApiTestsFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategory()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        
        var exampleCategory = exampleCategoriesList[10];
        var input = _fixture.GetExampleInput(exampleCategory.Id);

        var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>($"/api/categories/{exampleCategory.Id}", input);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be((bool)input.IsActive);
    }

    [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryOnlyName()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryInput(exampleCategory.Id, _fixture.GetValidCategoryName());

        var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>($"/api/categories/{exampleCategory.Id}", input);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive!);
        
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(exampleCategory.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }
    
    [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task UpdateCategoryNameAndDescription()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        
        var exampleCategory = exampleCategoriesList[10];
        var input = new UpdateCategoryInput(
            exampleCategory.Id, 
            _fixture.GetValidCategoryName(),
            _fixture.GetValidCategoryDescription());

        var (response, output) = await _fixture.ApiClient.Put<CategoryModelOutput>($"/api/categories/{exampleCategory.Id}", input);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Id.Should().Be(exampleCategory.Id);
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive!);
        
        var dbCategory = await _fixture.Persistence.GetById(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory!.Name.Should().Be(input.Name);
        dbCategory.Description.Should().Be(input.Description);
        dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
    }
    
    [Fact(DisplayName = nameof(ErroWhenNotFound))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    public async Task ErroWhenNotFound()
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        
        var randomGuid = Guid.NewGuid();
        var input = _fixture.GetExampleInput(randomGuid);

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"/api/categories/{randomGuid}", input);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output.Type.Should().Be("NotFound");
        output.Status.Should().Be(StatusCodes.Status404NotFound);
        output.Detail.Should().Be($"Category '{randomGuid}' not found.");
    }
    
    [Theory(DisplayName = nameof(ErroWhenCantInstantiateAggregate))]
    [Trait("EndToEnd/API", "Category/Update - Endpoints")]
    [MemberData(
        nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs),
        MemberType = typeof(UpdateCategoryApiTestDataGenerator))]
    public async Task ErroWhenCantInstantiateAggregate(UpdateCategoryInput input, string expectedDetails)
    {
        var exampleCategoriesList = _fixture.GetExampleCategoryList(20);
        await _fixture.Persistence.InsertList(exampleCategoriesList);
        
        var exampleCategory = exampleCategoriesList[10];
        input.Id = exampleCategory.Id;

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>(
            $"/api/categories/{exampleCategory.Id}", 
            input);
        
        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode) StatusCodes.Status422UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors occurred");
        output.Type.Should().Be("UnprocessableEntity");
        output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
        output.Detail.Should().Be(expectedDetails);
    }
    
    public void Dispose() => _fixture.CleanPersistence();
}
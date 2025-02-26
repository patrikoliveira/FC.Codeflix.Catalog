using FC.Codeflix.Catalog.Application.Exceptions;
using FluentAssertions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenre;

namespace FC.Codeflix.Catalog.UnitTests.Application.Genre.GetGenre;

[Collection(nameof(GetGenreTestFixture))]
public class GetGenreTest
{
    private readonly GetGenreTestFixture _fixture;

    public GetGenreTest(GetGenreTestFixture fixture)
        => _fixture = fixture;

    [Fact(DisplayName = nameof(GetGenre))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task GetGenre()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleCategories = _fixture.GetExampleCategoriesList();
        var exampleGenre = _fixture.GetExampleGenre(
            categoriesIds: exampleCategories.Select(x => x.Id).ToList()
        );
        genreRepositoryMock.Setup(x => x.Get(
            It.Is<Guid>(guid => guid == exampleGenre.Id),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(exampleGenre);
        categoryRepositoryMock.Setup(x => x.GetListByIds(
            It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleCategories);
        var useCase = new UseCase
            .GetGenre(genreRepositoryMock.Object, categoryRepositoryMock.Object);
        var input = new UseCase.GetGenreInput(exampleGenre.Id);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(exampleGenre.Id);
        output.Name.Should().Be(exampleGenre.Name);
        output.IsActive.Should().Be(exampleGenre.IsActive);
        output.CreatedAt.Should().BeSameDateAs(exampleGenre.CreatedAt);
        output.Categories.Should().HaveCount(exampleGenre.Categories.Count);

        foreach (var category in output.Categories)
        {
            var expectedCategory = exampleCategories.Single(x => x.Id == category.Id);
            category.Name.Should().Be(expectedCategory.Name);
        }

        genreRepositoryMock.Verify(
            x => x.Get(
                It.Is<Guid>(guid => guid == exampleGenre.Id),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Application", "GetGenre - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var genreRepositoryMock = _fixture.GetGenreRepositoryMock();
        var categoryRepositoryMock = _fixture.GetCategoryRepositoryMock();
        var exampleId = Guid.NewGuid();

        genreRepositoryMock.Setup(x 
            => x.Get(
                It.Is<Guid>(guid => guid == exampleId), 
                It.IsAny<CancellationToken>()
            ))
            .ThrowsAsync(new NotFoundException($"Genre '{exampleId}' not found")
        );

        var useCase = new UseCase.GetGenre(
            genreRepositoryMock.Object, 
            categoryRepositoryMock.Object);

        var input = new UseCase.GetGenreInput(exampleId);

        var action = async () => await useCase.Handle(input, CancellationToken.None);

        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Genre '{exampleId}' not found");

        genreRepositoryMock.Verify(
            x => x.Get(
                It.Is<Guid>(guid => guid == exampleId),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
    }
}
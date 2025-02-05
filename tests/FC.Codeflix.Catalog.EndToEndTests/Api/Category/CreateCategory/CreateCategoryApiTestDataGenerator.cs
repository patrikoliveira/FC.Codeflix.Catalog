namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory;

public class CreateCategoryApiTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateCategoryApiTestFixture();
        var invalidInputsList = new List<object[]>();
        const int totalInvalidCases = 3;

        for (var index = 0; index < totalInvalidCases; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    var input1 = fixture.GetExampleInput();
                    input1.Name = fixture.GetInvalidNameToShort();
                    invalidInputsList.Add(new object[] {
                        input1,
                        "Name should be at leats 3 characters long"
                    });
                    break;

                case 1:
                    var input2 = fixture.GetExampleInput();
                    input2.Name = fixture.GetInvalidNameTooLong();
                    invalidInputsList.Add(new object[] {
                        input2,
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                
                case 2:
                    var input6 = fixture.GetExampleInput();
                    input6.Description = fixture.GetInvalidDescriptionTooLong();
                    invalidInputsList.Add(new object[] {
                        input6,
                        "Description should be less or equal 10000 characters long"
                    });
                    break;
            }
        }


        return invalidInputsList;
    }
}
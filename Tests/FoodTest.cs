using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Eat
{
  public class FoodTest : IDisposable
  {
    public FoodTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=eat_test; Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = MakeFood.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNameAreTheSame()
    {
      //Arrange, Act
      MakeFood firstFood = new MakeFood("chicken");
      MakeFood secondFood = new MakeFood("chicken");

      //Assert
      Assert.Equal(firstFood, secondFood);
    }

    [Fact]
    public void Test_Save_SaveToDatabase()
    {
      //Arrange
      MakeFood testFood = new MakeFood("Beef");

      //Act
      testFood.Save();
      List<MakeFood> result = MakeFood.GetAll();
      List<MakeFood> testList = new List<MakeFood>{testFood};

      //Assert
      Assert.Equal(result, testList);
    }

    [Fact]
    public void Test_Save_AssignIdToObject()
    {
      //Arrange
      MakeFood testFood = new MakeFood("beef");

      //Act
      testFood.Save();
      MakeFood savedFood = MakeFood.GetAll()[0];

      int result = savedFood.GetId();
      int testId = testFood.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsFoodInDatabase()
    {
      //Arrange
      MakeFood testFood = new MakeFood("chicken");
      testFood.Save();

      //Act
      MakeFood foundFood = MakeFood.Find(testFood.GetId());

      //Assert
      Assert.Equal(testFood, foundFood);
    }

    public void Dispose()
    {
      MakeFood.DeleteAll();
    }
  }
}

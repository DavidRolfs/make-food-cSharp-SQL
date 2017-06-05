using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Eat
{
  public class MakeFood
  {
    private int _id;
    private string _name;

    public MakeFood(string name, int Id = 0)
    {
      _id = Id;
      _name = name;
    }

    public override bool Equals(System.Object otherFood)
    {
      if (!(otherFood is MakeFood))
      {
        return false;
      }
      else
      {
        MakeFood newFood = (MakeFood) otherFood;
        bool idEquality = (this.GetId() == newFood.GetId());
        bool foodtypeEquality = (this.GetName() == newFood.GetName());
        return (idEquality && foodtypeEquality);
      }
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public static List<MakeFood> GetAll()
    {
      List<MakeFood> allFood = new List<MakeFood>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM food;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int foodId = rdr.GetInt32(0);
        string foodName = rdr.GetString(1);
        MakeFood newFood = new MakeFood(foodName, foodId);
        allFood.Add(newFood);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allFood;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO food (type) OUTPUT INSERTED.id VALUES (@FoodType);", conn);

      SqlParameter typeParameter = new SqlParameter();
      typeParameter.ParameterName = "@FoodType";
      typeParameter.Value = this.GetName();
      cmd.Parameters.Add(typeParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static MakeFood Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM food WHERE id = @Foodid;", conn);
      SqlParameter foodIdParameter = new SqlParameter();
      foodIdParameter.ParameterName = "Foodid";
      foodIdParameter.Value = id.ToString();
      cmd.Parameters.Add(foodIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundFoodId = 0;
      string foundFoodType = null;
      while(rdr.Read())
      {
        foundFoodId = rdr.GetInt32(0);
        foundFoodType = rdr.GetString(1);
      }
      MakeFood foundFood = new MakeFood(foundFoodType, foundFoodId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundFood;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM food;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }
  }
}

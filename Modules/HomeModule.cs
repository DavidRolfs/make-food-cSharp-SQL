using System;
using Nancy;
using System.Collections.Generic;


namespace Eat
{
  public class HomeModule: NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ =>{
        List<MakeFood> foodList = MakeFood.GetAll();
        return View["index.cshtml", foodList];
      };
      Post["/newfood"] = _ =>{
        MakeFood newfood = new MakeFood(Request.Form["food"]);
        newfood.Save();
        return View["food_added.cshtml", newfood];
      };
    }
  }
}

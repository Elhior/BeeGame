using System;
using System.Web.Mvc;
using Bee_game.Models;
using Bee_game.Service;

namespace Bee_game.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Logger.InitLogger();
            return View();
        }

        public ActionResult Game()
        {
            return View(GameInstance.getInstance(GetBrowserId()).Stage);
        }
        
        //get unique id of browser to run new games from different browsers simultaneously
        public int GetBrowserId()
        {
            try
            {
                if (Request.Cookies["BeeGameSettings"]["ID"] == null){}
            }
            catch (Exception)
            {
                Response.Cookies["BeeGameSettings"]["ID"] = GameInstance.GetNewGameId().ToString();
            }
 
            return Int32.Parse(Request.Cookies["BeeGameSettings"]["ID"]);
        }
    }
}
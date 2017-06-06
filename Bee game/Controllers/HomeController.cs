using System;
using System.Web.Mvc;
using Bee_game.Models;

namespace Bee_game.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            Logger.Log.Debug("Home page requested.");
            return View("Index");
        }
        
        public ActionResult Game()
        {
            Logger.Log.Debug("Game page requested.");
            return View(GameInstance.getInstance(GetBrowserId()).Stage);
        }
        
        //get unique id of browser to run new games from different browsers simultaneously
        public int GetBrowserId()
        {
            try
            {
                if (Request.Cookies["BeeGameSettings"]["ID"] == null){}
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Browser haven't id (Probably first game)."+ ex.ToString());
                Response.Cookies["BeeGameSettings"]["ID"] = GameInstance.GetNewGameId().ToString();
            }
            Logger.Log.Debug("Browser id requested.");
            return Int32.Parse(Request.Cookies["BeeGameSettings"]["ID"]);
        }
    }
}
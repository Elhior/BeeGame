using System;
using System.Web.Mvc;
using Bee_game.Models;
using Bee_game.Service;
using Newtonsoft.Json;

namespace Bee_game.Controllers
{
    public class BeeGameRESTController : Controller
    {
       private IService beeGameService;

        public BeeGameRESTController(IService gameService)
        {
            beeGameService = gameService;
        }

        [HttpGet]
        public void NewGame()
        {
            beeGameService.NewGame(GetBrowserId());
        }

        [HttpGet]
        public void SaveGame(string savingtype)
        {
            beeGameService.SaveGame(GetBrowserId(), savingtype);
        }

        [HttpGet]
        public string LoadGame(string loadingtype)
        {
            return beeGameService.LoadGame(GetBrowserId(), loadingtype);
        }

        [HttpGet]
        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(beeGameService.GetConfiguration(GetBrowserId()));
        }

        [HttpPost]
        public void SaveConfiguration(GameConfiguration gameConfiguration)
        {
            beeGameService.SaveConfiguration(GetBrowserId(), gameConfiguration);
        }

        [HttpGet]
        public string HitBee(string id)
        {
            return beeGameService.HitBee(GetBrowserId(), id);
        }

        //get unique id of browser to run new games from different browsers simultaneously
        public int GetBrowserId()
        {
            try
            {
                if (Request.Cookies["BeeGameSettings"]["ID"] == null) { }
            }
            catch (Exception)
            {
                Response.Cookies["BeeGameSettings"]["ID"] = GameInstance.GetNewGameId().ToString();
            }
            Logger.Log.Debug("Browser id requested.");
            return Int32.Parse(Request.Cookies["BeeGameSettings"]["ID"]);
        }
    }
}
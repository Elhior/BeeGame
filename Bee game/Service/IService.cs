using System;
using System.Collections.Generic;
using Bee_game.Models;

namespace Bee_game.Service
{
    public interface IService
    {
        //initialise new game
        void NewGame(int gameID);
        //saving game
        void SaveGame(int gameID, string savingType);
        //loading game
        string LoadGame(int gameID ,string loadingType);
        //save new game configuration
        void SaveConfiguration(int gameID, GameConfiguration gameConfiguration);
        //return game configuration ????????return type config
        string GetConfiguration(int gameID);
        //hit target(chosen or random)
        string HitBee(int gameID, string target);
    }
}

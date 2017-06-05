using System;
using System.Collections.Generic;

namespace Bee_game.DAL
{
    public interface IRepository<T> : IDisposable
                where T : class
    {
        //saving game
        bool Save(T game);
        //loading game
        bool Load(T game);
    }
}

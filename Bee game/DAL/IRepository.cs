using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bee_game.Models;

namespace Bee_game.DAL
{
    interface IRepository<T> : IDisposable
                where T : class
    {
        //saving game
        bool Save(T game);
        //loading game
        bool Load(T game);
    }
}

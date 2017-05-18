using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bee_game.Models
{
    public interface IBee
    {
        string Name
        {
            get;
            set;
        }
        int Lifespan
        {
            get;
            set;
        }
        int Hitpoints
        {
            get;
            set;
        }
    }
}

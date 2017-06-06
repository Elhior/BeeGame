using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bee_game.Models
{
    [Table("Bees")]
    public abstract class IBee
    {
        [Key]
        [Column("id")]
        public long ID
        {
            get;
            set;
        }

        [Column("name")]
        public string Name
        {
            get;
            set;
        }
        [Column("life_span")]
        public int Lifespan
        {
            get;
            set;
        }
        [Column("hit_points")]
        public int Hitpoints
        {
            get;
            set;
        }

        public IBee() { }

        public IBee(string name, int lifespan, int hitpoints)
        {
            Name = name;
            Lifespan = lifespan;
            Hitpoints = hitpoints;
        }

        public override string ToString()
        {
            return Name + ":" + Lifespan + ":" + Hitpoints;
        }
    }
}

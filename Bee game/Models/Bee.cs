using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bee_game.Models
{
    [Table("Bees")]
    public class Bee : IBee
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
        [Column("lifespan")]
        public int Lifespan
        {
            get;
            set;
        }
        [Column("hitpoints")]
        public int Hitpoints
        {
            get;
            set;
        }
        //needed for load Bees from repository
        public Bee() { }

        public Bee(string name,int lifespan, int hitpoints)
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
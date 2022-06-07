using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APRICODETest.Model
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DeveloperID { get; set; }
        public Developer Developer { get; set; }
        public ICollection<Genre> Genres { get; set; }
     }
}

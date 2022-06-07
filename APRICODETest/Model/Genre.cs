using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APRICODETest.Model
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}

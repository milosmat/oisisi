using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.DTO
{
    public class PolozeniPredmetiDTO
    {
        public string SifraPredmeta { get; set; }
        public string NazivPredmeta { get; set; }
        public int ESPB { get; set; }
        public int Ocena { get; set; }
        public DateTime DatumPolaganja { get; set; }
    }
}

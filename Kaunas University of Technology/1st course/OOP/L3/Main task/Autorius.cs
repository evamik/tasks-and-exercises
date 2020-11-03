using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace programa
{
    class Autorius
    {
        public string VardasPavarde { get; set; }
        public int Skaicius { get; set; }

        public Autorius(string vardasPavarde, int skaicius)
        {
            VardasPavarde = vardasPavarde;
            Skaicius = skaicius;
        }
    }
}

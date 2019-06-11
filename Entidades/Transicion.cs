using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata3.Entidades
{
    public class Transicion
    {
        public string[] EstIncio;
        public List<Caso> casos;
        public bool flagStart;
        public bool flagTerminal;
        public Transicion()
        {
            flagStart = false;
            flagTerminal = false;
            List<Caso> casos = new List<Caso>();
        }
    }
}

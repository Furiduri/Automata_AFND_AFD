using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata3
{
    
    class Estado
    {
        private string name;
        private int key;


        public Estado(string name, int key)
        {
            Name = name;
            this.Key = key;
        }

        public int Key { get => key; set => key = value; }
        public string Name { get => name; set => name = value; }
    }
}

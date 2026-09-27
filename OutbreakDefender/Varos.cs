using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbreakDefender
{
    public class Varos
    {
        public string Nev {  get; set; }
        public int Lakossag { get; set; }
        public int Fertozottseg { get; set; } // 0-100%
        public int Vedelem { get; set; } // 0-3
        public bool Karanten {  get; set; }

        public Varos(string nev, int lakossag, int fertozottseg)
        {
            Nev = nev;
            Lakossag = lakossag;
            Fertozottseg = fertozottseg;
        }

        public override string ToString()
        {
            return $"{Nev} - Fertőzöttség: {Fertozottseg}%";
        }
    }
}

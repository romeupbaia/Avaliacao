using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dados_Covid_19.DTO
{
    public class Dados
    {   
        public string Province { get; set; }
        public string City { get; set; }
        public int Recovered { get; set; }
        public int Active { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public DateTime Date { get; set; }

    }
}


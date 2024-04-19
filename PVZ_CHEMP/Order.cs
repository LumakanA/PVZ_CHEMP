using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ_CHEMP
{
    public class Order
    {
        public int ID_order { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public int Cell { get; set; }
    }
}

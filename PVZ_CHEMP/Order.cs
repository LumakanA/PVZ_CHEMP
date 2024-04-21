using System;

namespace PVZ_CHEMP
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime ArrivedDate { get; set; }
        public string Status { get; set; }

        public int CellNumber { get; set; }

        public int ClientID { get; set; }
    }
}

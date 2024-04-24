using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVZ_CHEMP
{
    public class InventoryItem
    {
        public int OrderID { get; set; }
        public DateTime ArrivedDate { get; set; }
        public string Status { get; set; }
        public int CellNumber { get; set; }
        public int StoragePeriod { get; set; } // Период хранения в днях

        public DateTime ExpirationDate { get { return ArrivedDate.AddDays(StoragePeriod); } } // Дата истечения срока годности
        public int RemainingDays { get { return (ExpirationDate - DateTime.Today).Days; } } // Оставшиеся дни до истечения срока годности

        public InventoryItem()
        {
            // Установка значения по умолчанию для периода хранения
            StoragePeriod = 5; // По умолчанию 5 дней
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class TrafficFineType
    {
        [Key]
        public int TrafficFineTypeId { get; set; }
        public string Article { get; set; } = null!;
        public string Description { get; set; } = null!;

        /// <summary>
        /// Güncel ceza tutarı. Tutarlar her yıl değiştiği için cezanın kesildiği
        /// andaki tutar ayrıca TrafikCezasi.Tutar alanında saklanır.
        /// </summary>
        public decimal Amount { get; set; }
        public int TrafficFinePoint { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<TrafficFine> Fines { get; set; } = new List<TrafficFine>();
    }
}

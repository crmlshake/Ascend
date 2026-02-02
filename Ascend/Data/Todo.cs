using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascend.Data
{
    public partial class Todo
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsFavorite { get; set; }

        public virtual User User { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ascend.Data
{
    public partial class UserProgress
    {
        [Key]
        public int ProgressID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public int CurrentXp { get; set; }
        public int CurrentLevel { get; set; }
        public int XpToNextLevel { get; set; }

        //Die Navigation Property, damit EF die Verbindung kennt!
        public virtual User User { get; set; }
    }
}
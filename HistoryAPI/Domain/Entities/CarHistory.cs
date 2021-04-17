using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HistoryAPI.Domain.Entities
{
    public class CarHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Company { get; set; }

        public string Model { get; set; }

        public string UserLogin { get; set; }

        public string Action { get; set; }
    }
}

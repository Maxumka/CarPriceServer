using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMarkTypeAPI.Database.Entity
{
    public class CarType 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id  { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public int CarMarkId { get; set; }

        public CarMark CarMark { get; set; }
    }
}
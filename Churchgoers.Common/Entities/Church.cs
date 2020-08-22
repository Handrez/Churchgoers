using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Churchgoers.Common.Entities
{
    public class Church
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        [NotMapped]
        public int IdDistrict { get; set; }
    }
}

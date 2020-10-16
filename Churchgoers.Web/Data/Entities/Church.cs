using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Churchgoers.Web.Data.Entities
{
    public class Church
    {
        public int Id { get; set; }

        [Display(Name = "Church")]
        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        [NotMapped]
        public int IdDistrict { get; set; }

        [JsonIgnore]
        public District District { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }

        public ICollection<Meeting> Meetings { get; set; }

        [Display(Name = "# Users")]
        public int UsersNumber => Users == null ? 0 : Users.Count;
    }
}
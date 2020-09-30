using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Churchgoers.Web.Data.Entities
{
    public class District
    {
        public int Id { get; set; }

        [Display(Name = "District")]
        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<Church> Churches { get; set; }

        [Display(Name = "# Churches")]
        public int ChurchesNumber => Churches == null ? 0 : Churches.Count;

        [Display(Name = "# Users")]
        public int UsersNumber => Churches == null ? 0 : Churches.Sum(c => c.UsersNumber);

        [JsonIgnore]
        [NotMapped]
        public int IdField { get; set; }

        [JsonIgnore]
        public Field Field { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Churchgoers.Web.Data.Entities
{
    public class Field
    {
        public int Id { get; set; }

        [Display(Name = "Field")]
        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }

        [Display(Name = "# Districts")]
        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;

        [Display(Name = "# Churches")]
        public int ChurchesNumber => Districts == null ? 0 : Districts.Sum(d => d.ChurchesNumber);

        [Display(Name = "# Users")]
        public int UsersNumber => Districts == null ? 0 : Districts.Sum(d => d.UsersNumber);
    }
}
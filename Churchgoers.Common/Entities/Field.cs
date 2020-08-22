using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Churchgoers.Common.Entities
{
    public class Field
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }

        [DisplayName("Districts Number")]
        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;
    }
}

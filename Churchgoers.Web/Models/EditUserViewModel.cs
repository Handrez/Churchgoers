using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Churchgoers.Web.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://churchgoers.azurewebsites.net/images/noimage.png"
            : $"https://handres.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "Field")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a field.")]
        public int FieldId { get; set; }

        public IEnumerable<SelectListItem> Fields { get; set; }

        [Required]
        [Display(Name = "District")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a district.")]
        public int DistrictId { get; set; }

        public IEnumerable<SelectListItem> Districts { get; set; }

        [Required]
        [Display(Name = "Church")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a church.")]
        public int ChurchId { get; set; }

        public IEnumerable<SelectListItem> Churches { get; set; }

        [Required]
        [Display(Name = "Profession")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a profession.")]
        public int ProfessionId { get; set; }

        public IEnumerable<SelectListItem> Professions { get; set; }
    }

}

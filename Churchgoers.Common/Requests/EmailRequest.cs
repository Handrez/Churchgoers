using System.ComponentModel.DataAnnotations;

namespace Churchgoers.Common.Requests
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}

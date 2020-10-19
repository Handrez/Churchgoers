using Churchgoers.Common.Responses;
using System.ComponentModel.DataAnnotations;

namespace Churchgoers.Common.Requests
{
    public class AssistanceRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public UserResponse User { get; set; }

        [Required]
        public MeetingResponse Meeting { get; set; }

        [Required]
        public bool IsPresent { get; set; }
    }
}

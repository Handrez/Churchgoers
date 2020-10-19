using Churchgoers.Common.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Churchgoers.Common.Requests
{
    public class MeetingRequest
    {
        [Required]
        public int ChurchId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public ICollection<AssistanceResponse> Assistances { get; set; }
    }
}

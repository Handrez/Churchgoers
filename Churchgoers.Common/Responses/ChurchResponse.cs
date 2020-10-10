using System;
using System.Collections.Generic;
using System.Text;

namespace Churchgoers.Common.Responses
{
    public class ChurchResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserResponse> Users { get; set; }

        public ICollection<MeetingResponse> Meetings { get; set; }

        public int UsersNumber => Users == null ? 0 : Users.Count;
    }
}

using System.Collections.Generic;

namespace Churchgoers.Common.Responses
{
    public class ProfessionResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserResponse> Users { get; set; }

        public int UsersNumber => Users == null ? 0 : Users.Count;
    }
}

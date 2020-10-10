using System.Collections.Generic;
using System.Linq;

namespace Churchgoers.Common.Responses
{
    public class DistrictResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ChurchResponse> Churches { get; set; }

        public int ChurchesNumber => Churches == null ? 0 : Churches.Count;

        public int UsersNumber => Churches == null ? 0 : Churches.Sum(c => c.UsersNumber);
    }
}

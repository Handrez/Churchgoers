using System.Collections.Generic;
using System.Linq;

namespace Churchgoers.Common.Responses
{
    public class FieldResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<DistrictResponse> Districts { get; set; }

        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;

        public int ChurchesNumber => Districts == null ? 0 : Districts.Sum(d => d.ChurchesNumber);

        public int UsersNumber => Districts == null ? 0 : Districts.Sum(d => d.UsersNumber);
    }
}

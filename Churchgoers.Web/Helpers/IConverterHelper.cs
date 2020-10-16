using Churchgoers.Common.Responses;
using Churchgoers.Web.Data.Entities;
using System.Collections.Generic;

namespace Churchgoers.Web.Helpers
{
    public interface IConverterHelper
    {
        ICollection<FieldResponse> ToFieldResponse(ICollection<Field> fields);

        ICollection<DistrictResponse> ToDistrictResponse(ICollection<District> districts);

        ChurchResponse ToChurchResponse(Church churches);

        ProfessionResponse ToProfessionResponse(Profession professions);

        UserResponse ToUserResponse(User user);

        ICollection<MeetingResponse> ToMeetingResponse(ICollection<Meeting> meetings);
    }
}

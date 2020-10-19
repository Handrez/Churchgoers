using Churchgoers.Common.Responses;
using Churchgoers.Web.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Churchgoers.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public ICollection<FieldResponse> ToFieldResponse(ICollection<Field> fields)
        {
            return fields.Select(f => new FieldResponse
            {
                Districts = f.Districts.Select(d => new DistrictResponse
                {
                    Churches = d.Churches.Select(c => new ChurchResponse
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList(),
                    Id = d.Id,
                    Name = d.Name
                }).ToList(),
                Id = f.Id,
                Name = f.Name
            }).ToList();
        }

        public ICollection<DistrictResponse> ToDistrictResponse(ICollection<District> districts)
        {
            return districts.Select(d => new DistrictResponse
            {
                Churches = d.Churches.Select(c => new ChurchResponse
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                Id = d.Id,
                Name = d.Name
            }).ToList();
        }

        public ChurchResponse ToChurchResponse(Church churches)
        {
            return new ChurchResponse
            {
                Id = churches.Id,
                Name = churches.Name
            };
        }

        public ProfessionResponse ToProfessionResponse(Profession professions)
        {
            return new ProfessionResponse
            {
                Id = professions.Id,
                Name = professions.Name
            };
        }

        public UserResponse ToUserResponse(User user)
        {
            return new UserResponse
            {
                Address = user.Address,
                Church = ToChurchResponse(user.Church),
                Document = user.Document,
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Profession = ToProfessionResponse(user.Profession),
                UserType = user.UserType
            };
        }

        public ICollection<MeetingResponse> ToMeetingResponse(ICollection<Meeting> meetings)
        {
            return meetings.Select(m => new MeetingResponse
            {
                Assistances = m.Assistances.Select(a => new AssistanceResponse
                {
                    Id = a.Id,
                    IsPresent = a.IsPresent,
                    User = ToUserResponse(a.User)
                }).ToList(),
                Church = ToChurchResponse(m.Church),
                Date = m.Date,
                Id = m.Id
            }).ToList();
        }

        public MeetingResponse ToMeetingResponse(Meeting meeting)
        {
            return (new MeetingResponse
            {
                Assistances = meeting.Assistances.Select(a => new AssistanceResponse
                {
                    Id = a.Id,
                    IsPresent = a.IsPresent,
                    User = ToUserResponse(a.User)

                }).ToList(),
                Church = ToChurchResponse(meeting.Church),
                Date = meeting.Date,
                Id = meeting.Id
            });
        }
    }
}
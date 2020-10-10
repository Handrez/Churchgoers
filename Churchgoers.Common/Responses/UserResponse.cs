using Churchgoers.Common.Enums;
using System;
using System.Collections.Generic;

namespace Churchgoers.Common.Responses
{
    public class UserResponse
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";

        public string Address { get; set; }

        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://churchgoers.azurewebsites.net/images/noimage.png"
            : $"https://handres.blob.core.windows.net/users/{ImageId}";

        public UserType UserType { get; set; }

        public ChurchResponse Church { get; set; }

        public ProfessionResponse Profession { get; set; }

        public ICollection<AssistanceResponse> Assistances { get; set; }
    }
}

﻿using Churchgoers.Common.Responses;

namespace Churchgoers.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}

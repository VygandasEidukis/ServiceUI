using System;

namespace EPS.Administration.APIAccess.Models.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException() { }
        public ServiceException(string message) : base(message) { }
    }
}

using System;

namespace Common.Utils
{
    public class UrlHelper
    {
        public string GetHostName(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException("address");
            }

            var uri = new Uri(address);
            var hostName = string.Format("{0}://{1}:{2}/", uri.Scheme, uri.Host, uri.Port);

            return hostName;
        }
    }
}

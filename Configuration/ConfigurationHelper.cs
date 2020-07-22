using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funda_assignment_Silvestrova.Configuration
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        private string securityKey;
        public ConfigurationHelper(string securityKey)
        {
            this.securityKey = securityKey;
        }
        public string CombineListingsRequestUrl(string [] sortParams, int pageNumber, int pageSize)
        {
            return $"/feeds/Aanbod.svc/json/{securityKey}/?type=koop&zo={CreateQueryString(sortParams)}&page={pageNumber}&pagesize={pageSize}";
        }
        private string CreateQueryString(string[] sortParams)
        {
            var newElements = sortParams.Select(x => x.Replace("/", string.Empty)).ToArray();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("/");
            stringBuilder.AppendJoin('/', sortParams);
            stringBuilder.Append("/");
            return stringBuilder.ToString();
        }
    }
}

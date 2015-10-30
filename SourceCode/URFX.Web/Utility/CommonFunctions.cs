using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Resources;

namespace URFX.Web.Utility
{
    public static class CommonFunctions
    {
        public static string ReadResourceValue(string Value)
        {
            string resourceValue = string.Empty;
            try
            {
                resourceValue = Resources.ResourceManager.GetString(Value);
                return resourceValue;
                
            }
            catch (Exception ex)
            {               
                resourceValue = Value;
            }
            return resourceValue;
        }
    }
}

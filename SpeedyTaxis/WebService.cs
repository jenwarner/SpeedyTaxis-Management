using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyTaxis
{
    class WebService
    {
        private static string localHostUrl;

        public static string LocalHostUrl
        {
            get { return localHostUrl; }
            set { localHostUrl = "http://localhost:10293/"; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Web.Utility
{
    public class SystemCodeInfo
    {
        public string SystemCode
        {
            get;
            set;
        }

        public string SystemURL
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int OrderIndex
        {
            get;
            set;
        }

        public bool Display
        {
            get;
            set;
        }
    }
    public class SSOClaimsPrincipal : IPrincipal
    {
        private SSOClaimsIdentity _identity;

        public SSOClaimsIdentity Identity
        {
            get
            {
                return this._identity;
            }
        }

        IIdentity IPrincipal.Identity
        {
            get
            {
                return this._identity;
            }
        }

        public string SystemCode
        {
            get;
            set;
        }

        public string ChallengeNumer
        {
            get;
            set;
        }

        public DateTime ClientDate
        {
            get;
            set;
        }

        public Dictionary<string, SystemCodeInfo> SystemCodes
        {
            get;
            set;
        }

        public SSOClaimsPrincipal(SSOClaimsIdentity identity)
        {
            this._identity = identity;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.MVC.Tenancy.Security
{
  internal  class TenantSecurityConstants
    {
        internal const string CookieName = "_borg";
        internal const string AuthorizationHeaderName = "Borg:Authorization";
        internal const string TenantSchemeName = "Borg";
    }
}

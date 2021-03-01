using System;
using Wbcl.Core.Models.Database;

namespace Wbcl.Core.Utils
{
    public interface IVkUtils
    {
        (bool Success, long Id, string Name, PreferenceType LinkType) GetObjIdIdByLink(Uri link);
    }
}

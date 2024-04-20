using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EnumCode;

public static class EnumCode
{
    public static string GetCode(this Enum enumVal)
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumCodeAttribute), false);
        return ((EnumCodeAttribute)attributes[0]).Name;
    }
}

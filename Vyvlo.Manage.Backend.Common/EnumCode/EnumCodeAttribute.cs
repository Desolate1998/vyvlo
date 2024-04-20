using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EnumCode;

public class EnumCodeAttribute : Attribute
{
    public string Name { get; set; }
    public EnumCodeAttribute(string Name)
    {
        this.Name = Name;
    }
}

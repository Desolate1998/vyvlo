using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DateTimeProvider;
public static class DateTimeProvider
{
    public static DateTime ApplicationDate => DateTime.UtcNow;
}

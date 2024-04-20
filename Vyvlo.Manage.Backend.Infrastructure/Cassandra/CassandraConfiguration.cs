using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vyvlo.Manage.Backend.Infrastructure.Cassandra;

public class CassandraConfiguration
{
    public static string SectionName = "CassandraConfiguration";

    public string Node { get; set; } 
    public string Username { get; set; }
    public string Password { get; set; }
    public string Keyspace { get; set; }
}

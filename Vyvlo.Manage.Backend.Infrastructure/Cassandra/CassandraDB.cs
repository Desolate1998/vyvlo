using Cassandra;
using Common.JwtTokenGenerator;
using Microsoft.Extensions.Options;

namespace Vyvlo.Manage.Backend.Infrastructure.Cassandra;

public class CassandraDB
{
    private Cluster _cluster;
    private ISession? _session;
    private readonly CassandraConfiguration _configuration;

    public CassandraDB(IOptions<CassandraConfiguration>  configuration)
    {
        _configuration = configuration.Value;
        Connect();
    }

    private void Connect()
    {
        _cluster = Cluster.Builder()
                          .WithCredentials(_configuration.Username, _configuration.Password)
                          .AddContactPoint(_configuration.Node).Build();
        
        _session = _cluster.Connect();
        _session.ChangeKeyspace(_configuration.Keyspace);
    }

    public ISession GetSession()
    {
        if (_session == null)
        {
            Connect();
        }
        return _session is null ? throw new NullReferenceException("Cassandra Session is null") : _session;
    }
}

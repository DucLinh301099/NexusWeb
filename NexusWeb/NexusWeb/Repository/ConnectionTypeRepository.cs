using NexusWeb.Models;
using System;
namespace NexusWeb.Repository
{
    public class ConnectionTypeRepository : IConnectionTypeRepository
    {
        private readonly NexusWebAppContext _appContext;
        public ConnectionTypeRepository(NexusWebAppContext appContext)
        {
            _appContext = appContext;
        }


        public ConnectionType Add(ConnectionType connectionType)
        {
            _appContext.ConnectionTypes.Add(connectionType);
            _appContext.SaveChanges();
            return connectionType;
        }
        public ConnectionType Update(ConnectionType connectionType)
        {
            _appContext.ConnectionTypes.Update(connectionType);
            _appContext.SaveChanges(true);
            return connectionType;
        }
        public ConnectionType Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ConnectionType Get(int id)
        {
            return _appContext.ConnectionTypes.Find(id);
        }
        public IEnumerable<ConnectionType> GetAll()
        {
            return _appContext.ConnectionTypes;
        }
    }
}

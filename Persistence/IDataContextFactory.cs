using System;
using CrudDatastore;

namespace Persistence
{
    public interface IDataContextFactory
    {
        DataContextBase CreateDataContext();
    }
}


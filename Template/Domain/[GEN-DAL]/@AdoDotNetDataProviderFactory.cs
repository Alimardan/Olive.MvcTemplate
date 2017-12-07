﻿namespace AppData
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Olive;
    using Olive.Entities;
    using Olive.Entities.Data;
    /// <summary>A factory that can instantiate Data Provider objects for MY.PROJECT.NAME</summary>
    public class AdoDotNetDataProviderFactory : IDataProviderFactory
    {
        string ConnectionStringKey;
        public string ConnectionString {get; private set;}
        
        /// <summary>Initializes a new instance of AdoDotNetDataProviderFactory.</summary>
        public AdoDotNetDataProviderFactory(DataProviderFactoryInfo factoryInfo)
        {
            ConnectionString = factoryInfo.ConnectionString.Or(DataAccess.GetCurrentConnectionString());
            ConnectionStringKey = factoryInfo.ConnectionStringKey;
        }
        
        public IDataAccess GetAccess() => new DataAccess<SqlConnection>();
        
        /// <summary>Gets a data provider instance for the specified entity type.</summary>
        public virtual IDataProvider GetProvider(Type type)
        {
            IDataProvider result = null;
            
            if (type == typeof(Domain.Administrator)) result = new AdministratorDataProvider();
            else if (type == typeof(Domain.ContentBlock)) result = new ContentBlockDataProvider();
            else if (type == typeof(Domain.EmailTemplate)) result = new EmailTemplateDataProvider();
            else if (type == typeof(Domain.Settings)) result = new SettingsDataProvider();
            else if (type == typeof(Domain.LogonFailure)) result = new LogonFailureDataProvider();
            else if (type == typeof(Domain.ApplicationEvent)) result = new ApplicationEventDataProvider();
            else if (type == typeof(Domain.User)) result = new UserDataProvider();
            else if (type == typeof(Domain.EmailQueueItem)) result = new EmailQueueItemDataProvider();
            else if (type == typeof(Domain.PasswordResetTicket)) result = new PasswordResetTicketDataProvider();
            else if (type.IsInterface) result = new InterfaceDataProvider(type);
            
            if (result == null)
            {
                throw new NotSupportedException(type + " is not a data-supported type.");
            }
            else if (this.ConnectionString.HasValue())
            {
                result.ConnectionString = this.ConnectionString;
            }
            else if (this.ConnectionStringKey.HasValue())
            {
                result.ConnectionStringKey = this.ConnectionStringKey;
            }
            
            return result;
        }
        
        /// <summary>Determines whether this data provider factory handles interface data queries.</summary>
        public virtual bool SupportsPolymorphism() => true;
    }
}
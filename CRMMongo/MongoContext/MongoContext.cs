using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CRMMongo.Properties;
using MongoDB.Driver;

namespace CRMMongo.MongoContext {
	public interface IMongoContext {
		IMongoClient Client { get; set; }
		IMongoDatabase Database { get; set; }
		string CollectionName { get; set; }
	}

	public class MongoContext : IMongoContext {
		public IMongoClient Client { get; set; }
		public IMongoDatabase Database { get; set; }
		public string CollectionName { get; set; }

		public MongoContext() {
			Client = new MongoClient(Settings.Default.mongoDbConnectionString);
			Database = Client.GetDatabase(Settings.Default.mongoDbName);
		}
	}
}
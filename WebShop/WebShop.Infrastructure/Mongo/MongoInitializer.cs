using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Infrastructure.Mongo
{
    public class MongoInitializer : IDatabaseInitializer
    {
        private bool _initializer;
        private IMongoDatabase _database;

        public MongoInitializer(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task InitializeAsync()
        {
            if (_initializer)
                return;

            IConventionPack conventionPack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true),
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
            };
            ConventionRegistry.Register("WebShop", conventionPack, x => true);

            _initializer = true;
            await Task.CompletedTask;
        }
    }
}

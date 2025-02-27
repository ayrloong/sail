using Sail.Core.Entities;
using Sail.Core.Options;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Sail.Storage.MongoDB;

public class SailContext
{
    private readonly IMongoDatabase _database;

    public SailContext(IOptions<DatabaseOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    static SailContext()
    {
        RegisterClassMaps();
    }

    public IMongoCollection<Cluster> Clusters => _database.GetCollection<Cluster>("clusters");
    public IMongoCollection<Route> Routes => _database.GetCollection<Route>("routes");
    public IMongoCollection<Certificate> Certificates => _database.GetCollection<Certificate>("certificates");

    public async Task InitializeAsync()
    {
        var collectionOptions = new CreateCollectionOptions
        {
            ChangeStreamPreAndPostImagesOptions = new ChangeStreamPreAndPostImagesOptions
            {
                Enabled = true
            }
        };
        await _database.CreateCollectionAsync("routes", collectionOptions);
        await _database.CreateCollectionAsync("clusters", collectionOptions);
        await _database.CreateCollectionAsync("certificates", collectionOptions);
    }
    private static void RegisterClassMaps()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Route)))
        {
            BsonClassMap.RegisterClassMap<Route>(classMap =>
            {
                classMap.AutoMap();
                classMap.MapIdMember(x => x.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Cluster)))
        {
            BsonClassMap.RegisterClassMap<Cluster>(classMap =>
            {
                classMap.AutoMap();
                classMap.MapIdMember(x => x.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }
        
        if (!BsonClassMap.IsClassMapRegistered(typeof(Certificate)))
        {
            BsonClassMap.RegisterClassMap<Certificate  >(classMap =>
            {
                classMap.AutoMap();
                classMap.MapIdMember(x => x.Id)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            });
        }
    }
}
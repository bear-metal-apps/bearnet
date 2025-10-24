using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bearnet.Models.Cache;

public class CachedResponse {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("api")]
    public string Api { get; set; } = string.Empty;

    [BsonElement("endpoint")]
    public string Endpoint { get; set; } = string.Empty;

    [BsonElement("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("contentType")]
    public string? ContentType { get; set; }

    [BsonElement("etag")]
    public string? ETag { get; set; }

    [BsonElement("lastModified")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime LastModified { get; set; }

    [BsonElement("expiresAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ExpiresAt { get; set; }
}

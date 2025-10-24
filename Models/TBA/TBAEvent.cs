using System.Text.Json.Serialization;

namespace Bearnet.Models.TBA;

public class TBAEvent {
    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("event_code")] public string EventCode { get; set; } = string.Empty;

    [JsonPropertyName("event_type")] public int EventType { get; set; }

    [JsonPropertyName("district")] public District? District { get; set; }

    [JsonPropertyName("city")] public string? City { get; set; }

    [JsonPropertyName("state_prov")] public string? StateProv { get; set; }

    [JsonPropertyName("country")] public string? Country { get; set; }

    [JsonPropertyName("start_date")] public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")] public DateTime EndDate { get; set; }

    [JsonPropertyName("year")] public int Year { get; set; }

    [JsonPropertyName("short_name")] public string? ShortName { get; set; }

    [JsonPropertyName("event_type_string")]
    public string EventTypeString { get; set; } = string.Empty;

    [JsonPropertyName("week")] public int? Week { get; set; }

    [JsonPropertyName("address")] public string? Address { get; set; }

    [JsonPropertyName("postal_code")] public string? PostalCode { get; set; }

    [JsonPropertyName("gmaps_place_id")] public string? GmapsPlaceId { get; set; }

    [JsonPropertyName("gmaps_url")] public string? GmapsUrl { get; set; }

    [JsonPropertyName("lat")] public double? Lat { get; set; }

    [JsonPropertyName("lng")] public double? Lng { get; set; }

    [JsonPropertyName("location_name")] public string? LocationName { get; set; }

    [JsonPropertyName("timezone")] public string? Timezone { get; set; }

    [JsonPropertyName("website")] public string? Website { get; set; }

    [JsonPropertyName("first_event_id")] public string? FirstEventId { get; set; }

    [JsonPropertyName("first_event_code")] public string? FirstEventCode { get; set; }

    [JsonPropertyName("webcasts")] public List<Webcast>? Webcasts { get; set; }

    [JsonPropertyName("division_keys")] public List<string>? DivisionKeys { get; set; }

    [JsonPropertyName("parent_event_key")] public string? ParentEventKey { get; set; }

    [JsonPropertyName("playoff_type")] public int? PlayoffType { get; set; }

    [JsonPropertyName("playoff_type_string")]
    public string? PlayoffTypeString { get; set; }
}

public class District {
    [JsonPropertyName("abbreviation")] public string Abbreviation { get; set; } = string.Empty;

    [JsonPropertyName("display_name")] public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;

    [JsonPropertyName("year")] public int Year { get; set; }
}

public class Webcast {
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    [JsonPropertyName("channel")] public string? Channel { get; set; }

    [JsonPropertyName("date")] public string? Date { get; set; }

    [JsonPropertyName("file")] public string? File { get; set; }
}

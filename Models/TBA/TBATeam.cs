namespace Bearnet.Models.TBA;

using System.Text.Json.Serialization;

public class TBATeam {
    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;

    [JsonPropertyName("team_number")] public int TeamNumber { get; set; }

    [JsonPropertyName("nickname")] public string Nickname { get; set; } = string.Empty;

    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("school_name")] public string? SchoolName { get; set; }

    [JsonPropertyName("city")] public string? City { get; set; }

    [JsonPropertyName("state_prov")] public string? StateProv { get; set; }

    [JsonPropertyName("country")] public string? Country { get; set; }

    [JsonPropertyName("address")] public string? Address { get; set; }

    [JsonPropertyName("postal_code")] public string? PostalCode { get; set; }

    [JsonPropertyName("gmaps_place_id")] public string? GmapsPlaceId { get; set; }

    [JsonPropertyName("gmaps_url")] public string? GmapsUrl { get; set; }

    [JsonPropertyName("lat")] public double? Lat { get; set; }

    [JsonPropertyName("lng")] public double? Lng { get; set; }

    [JsonPropertyName("location_name")] public string? LocationName { get; set; }

    [JsonPropertyName("website")] public string? Website { get; set; }

    [JsonPropertyName("rookie_year")] public int? RookieYear { get; set; }
}

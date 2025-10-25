using System;
using System.Text.Json.Serialization;
using HotChocolate;


namespace Bearnet.Models;

public class User {
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("firstName")] public string FirstName { get; set; }
    [JsonPropertyName("lastName")] public string LastName { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")] public DateTime? UpdatedAt { get; set; }
    [JsonPropertyName("lastLoginAt")] public DateTime? LastLoginAt { get; set; }
    [JsonPropertyName("preferences")] public UserPreferences Preferences { get; set; }
    [JsonPropertyName("roles")] public Role[] Roles { get; set; }
}

public class UserPreferences {
    [JsonPropertyName("theme")] public Theme Theme { get; set; }
    [JsonPropertyName("themeColor"), GraphQLDescription("Hex code for the theme color")] public string ThemeColor { get; set; }
}

public enum Theme {
    Light,
    Dark,
    System
}


public enum Role {
    [GraphQLDescription("Edit roles for other users")] ManageRoles,
    [GraphQLDescription("Create, edit, and view any picklist")] ManagePicklists,
    [GraphQLDescription("View all notes on teams")] ViewNotes,
    [GraphQLDescription("Access drive team features (prematch strategy and note taking)")] DriveTeam,
    [GraphQLDescription("Access strategist features (prematch strategy and sending notes to drive team)")] Strategist,
    [GraphQLDescription("Being able to scout matches and pits")] Scout
}

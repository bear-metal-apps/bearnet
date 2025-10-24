namespace bearnet.Models.TBA;

using System.Text.Json;
using System.Text.Json.Serialization;

public class TBAMatch {
    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;

    [JsonPropertyName("comp_level")] public string CompLevel { get; set; } = string.Empty;

    [JsonPropertyName("set_number")] public int SetNumber { get; set; }

    [JsonPropertyName("match_number")] public int MatchNumber { get; set; }

    [JsonPropertyName("alliances")] public TBAMatchAlliances? Alliances { get; set; }

    [JsonPropertyName("winning_alliance")] public string? WinningAlliance { get; set; }

    [JsonPropertyName("event_key")] public string EventKey { get; set; } = string.Empty;

    [JsonPropertyName("time")] public long? Time { get; set; }

    [JsonPropertyName("actual_time")] public long? ActualTime { get; set; }

    [JsonPropertyName("predicted_time")] public long? PredictedTime { get; set; }

    [JsonPropertyName("post_result_time")] public long? PostResultTime { get; set; }

    [JsonPropertyName("score_breakdown")]
    [JsonConverter(typeof(ScoreBreakdownConverter))]
    public IScoreBreakdown? ScoreBreakdown { get; set; }

    [JsonPropertyName("videos")] public List<MatchVideo>? Videos { get; set; }
}

public class TBAMatchAlliances {
    [JsonPropertyName("red")] public TBAAlliance Red { get; set; } = new();

    [JsonPropertyName("blue")] public TBAAlliance Blue { get; set; } = new();
}

public class TBAAlliance {
    [JsonPropertyName("score")] public int Score { get; set; }

    [JsonPropertyName("team_keys")] public List<string> TeamKeys { get; set; } = new();

    [JsonPropertyName("surrogate_team_keys")]
    public List<string>? SurrogateTeamKeys { get; set; }

    [JsonPropertyName("dq_team_keys")] public List<string>? DqTeamKeys { get; set; }
}

public class MatchVideo {
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    [JsonPropertyName("key")] public string Key { get; set; } = string.Empty;
}


public interface IScoreBreakdown {
    int Year { get; }
}

// 2015 Recycle Rush
public class ScoreBreakdown2015 : IScoreBreakdown {
    public int Year => 2015;

    [JsonPropertyName("blue")] public AllianceScoreBreakdown2015 Blue { get; set; } = new();

    [JsonPropertyName("red")] public AllianceScoreBreakdown2015 Red { get; set; } = new();

    [JsonPropertyName("coopertition")] public string? Coopertition { get; set; }

    [JsonPropertyName("coopertition_points")] public int? CoopertitionPoints { get; set; }
}

public class AllianceScoreBreakdown2015 {
    [JsonPropertyName("container_points")] public int ContainerPoints { get; set; }

    [JsonPropertyName("tote_points")] public int TotePoints { get; set; }

    [JsonPropertyName("litter_points")] public int LitterPoints { get; set; }

    [JsonPropertyName("tote_count_far")] public int ToteCountFar { get; set; }

    [JsonPropertyName("tote_count_near")] public int ToteCountNear { get; set; }

    [JsonPropertyName("tote_set")] public bool ToteSet { get; set; }

    [JsonPropertyName("tote_stack")] public bool ToteStack { get; set; }

    [JsonPropertyName("container_count_level1")]
    public int ContainerCountLevel1 { get; set; }

    [JsonPropertyName("container_count_level2")]
    public int ContainerCountLevel2 { get; set; }

    [JsonPropertyName("container_count_level3")]
    public int ContainerCountLevel3 { get; set; }

    [JsonPropertyName("container_count_level4")]
    public int ContainerCountLevel4 { get; set; }

    [JsonPropertyName("container_count_level5")]
    public int ContainerCountLevel5 { get; set; }

    [JsonPropertyName("container_count_level6")]
    public int ContainerCountLevel6 { get; set; }

    [JsonPropertyName("container_set")] public bool ContainerSet { get; set; }

    [JsonPropertyName("litter_count_container")]
    public int LitterCountContainer { get; set; }

    [JsonPropertyName("litter_count_landfill")]
    public int LitterCountLandfill { get; set; }

    [JsonPropertyName("litter_count_unprocessed")]
    public int LitterCountUnprocessed { get; set; }

    [JsonPropertyName("robot_set")] public bool RobotSet { get; set; }

    [JsonPropertyName("auto_points")] public int AutoPoints { get; set; }
    [JsonPropertyName("teleop_points")] public int TeleopPoints { get; set; }
    [JsonPropertyName("foul_points")] public int FoulPoints { get; set; }
    [JsonPropertyName("adjust_points")] public int AdjustPoints { get; set; }
    [JsonPropertyName("total_points")] public int TotalPoints { get; set; }
    [JsonPropertyName("foul_count")] public int FoulCount { get; set; }
}

// 2025 Reefscape
public class ScoreBreakdown2025 : IScoreBreakdown {
    public int Year => 2025;

    [JsonPropertyName("blue")] public AllianceScoreBreakdown2025 Blue { get; set; } = new();

    [JsonPropertyName("red")] public AllianceScoreBreakdown2025 Red { get; set; } = new();

    [JsonPropertyName("coopertition")] public string? Coopertition { get; set; }

    [JsonPropertyName("coopertition_points")]
    public int? CoopertitionPoints { get; set; }
}

public class AllianceScoreBreakdown2025 {
    [JsonPropertyName("algaePoints")] public int AlgaePoints { get; set; }

    [JsonPropertyName("autoBonusAchieved")]
    public bool AutoBonusAchieved { get; set; }

    [JsonPropertyName("autoCoralCount")] public int AutoCoralCount { get; set; }

    [JsonPropertyName("autoCoralPoints")] public int AutoCoralPoints { get; set; }

    [JsonPropertyName("autoLineRobot1")] public string? AutoLineRobot1 { get; set; }

    [JsonPropertyName("autoLineRobot2")] public string? AutoLineRobot2 { get; set; }

    [JsonPropertyName("autoLineRobot3")] public string? AutoLineRobot3 { get; set; }

    [JsonPropertyName("autoMobilityPoints")]
    public int AutoMobilityPoints { get; set; }

    [JsonPropertyName("autoReef")] public Reef? AutoReef { get; set; }

    [JsonPropertyName("bargeBonusAchieved")]
    public bool BargeBonusAchieved { get; set; }

    [JsonPropertyName("coopertitionCriteriaMet")]
    public bool CoopertitionCriteriaMet { get; set; }

    [JsonPropertyName("coralBonusAchieved")]
    public bool CoralBonusAchieved { get; set; }

    [JsonPropertyName("endGameBargePoints")]
    public int EndGameBargePoints { get; set; }

    [JsonPropertyName("endGameRobot1")] public string? EndGameRobot1 { get; set; }

    [JsonPropertyName("endGameRobot2")] public string? EndGameRobot2 { get; set; }

    [JsonPropertyName("endGameRobot3")] public string? EndGameRobot3 { get; set; }

    [JsonPropertyName("g206Penalty")] public bool G206Penalty { get; set; }

    [JsonPropertyName("g410Penalty")] public bool G410Penalty { get; set; }

    [JsonPropertyName("g418Penalty")] public bool G418Penalty { get; set; }

    [JsonPropertyName("g428Penalty")] public bool G428Penalty { get; set; }

    [JsonPropertyName("netAlgaeCount")] public int NetAlgaeCount { get; set; }

    [JsonPropertyName("rp")] public int RankingPoints { get; set; }

    [JsonPropertyName("techFoulCount")] public int TechFoulCount { get; set; }

    [JsonPropertyName("teleopCoralCount")] public int TeleopCoralCount { get; set; }

    [JsonPropertyName("teleopCoralPoints")]
    public int TeleopCoralPoints { get; set; }

    [JsonPropertyName("teleopReef")] public Reef? TeleopReef { get; set; }

    [JsonPropertyName("wallAlgaeCount")] public int WallAlgaeCount { get; set; }

    [JsonPropertyName("autoPoints")] public int AutoPoints { get; set; }
    [JsonPropertyName("teleopPoints")] public int TeleopPoints { get; set; }
    [JsonPropertyName("foulPoints")] public int FoulPoints { get; set; }
    [JsonPropertyName("adjustPoints")] public int AdjustPoints { get; set; }
    [JsonPropertyName("totalPoints")] public int TotalPoints { get; set; }
    [JsonPropertyName("foulCount")] public int FoulCount { get; set; }
}

public class Reef {
    [JsonPropertyName("botRow")] public ReefRow BotRow { get; set; } = new();

    [JsonPropertyName("midRow")] public ReefRow MidRow { get; set; } = new();

    [JsonPropertyName("topRow")] public ReefRow TopRow { get; set; } = new();

    [JsonPropertyName("trough")] public int Trough { get; set; }

    [JsonPropertyName("tba_botRowCount")] public int TbaBotRowCount { get; set; }

    [JsonPropertyName("tba_midRowCount")] public int TbaMidRowCount { get; set; }

    [JsonPropertyName("tba_topRowCount")] public int TbaTopRowCount { get; set; }
}

public class ReefRow {
    [JsonPropertyName("nodeA")] public bool NodeA { get; set; }
    [JsonPropertyName("nodeB")] public bool NodeB { get; set; }
    [JsonPropertyName("nodeC")] public bool NodeC { get; set; }
    [JsonPropertyName("nodeD")] public bool NodeD { get; set; }
    [JsonPropertyName("nodeE")] public bool NodeE { get; set; }
    [JsonPropertyName("nodeF")] public bool NodeF { get; set; }
    [JsonPropertyName("nodeG")] public bool NodeG { get; set; }
    [JsonPropertyName("nodeH")] public bool NodeH { get; set; }
    [JsonPropertyName("nodeI")] public bool NodeI { get; set; }
    [JsonPropertyName("nodeJ")] public bool NodeJ { get; set; }
    [JsonPropertyName("nodeK")] public bool NodeK { get; set; }
    [JsonPropertyName("nodeL")] public bool NodeL { get; set; }
}

public class ScoreBreakdownConverter : JsonConverter<IScoreBreakdown> {
    public override IScoreBreakdown? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        // Check if blue alliance has year-specific properties
        if (root.TryGetProperty("blue", out var blueElement)) {
            // 2025: has algaePoints, coralBonusAchieved
            if (blueElement.TryGetProperty("algaePoints", out _)) {
                return JsonSerializer.Deserialize<ScoreBreakdown2025>(root.GetRawText(), options);
            }

            // 2015: has containerPoints, totePoints
            if (blueElement.TryGetProperty("containerPoints", out _) ||
                blueElement.TryGetProperty("container_points", out _)) {
                return JsonSerializer.Deserialize<ScoreBreakdown2015>(root.GetRawText(), options);
            }
        }

        // Default to 2025
        return JsonSerializer.Deserialize<ScoreBreakdown2025>(root.GetRawText(), options);
    }

    public override void Write(Utf8JsonWriter writer, IScoreBreakdown value, JsonSerializerOptions options) {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
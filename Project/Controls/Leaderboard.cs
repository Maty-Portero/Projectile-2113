using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

public class Leaderboard
{
    private List<LeaderboardEntry> _entries;
    private readonly string _filePath;

    public Leaderboard(string filePath)
    {
        _filePath = filePath;
        _entries = new List<LeaderboardEntry>();

        Load();
    }

    public void AddEntry(string playerID, int score, string mode)
    {
        _entries.Add(new LeaderboardEntry { PlayerID = playerID, Score = score, Mode = mode});
        Save();
    }

    public List<LeaderboardEntry> GetTopEntries(int count = 10)
    {
        return _entries.OrderByDescending(entry => entry.Score).Take(count).ToList();
    }
    public List<LeaderboardEntry> GetLastEntries(int count = 10)
    {
        return _entries
            .TakeLast(count)  // Tomar los últimos `count` elementos
            .Reverse()        // Invertir el orden de los elementos para mostrarlos en el orden en que llegaron
            .ToList();
    }
    public List<LeaderboardEntry> GetAllEntries()
    {
        return _entries;
    }



    public void Clear()
    {
        _entries.Clear();
        // Agregar algunas entradas de prueba
        AddEntry("AGUANTE",0, "test");
        AddEntry("LA",0, "test");
        AddEntry("SUIZA",0, "test");
        AddEntry("ET", 26, "test");
        AddEntry("RADIO", 0, "test");
        AddEntry("PARIS", 0, "test");
        AddEntry("TYPHOON", 0, "test");
        AddEntry("INTERACTIVE", 0, "test");
        AddEntry("LSDC", 0, "test");
        AddEntry("BAUCHI", 0, "test");
    }
    private void Save()
    {
        var jsonString = JsonSerializer.Serialize(_entries);
        File.WriteAllText(_filePath, jsonString);
    }

    private void Load()
    {
        if (File.Exists(_filePath))
        {
            var jsonString = File.ReadAllText(_filePath);
            _entries = JsonSerializer.Deserialize<List<LeaderboardEntry>>(jsonString);
        }
    }
}

public class LeaderboardEntry
{
    public string PlayerID { get; set; }
    public int Score { get; set; }
    public string Mode { get; set; }
}

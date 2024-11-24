public class PlayerService
{
    private readonly Dictionary<string, Player> _players = new Dictionary<string, Player>();

    public void AddPlayer(string nickname, string deviceId)
    {
        var key = $"{nickname}_{deviceId}";
        if (!_players.ContainsKey(key))
        {
            _players[key] = new Player { Nickname = nickname, DeviceId = deviceId, MatchedPairsCount = 0 };
        }
    }

    public void UpdatePlayerMatchedPairs(string nickname, string deviceId, int matchedPairs)
    {
        var key = $"{nickname}_{deviceId}";
        if (_players.ContainsKey(key))
        {
            _players[key].MatchedPairsCount = matchedPairs;
        }
    }

    public List<Player> GetPlayers()
    {
        return _players.Values.ToList();
    }
}

public class Player
{
    public string Nickname { get; set; }
    public string DeviceId { get; set; }
    public int MatchedPairsCount { get; set; }
}
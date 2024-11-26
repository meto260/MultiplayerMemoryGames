public class PlayerService {
    private readonly Dictionary<string, Dictionary<string, Player>> _channelPlayers = new Dictionary<string, Dictionary<string, Player>>();

    public void AddPlayer(string channelName, string nickname, string deviceId) {
        if (!_channelPlayers.ContainsKey(channelName)) {
            _channelPlayers[channelName] = new Dictionary<string, Player>();
        }

        var key = $"{nickname}_{deviceId}";

        // E�er oyuncu zaten varsa son aktivite zaman�n� g�ncelle
        if (_channelPlayers[channelName].ContainsKey(key)) {
            _channelPlayers[channelName][key].LastActivityTime = DateTime.UtcNow;
        } else {
            // Yeni oyuncu ekle
            _channelPlayers[channelName][key] = new Player {
                Nickname = nickname,
                DeviceId = deviceId,
                MatchedPairsCount = 0,
                LastActivityTime = DateTime.UtcNow
            };
        }
    }

    public void UpdatePlayerMatchedPairs(string channelName, string nickname, string deviceId, int matchedPairs) {
        if (!_channelPlayers.ContainsKey(channelName)) {
            // E�er kanal yoksa yeni kanal� olu�tur
            _channelPlayers[channelName] = new Dictionary<string, Player>();
        }

        var key = $"{nickname}_{deviceId}";

        // Oyuncu yoksa ekle
        if (!_channelPlayers[channelName].ContainsKey(key)) {
            AddPlayer(channelName, nickname, deviceId);
        }

        // Oyuncunun bilgilerini g�ncelle
        _channelPlayers[channelName][key].MatchedPairsCount = matchedPairs;
        _channelPlayers[channelName][key].LastActivityTime = DateTime.UtcNow;
    }

    public List<Player> GetPlayers(string channelName) {
        if (_channelPlayers.ContainsKey(channelName)) {
            // 1 dakikadan fazla s�re ge�en oyuncular� temizle
            RemoveInactivePlayers(channelName);
            return _channelPlayers[channelName].Values.ToList();
        }
        return new List<Player>();
    }

    private void RemoveInactivePlayers(string channelName) {
        var inactivePlayers = _channelPlayers[channelName]
            .Where(p => (DateTime.UtcNow - p.Value.LastActivityTime).TotalMinutes > 1)
            .Select(p => p.Key)
            .ToList();

        foreach (var playerKey in inactivePlayers) {
            _channelPlayers[channelName].Remove(playerKey);
        }
    }
}

public class Player {
    public string Nickname { get; set; }
    public string DeviceId { get; set; }
    public int MatchedPairsCount { get; set; }
    public DateTime LastActivityTime { get; set; }
}
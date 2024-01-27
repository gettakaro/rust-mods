using ConVar;
using Newtonsoft.Json;
using UnityEngine;

namespace Oxide.Plugins;

[Info("Hook Parser", "Strobez", "0.0.1")]
internal class HookParser : RustPlugin
{
    #region Oxide Hooks

    private void OnPlayerConnected(BasePlayer player)
    {
        Hook hook = new(Type.PLAYER_CONNECTED, new PlayerConnected
        {
            Player = GetTakaroPlayer(player)
        });

        LogParsedJson(hook);
    }

    private void OnPlayerDisconnected(BasePlayer player, string reason)
    {
        Hook hook = new(Type.PLAYER_DISCONNECTED, new PlayerDisconnected
        {
            Player = GetTakaroPlayer(player)
        });

        LogParsedJson(hook);
    }

    private void OnPlayerChat(BasePlayer player, string? message, Chat.ChatChannel channel)
    {
        if (player == null || message == null) return;

        Hook hook = new(Type.CHAT_MESSAGE, new ChatMessage
        {
            Player = GetTakaroPlayer(player),
            Message = message,
            Channel = channel.ToString()
        });

        LogParsedJson(hook);
    }

    private void OnPlayerDeath(BasePlayer player, HitInfo? info)
    {
        if (player == null || info == null) return;

        BasePlayer attacker = info.InitiatorPlayer;
        if (attacker == null) return;

        Hook hook = new(Type.PLAYER_DEATH, new PlayerDeath
        {
            Player = GetTakaroPlayer(player),
            Attacker = GetTakaroPlayer(attacker),
            Position = player.transform.position,
            Weapon = info.WeaponPrefab.ShortPrefabName ?? "Unknown"
        });

        LogParsedJson(hook);
    }

    private void OnEntityDeath(BaseCombatEntity entity, HitInfo? info)
    {
        if (entity == null || info == null) return;

        if (entity is BasePlayer) return;

        BasePlayer player = info.InitiatorPlayer;
        if (player == null) return;

        Hook hook = new(Type.ENTITY_KILLED, new EntityKilled
        {
            Player = GetTakaroPlayer(player),
            Entity = entity.ShortPrefabName,
            Weapon = info.WeaponPrefab.ShortPrefabName
        });

        LogParsedJson(hook);
    }

    #endregion
    
    #region Helpers
    private TakaroPlayer GetTakaroPlayer(BasePlayer player)
    {
        return new()
        {
            Name = player.displayName,
            SteamId = player.userID,
            Ip = player.net.connection.ipaddress,
            Ping = Network.Net.sv.GetAveragePing(player.net.connection)
        };
    }
    
    private void LogParsedJson(Hook hook)
    {
        string stringifyJson = JsonConvert.SerializeObject(hook, Formatting.None);
        Puts(stringifyJson);
    }
    #endregion

    #region Classes
    private enum Type
    {
        PLAYER_CONNECTED = 0,
        PLAYER_DISCONNECTED = 1,
        CHAT_MESSAGE = 2,
        PLAYER_DEATH = 3,
        ENTITY_KILLED = 4
    }

    private class TakaroPlayer
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("steamId")]
        public ulong SteamId { get; set; }
        
        [JsonProperty("ip")]
        public string? Ip { get; set; }
        
        [JsonProperty("ping")]
        public int Ping { get; set; }
    }
    
    private class Hook
    {
        [JsonProperty("type")]
        public Type Type { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
        
        public Hook(Type type, object data)
        {
            Type = type;
            Data = data;
        }
    }

    private class PlayerConnected
    {
        [JsonProperty("player")]
        public TakaroPlayer? Player { get; set; }
    }

    private class PlayerDisconnected
    {
        [JsonProperty("player")]
        public TakaroPlayer? Player { get; set; }
    }

    private class ChatMessage
    {
        [JsonProperty("player")]
        public TakaroPlayer? Player { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("channel")]
        public string? Channel { get; set; }
    }

    private class PlayerDeath
    {
        [JsonProperty("player")]
        public TakaroPlayer? Player { get; set; }

        [JsonProperty("attacker")]
        public TakaroPlayer? Attacker { get; set; }

        [JsonProperty("position")]
        public Vector3 Position { get; set; }
        
        [JsonProperty("weapon")]
        public string? Weapon { get; set; }
    }

    private class EntityKilled
    {
        [JsonProperty("player")]
        public TakaroPlayer? Player { get; set; }

        [JsonProperty("entity")]
        public string? Entity { get; set; }

        [JsonProperty("weapon")]
        public string? Weapon { get; set; }
    }
    #endregion
}
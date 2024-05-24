using Photon.Realtime;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{

    public const string READY = "Ready";
    public const string LOAD = "Load";
    public static bool GetReady( this Player player )
    {
        PhotonHashtable customProperty = player.CustomProperties;
        bool ready;
        if ( customProperty.TryGetValue(READY, out object value) )
        {
            ready = ( bool )value;
        }
        else
        {
            ready = false;
        }
        return ready;
    }
    static PhotonHashtable Propertys = new PhotonHashtable();
    public static void SetReady( this Player player, bool value )
    {
        Propertys.Clear();
        Propertys [READY] = value;
        player.SetCustomProperties(Propertys);
    }
    public static bool GetLoad( this Player player )
    {
        PhotonHashtable customProperty = player.CustomProperties;
        bool ready;
        if ( customProperty.TryGetValue(LOAD, out object value) )
        {
            ready = ( bool )value;
        }
        else
        {
            ready = false;
        }
        return ready;
    }
    public static void SetLoad( this Player player, bool value )
    {
        Propertys.Clear();
        Propertys [LOAD] = value;
        player.SetCustomProperties(Propertys);
    }

    public const string GAMESTART = "GameStart";
    public static bool GetGameStart( this Room room )
    {
        PhotonHashtable customProperty = room.CustomProperties;
        bool ready;
        if ( customProperty.TryGetValue(LOAD, out object value) )
        {
            ready = ( bool )value;
        }
        else
        {
            ready = false;
        }
        return ready;
    }
    public static void SetGameStart( this Room player, bool value )
    {
        Propertys.Clear();
        Propertys [GAMESTART] = value;
        player.SetCustomProperties(Propertys);

    }

    public const string GMAESTARTTIME = "GaemeStartTime";

    public static double GetGameStartTimne( this Room room )
    {
        PhotonHashtable customProperty = room.CustomProperties;
        if ( customProperty.TryGetValue(GMAESTARTTIME, out object value) )
        {
            return ( double )value;
        }
        else
        {
            return 0;
        }
    }
    public static void SetGameStartTime( this Room player, double value )
    {
        Propertys.Clear();
        Propertys [GMAESTARTTIME] = value;
        player.SetCustomProperties(Propertys);

    }

    public const string PLAYERSTATE = "PlayerState";
    public static PlayerState GetState( this Player player )
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if ( customProperty.TryGetValue(PLAYERSTATE, out object value) )
        {
            return ( PlayerState )value;
        }
        else
        {
            return PlayerState.Size;
        }
    }
    public static void SetState( this Player player, PlayerState state )
    {
        Propertys.Clear();
        Propertys [PLAYERSTATE] = state;
        player.SetCustomProperties(Propertys);
    }

    public const string GAMEMODE = "GameMode";
    public static GameMode GetMode( this Room room )
    {
        PhotonHashtable customProperty = room.CustomProperties;
        if ( customProperty.TryGetValue(GAMEMODE, out object value) )
        {
            return ( GameMode )value;
        }
        else
        {
            return GameMode.normal;
        }
    }
    public static void SetMode( this Room room, GameMode gameMode )
    {
        Propertys.Clear();
        Propertys [GAMEMODE] = gameMode;
        room.SetCustomProperties(Propertys);
    }
    public const string KILLCOUNT = "KillCount";
    public static int GetKillCount(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if( customProperty.TryGetValue(KILLCOUNT, out object value) )
        {
            return ( int )value;
        }
        else
        {
            return 0;
        }
    }

    public static void SetKillCount( this Player player, int KillPoint )
    {
        Propertys.Clear();
        Propertys [KILLCOUNT] = KillPoint;
        player.SetCustomProperties(Propertys);
    }
}

public enum PlayerState
{
    Live,
    Die,
    Default,
    Size
}
public enum GameMode
{
    normal,
    mission,
    Item
}

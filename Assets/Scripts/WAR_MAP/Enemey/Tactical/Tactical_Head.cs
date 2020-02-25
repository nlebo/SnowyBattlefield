using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tactical_Head : MonoBehaviour
{
    protected static Tile_Manager _Tile;
    protected static List<Enemy_Manager> _Enemys;
    protected static List<Unit_Manager> _Players;

    public Enemy_Manager _this;
    public bool _Idle, _Meet;

    protected Coroutine _MEET, _IDLE;
    protected void Start()
    {
        if (_Tile == null) _Tile = Tile_Manager.m_Tile_Manager;
        
    }


    public static void AddEnemy(Enemy_Manager _Enemy)
    {
        if (_Enemys == null) _Enemys = new List<Enemy_Manager>();
        _Enemys.Add(_Enemy);
    }
    public static void DeleteEnemy(Enemy_Manager _Enemy)
    {
        if (_Enemys.Contains(_Enemy)) _Enemys.Remove(_Enemy);
    }
    public static void AddPlayer(Unit_Manager _Player)
    {
        if (_Players == null) _Players = new List<Unit_Manager>();
        _Players.Add(_Player);
    }

    public static int GetPlayerCount()
    {
        return _Players.Count;
    }
    public static Unit_Manager GetPlayer(int i)
    {
        return _Players[i];
    }


    public static int GetEnemyCount()
    {
        return _Enemys.Count;
    }

    public static Unit_Manager GetEnemy(int i)
    {
        return _Enemys[i];
    }


    public virtual void Idle()
    {
       
    }

    public virtual void Meet()
    {
    }

    
}

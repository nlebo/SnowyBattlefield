using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : Tactical_Head
{
    Vector2 Goal;



    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Goal.x = Random.Range(
            ((int)_Tile.SpawnPoint[0].x - 6) > 0 ? (int)_Tile.SpawnPoint[0].x - 6 : 0,
            ((int)_Tile.SpawnPoint[0].x + 7) >= _Tile.X ? _Tile.X - 1 : (int)_Tile.SpawnPoint[0].x + 7);
        Goal.y = Random.Range(
                ((int)_Tile.SpawnPoint[0].y - 6) > 0 ? (int)_Tile.SpawnPoint[0].y - 6 : 0,
                ((int)_Tile.SpawnPoint[0].y + 7) >= _Tile.Y ? _Tile.Y - 1 : (int)_Tile.SpawnPoint[0].y + 7);

        if (_this._CLASS == Enemy_Manager.CLASS.AggresiveScout || _this._CLASS == Enemy_Manager.CLASS.DefensiveScout)
        {
            Tag_Player = new List<Unit_Manager>();
            UnTageed_Player = new List<Unit_Manager>();
            for (int i = 0; i < _Players.Count; i++)
            {
                UnTageed_Player.Add(_Players[i]);
            }
        }

        _this.MakeNav(Goal);
    }

    public override void Idle()
    {
        base.Idle();

        switch (_this._CLASS)
        {
            case Enemy_Manager.CLASS.Default:
                _IDLE = StartCoroutine(IDLE());
                break;
            case Enemy_Manager.CLASS.AggresiveScout:
                _IDLE = _this.SpecialFlag == true ? StartCoroutine(ScoutIDLE()) : StartCoroutine(IDLE());
                break;
        }
    }

    public override void Meet()
    {
        base.Meet();

        switch (_this._CLASS)
        {
            case Enemy_Manager.CLASS.Default:
                _MEET = StartCoroutine(MEET());
                break;
            case Enemy_Manager.CLASS.AggresiveScout:
                _MEET = _this.SpecialFlag == true ? StartCoroutine(AgressiveScoutMEET()) : StartCoroutine(MEET());
                break;
        }
        
    }

    IEnumerator IDLE()
    {
        if (_this.EveryView())
        {
            _this.SharingVison(100);
        }

        if (_this.GetNav().Count == 0 && _this.stay <= 0)
        {
            _this.MakeNav(Goal);
        }
        else if (_this.GetNav().Count == 0 && _this.stay > 0)
        {
            _this.stay--;
        }

        _this.StartMove();
        while (!_this.MovingNow)
        {
            yield return null;
        }
        _this.MovingNow = false;


        _this.Seek();
        while (!_this.Watching)
        {
            yield return null;
        }

        if(_this.seek) _this.loop = true;
        _Idle = true;
        yield return null;
    }


    IEnumerator MEET()
    {
        
        _this.Tracking = false;

        if (_this.GetMeetPlayer().Count > _this.CloserPlayer())
        {

            if (!_this.CoverCheck(_this.GetMeetPlayer()[_this.CloserPlayer()]))
            {

                _this.MovingNow = false;
                _this.StartMove();
                while (!_this.MovingNow)
                {
                    yield return null;
                }

            }

            if (_this.CloserPlayer() != -1)
            {
                while (_this.Attack())
                {
                    if (_this.GetMeetPlayer()[_this.CloserPlayer()] == null) break;
                    yield return new WaitForSeconds(3f);
                }
            }

        }

        _Meet = true;
        yield return null;
    }

    IEnumerator ScoutIDLE()
    {
        
        Unit_Manager CloserPlayer = _this.CloserPlayer(ref UnTageed_Player);
        Vector2 _Goal = new Vector2(CloserPlayer.x,CloserPlayer.y);
        _this.ScoutMakeNav(Goal);

        if(_this.Player_See.Count > 0)
        {
            _this.seek = true;
             _Idle = true;
            StopCoroutine(_IDLE);
            yield return null;
        }

        _this.StartMove();
        while (!_this.MovingNow)
        {
            yield return null;
        }
        _this.MovingNow = false;


        _this.Seek();
        while (!_this.Watching)
        {
            yield return null;
        }

        if(_this.seek){
            Tag_Player.Add(_this.GetMeetPlayer()[_this.CloserPlayer()]);
            UnTageed_Player.Remove(_this.GetMeetPlayer()[_this.CloserPlayer()]);
        }

        for(int i = 0;i<_Players.Count;i++)
        {
            if(!Tag_Player.Contains(_Players[i]))
            {
                break;
            }

            if(i == _Players.Count - 1) _this.SpecialFlag = false;
        }

         _Idle = true;
        yield return null;
    }
    IEnumerator AgressiveScoutMEET()
    {
         _this.Tracking = false;

        if (_this.GetMeetPlayer().Count > _this.CloserPlayer())
        {

            if (!_this.CoverCheck(_this.GetMeetPlayer()[_this.CloserPlayer()]))
            {
                if (_this.SpecialFlag)
                {
                    _IDLE = StartCoroutine(ScoutIDLE());
                   while(!_Idle)
                   {
                       yield return null;
                   }
                   yield return null;
                }
                else
                {
                    _this.MovingNow = false;
                    _this.StartMove();
                    while (!_this.MovingNow)
                    {
                        yield return null;
                    }
                }

            }

            if (_this.CloserPlayer() != -1)
            {
                while (_this.Attack())
                {
                    if (_this.GetMeetPlayer()[_this.CloserPlayer()] == null) break;
                    yield return new WaitForSeconds(3f);
                }
            }
            

        }

        _Meet = true;
        yield return null;
    }
}

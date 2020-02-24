using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siege : Tactical_Head
{
    enum flag { Move_Forward, ByPass };

    Vector2[] Goal;
    static int Count = 0;
    int Stay = 1;
    flag _flag;
    int Locate;
    
    int arriveflag;

    new void Start()
    {
        base.Start();
        arriveflag = 0;

        Goal = new Vector2[] { Vector2.zero, Vector2.zero };

        if (Count < SpawnPocket.MaxEnemyCount / 2)
        {
            if (Mathf.Abs(_Tile.SpawnPoint[0].x - _Tile.SpawnPoint[1].x) > Mathf.Abs(_Tile.SpawnPoint[0].y - _Tile.SpawnPoint[1].y))
            {
                if (Mathf.Abs(_Tile.SpawnPoint[0].y - _Tile.Y) > Mathf.Abs(_Tile.SpawnPoint[0].y - 0))
                    Goal[0].y = Random.Range(_Tile.Y - 4, _Tile.Y);
                else
                    Goal[0].y = Random.Range(0, 4);

                Goal[0].x = _this.x;
                Goal[1].x = _Tile.SpawnPoint[0].x;
                Goal[1].y = Goal[0].y;

                if (_Tile.SpawnPoint[0].x - _Tile.SpawnPoint[1].x > 0)
                {
                    Locate = 1;
                    Goal[1].x = _Tile.X - 1;
                }
                else
                {
                    Locate = 2;
                    Goal[1].x = 0;
                }

            }
            else
            {
                if (Mathf.Abs(_Tile.SpawnPoint[0].x - _Tile.X) > Mathf.Abs(_Tile.SpawnPoint[0].x - 0))
                    Goal[0].x = Random.Range(_Tile.X - 4, _Tile.X);
                else
                    Goal[0].x = Random.Range(0, 4);

                Goal[0].y = _this.y;
                Goal[1].y = _Tile.SpawnPoint[0].y;
                Goal[1].x = Goal[0].x;

                if (_Tile.SpawnPoint[0].y - _Tile.SpawnPoint[1].y > 0)
                {
                    Locate = 1;
                    Goal[1].y = _Tile.Y - 1;
                }
                else
                {
                    Locate = 2;
                    Goal[1].y = 0;
                }
            }

            Count++;
            _this.Watch_Stop = false;
            _flag = flag.ByPass;
        }
        else
        {
            Goal[0].x = Random.Range(
            ((int)_Tile.SpawnPoint[0].x - 6) > 0 ? (int)_Tile.SpawnPoint[0].x - 6 : 0,
            ((int)_Tile.SpawnPoint[0].x + 7) >= _Tile.X ? _Tile.X - 1 : (int)_Tile.SpawnPoint[0].x + 7);
            Goal[0].y = Random.Range(
            ((int)_Tile.SpawnPoint[0].y - 6) > 0 ? (int)_Tile.SpawnPoint[0].y - 6 : 0,
            ((int)_Tile.SpawnPoint[0].y + 7) >= _Tile.Y ? _Tile.Y - 1 : (int)_Tile.SpawnPoint[0].y + 7);

            _flag = flag.Move_Forward;
        }

        _this.MakeNav(Goal[0]);
    }

    public override void Idle()
    {
        base.Idle();
        _IDLE = null;
        switch(_flag){
            case flag.Move_Forward:
                _IDLE = StartCoroutine(IDLE());
                break;
            case flag.ByPass:
                _IDLE = StartCoroutine(IDLE_BYPASS());
                break;
        }
    }

    public override void Meet()
    {
        base.Meet();
        _MEET = null;
        _MEET = StartCoroutine(MEET());
    }

    IEnumerator IDLE()
    {
        yield return null;
        if (Stay > 0)
        {
            Stay--;
            _Idle = true;
            StopCoroutine(_IDLE);
            yield return null;
        }

        if (!_this.Tracking && _this.stay <= 0)
        {
            _this.SharingVison();
        }

        if (_this.GetNav().Count == 0 && _this.stay <= 0)
        {
            _this.MakeNav(Goal[0]);
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

        if (_this.seek)
        {
            Meet();
            while (!_Meet) { yield return null; }
        }

        _Idle = true;
        yield return null;
    }
    IEnumerator IDLE_BYPASS()
    {
        _this.StartMove();
        while (!_this.MovingNow)
        {
            yield return null;
        }
        _this.MovingNow = false;


        if (arriveflag <= 1)
        {
            if ( arriveflag == 0 && _this.GetNav().Count == 0)
            {
                if (arriveflag == 0) arriveflag = 1;

                _this.MakeNav(Goal[1]);
            }

            _this.StartMove();
            while (!_this.MovingNow)
            {
                yield return null;
            }
            _this.MovingNow = false;

            if (arriveflag == 1)
            {
                int MaxD = -1;
                if (Goal[1].x == _this.x)
                {
                    for (int i = 0; i < _Players.Count; i++)
                    {
                        if (Locate == 1)
                        {
                            if (_Players[i].y > MaxD) MaxD = _Players[i].y;
                        }
                        else
                        {
                            if (_Players[i].y < MaxD || MaxD == -1) MaxD = _Players[i].y;
                        }
                    }

                    if (Locate == 1)
                    {
                        if (MaxD + 2 < _this.y || _this.y >= _Tile.Y - 1) arriveflag = 2;
                    }
                    else
                    {
                        if (MaxD - 2 > _this.y || _this.y <= 0) arriveflag = 2;
                    }
                }
                else
                {
                    for (int i = 0; i < _Players.Count; i++)
                    {
                        if (Locate == 1)
                        {
                            if (_Players[i].x > MaxD) MaxD = _Players[i].x;
                        }
                        else
                        {
                            if (_Players[i].x < MaxD || MaxD == -1) MaxD = _Players[i].x;
                        }
                    }

                    if (Locate == 1)
                    {
                        if (MaxD + 2 < _this.x || _this.x >= _Tile.X - 1) arriveflag = 2;
                    }
                    else
                    {
                        if (MaxD - 2 > _this.x || _this.x <= 0) arriveflag = 2;
                    }
                }
            }

            if (arriveflag <= 1)
            {
                _Idle = true;
                StopCoroutine(_IDLE);
                yield return null;
            }

            _this.Watch_Stop = true;
        }

        if (!_this.Tracking && _this.stay <= 0)
        {
            _this.SharingVison();
        }

        if (_this.GetNav().Count == 0 && _this.stay <= 0)
        {
            int MaxD = -1;
            int _x=0, _y=0;
            if (Goal[1].x == _this.x)
            {
                for (int i = 0; i < _Players.Count; i++)
                {
                    if (Goal[1].y > _this.y)
                    {
                        if (_Players[i].y > MaxD)
                        {
                            MaxD = _Players[i].y;
                            _x = _Players[i].x;
                            _y = _Players[i].y;
                        }
                    }
                    else
                    {
                        if (_Players[i].y < MaxD || MaxD == -1)
                        {
                            MaxD = _Players[i].y;
                            _x = _Players[i].x;
                            _y = _Players[i].y;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _Players.Count; i++)
                {
                    if (Goal[1].x > _this.x)
                    {
                        if (_Players[i].x > MaxD)
                        {
                            MaxD = _Players[i].x;
                            _x = _Players[i].x;
                            _y = _Players[i].y;
                        }
                    }
                    else
                    {
                        if (_Players[i].x < MaxD || MaxD == -1)
                        {
                            MaxD = _Players[i].x;
                            _x = _Players[i].x;
                            _y = _Players[i].y;
                        }
                    }
                }
            }
            Goal[1] = new Vector2(_x, _y);
            _this.MakeNav(Goal[1]);
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


        if (_this.seek)
        {
            Meet();
            while (!_Meet) { yield return null; }
        }

        _Idle = true;

        yield return null;
    }

    IEnumerator MEET()
    {
        if (_flag == flag.ByPass && arriveflag != 2)
        {
            Idle();
            while (!_Idle)
            {
                yield return null;
            }
            _Idle = false;
            _Meet = true;
            StopCoroutine(_MEET);
            yield return null;
        }

        _this.stay = 1;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class By_Pass : Tactical_Head
{

    Vector2 Goal;
    Vector2[] ByPassGoal;

    [SerializeField]
    bool ByPass;
    int Locate;
    int arriveflag;

    new void Start()
    {
        base.Start();
        arriveflag = 0;
        Goal.x = Random.Range(
            ((int)_Tile.SpawnPoint[0].x - 6) > 0 ? (int)_Tile.SpawnPoint[0].x - 6 : 0,
            ((int)_Tile.SpawnPoint[0].x + 7) >= _Tile.X ? _Tile.X - 1 : (int)_Tile.SpawnPoint[0].x + 7);
        Goal.y = Random.Range(
                ((int)_Tile.SpawnPoint[0].y - 6) > 0 ? (int)_Tile.SpawnPoint[0].y - 6 : 0,
                ((int)_Tile.SpawnPoint[0].y + 7) >= _Tile.Y ? _Tile.Y - 1 : (int)_Tile.SpawnPoint[0].y + 7);
        ByPass = false;
        _this.MakeNav(Goal);
    }

    public override void Idle()
    {
        base.Idle();
        if (!ByPass)
            _IDLE = StartCoroutine(IDLE());
        else
            _IDLE = StartCoroutine(IDLE_BYPASS());
    }

    public override void Meet()
    {
        base.Meet();
        _MEET = StartCoroutine(MEET());
    }

    IEnumerator IDLE()
    {
        if (_this.EveryView())
        {
            _this.SharingVison(15);
            if (!_this.Tracking)
            {
                MakeByPass(_Players[FarPlayer()]);
                _this.MakeNav(ByPassGoal[0]);
                ByPass = true;
                yield return null;
                StopCoroutine(_IDLE);
                Idle();
                yield return null;
            }
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

        if (_this.seek)
        {
            _this.loop = true;
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
            if (_this.GetNav() != null && arriveflag == 0 && _this.GetNav().Count == 0)
            {
                arriveflag = 1;

                _this.MakeNav(ByPassGoal[1]);
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
                if (ByPassGoal[1].x == _this.x)
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
            int _x = 0, _y = 0;
            if (ByPassGoal[1].x == _this.x)
            {
                for (int i = 0; i < _Players.Count; i++)
                {
                    if (ByPassGoal[1].y > _this.y)
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
                    if (ByPassGoal[1].x > _this.x)
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
            ByPassGoal[1] = new Vector2(_x, _y);
            _this.MakeNav(ByPassGoal[1]);
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

    public void MakeByPass(Unit_Manager _Player)
    {
        ByPassGoal = new Vector2[] { Vector2.zero, Vector2.zero };
        if (Mathf.Abs(_this.x - _Player.x) <= Mathf.Abs(_this.y - _Player.y))
        {
            if (Mathf.Abs(_this.x - _Tile.X) >= _this.x)
            {
                ByPassGoal[0].x = 0;
                ByPassGoal[0].y = _this.y;
                
            }
            else
            {
                ByPassGoal[0].x = _Tile.X - 1;
                ByPassGoal[0].y = _this.y;
            }

            ByPassGoal[1].x = ByPassGoal[0].x;
            if (_this.y >= _Player.y)
            {
                Locate = 2;
                ByPassGoal[1].y = 0;

            }
            else
            {
                Locate = 1;
                ByPassGoal[1].y = _Tile.Y - 1;
            }
        }
        else
        {
            if (Mathf.Abs(_this.y - _Tile.Y) >= _this.y)
            {
                Locate = 1;
                ByPassGoal[0].y = 0;
                ByPassGoal[0].x = _this.x;
            }
            else
            {
                Locate = 2;
                ByPassGoal[0].y = _Tile.Y - 1;
                ByPassGoal[0].x = _this.x;
            }

            ByPassGoal[1].y = ByPassGoal[0].y;
            if (_this.x >= _Player.x)
            {
                Locate = 2;
                ByPassGoal[1].x = 0;

            }
            else
            {
                Locate = 1;
                ByPassGoal[1].x = _Tile.X - 1;
            }
        }
    }

    public int FarPlayer()
    {
        int maxDistance = -1;
        int _where = -1;

        for (int i = 0; i < _Players.Count; i++)
        {
            if (Mathf.Abs(_Players[i].x - _this.x) + Mathf.Abs(_Players[i].y - _this.y) >= maxDistance)
            {
                maxDistance = Mathf.Abs(_Players[i].x - _this.x) + Mathf.Abs(_Players[i].y - _this.y);
                _where = i;
            }
        }

        return _where;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Run : Tactical_Head
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

        _this.MakeNav(Goal);
    }

    public override void Idle()
    {
        base.Idle();
        _IDLE = StartCoroutine(IDLE());
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
            _this.SharingVison();
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
            Meet();
            while (!_Meet) { yield return null; }
        }

        _Idle = true;
        yield return null;
    }
    IEnumerator MEET()
    {
        _this.Tracking = false;
        

        if (_this.GetMeetPlayer().Count > _this.CloserPlayer())
        {
            if (_this.CloserPlayer() != -1)
            {
                _this.Attack();
                yield return new WaitForSeconds(2f);
            }

            if (_this.Now_Action_Point > 0)
            {
                MakeBehindRoot(_this.GetMeetPlayer()[_this.CloserPlayer()]);

                _this.MovingNow = false;
                _this.StartMove();
                while (!_this.MovingNow)
                {
                    yield return null;
                }
                
            }

        }

        _Meet = true;
        yield return null;
    }

    public void MakeBehindRoot(Unit_Manager _Player)
    {
        Vector2 _Goal = new Vector2(_this.x, _this.y);
        int X, Y;
        X = 0;
        Y = 0;
        int deep = 1;
        int RemainPoint = _this.Now_Action_Point;

        if (Mathf.Abs(_this.x - _Player.x) >= Mathf.Abs(_this.y - _Player.y))
        {
            if (_this.x >= _Player.x) X = 1;
            else X = -1;
        }
        else
        {
            if (_this.y >= _Player.y) Y = 1;
            else Y = -1;
        }


        while (RemainPoint > 0)
        {
            RemainPoint--;

            if (_Tile.TileMap[_this.x + (X * deep)][_this.y + (Y * deep)] == Tile_Manager.Cover_Kind.HalfCover)
            {
                if (RemainPoint > 0)
                    RemainPoint--;
                else
                    break;
            }

            if (_this.x + (X * deep) < 0 || _this.x + (X * deep) >= _Tile.X) break;
            else if (_this.y + (Y * deep) < 0 || _this.y + (Y * deep) >= _Tile.Y) break;

            _Goal.x = _this.x + (X * deep);
            _Goal.y = _this.y + (Y * deep);
            if (Mathf.Abs(_Player.x - (_this.x + (X * deep))) + Mathf.Abs(_Player.y - (_this.y + (Y * deep))) > _Player.View.ViewRange) break;
            deep++;
        }
        _this.MakeNav(_Goal);
    }
}

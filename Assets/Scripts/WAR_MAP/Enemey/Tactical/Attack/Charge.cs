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



}

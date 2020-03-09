using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defensive_Scout : Enemy_Manager
{
    public override void SelectClass()
    {
        _CLASS = CLASS.DefensiveScout;
        SpecialFlag = true;
    }
     public override bool Attack()
    {
        if(Player_See.Count > 0 || !SpecialFlag){
            SpecialFlag = false;
            return base.Attack();
        }
        else
        {
            seek = false;
            loop = true;
            return false;
        }
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        if(Player_See.Count > 0 || !SpecialFlag){
            SpecialFlag = false;
            return base.CoverCheck(_Unit);
        }
        else
        {
            seek = false;
            loop = true;
            return false;
        }
    }

    public override void Scout_A_Star(int _x, int _y, int px, int py, int count, int deep)
    {
        int r = 10000;
        int[] _Cost = { Now_Move_Point, Now_Move_Point, Now_Move_Point, Now_Move_Point };
        int[] pr = { -1, -1, -1, -1 };
        int rr = 10000;

        if (count >= cost || deep >= 1000)
            return;

        if (x == _x && y == _y)
        {

            cost = count;

            A_T.Clear();
            A_T = new List<Tile>();
            comple = true;
            return;
        }

        #region MoveInform
        if (_x - 1 >= 0)
        {
            if (CheckLoot(_x - 1,y) && _Tile.TileMap[_x - 1][_y] != Tile_Manager.Cover_Kind.HighCover && _x - 1 != px)
            {
                if (_Tile.TileMap[_x - 1][_y] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[0]++;

                pr[0] = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y) + _Cost[0] + count;
                r = pr[0];
                rr = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y);
            }
        }

        if (_x + 1 < _Tile.TileMap.Count)
        {
            if (CheckLoot(_x + 1,y) && _Tile.TileMap[_x + 1][_y] != Tile_Manager.Cover_Kind.HighCover && _x + 1 != px)
            {
                if (_Tile.TileMap[_x + 1][_y] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[1]++;

                pr[1] = Mathf.Abs(x - (_x + 1)) + Mathf.Abs(y - _y) + _Cost[1] + count;
                if (r > pr[1])
                {
                    r = pr[1];
                    rr = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y);
                }
            }
        }

        if (_y - 1 >= 0)
        {
            if (CheckLoot(_x,y - 1) && _Tile.TileMap[_x][_y - 1] != Tile_Manager.Cover_Kind.HighCover && _y - 1 != py)
            {
                if (_Tile.TileMap[_x][_y - 1] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[2]++;

                pr[2] = Mathf.Abs(x - _x) + Mathf.Abs(y - (_y - 1)) + _Cost[2] + count;
                if (r > pr[2])
                {
                    r = pr[2];
                    rr = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y);
                }
            }
        }

        if (CheckLoot(_x,y + 1) && _y + 1 < _Tile.TileMap[_x].Count)
        {
            if (_Tile.TileMap[_x][_y + 1] != Tile_Manager.Cover_Kind.HighCover && _y + 1 != py)
            {
                if (_Tile.TileMap[_x][_y + 1] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[3]++;

                pr[3] = Mathf.Abs(x - _x) + Mathf.Abs(y - (_y + 1)) + _Cost[3] + count;
                if (r > pr[3])
                {
                    r = pr[3];
                    rr = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y);
                }
            }
        }


        #endregion

        #region InFunction
        if (realR + 2 < rr || realDeep + 2 < deep) { }
        else
        {
            if (realR >= rr)
            {
                realR = rr;
                realDeep = deep;
            }
            if (pr[0] >= 0)
                if (r == pr[0] && count + _Cost[0] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                    }
                    A_Star(_x - 1, _y, _x, _y, count + _Cost[0], deep + 1);
                }

            if (pr[1] >= 0)
                if (r == pr[1] && count + _Cost[1] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                    }
                    A_Star(_x + 1, _y, _x, _y, count + _Cost[1], deep + 1);
                }

            if (pr[2] >= 0)
                if (r == pr[2] && count + _Cost[2] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                    }
                    A_Star(_x, _y - 1, _x, _y, count + _Cost[2], deep + 1);
                }

            if (pr[3] >= 0)
                if (r == pr[3] && count + _Cost[3] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                    }
                    A_Star(_x, _y + 1, _x, _y, count + _Cost[3], deep + 1);
                }
        }
        #endregion

        if (where == deep) comple = true;

        if (comple == true)
        {
            realR = rr;
            realDeep = deep;
            A_T.Add(_Tile.MY_Tile[_x][_y]);
            //_Tile.MY_Tile[_x][_y].HighLight();
        }
    
    }
    public override void ScoutMakeNav(Vector2 _Goal)
    {
        realR = Mathf.Abs(x - (int)_Goal.x) + Mathf.Abs(y - (int)_Goal.y);
        Scout_A_Star((int)_Goal.x, (int)_Goal.y, (int)_Goal.x, (int)_Goal.y, 0, 0);
        cost = 20000;
        where = 20000;
        realR = 10000;
        realDeep = 10000;
        comple = false;
    }
    bool CheckLoot(int x,int y)
    {
        for(int i=0; i<_Tactical.Tag_Player.Count;i++)
        {
            if(Mathf.Abs(_Tactical.Tag_Player[i].x - x) + Mathf.Abs(_Tactical.Tag_Player[i].y -y) <= _Tactical.Tag_Player[i].View.ViewRange / 2) return false;
        }

        return true;
    }

}

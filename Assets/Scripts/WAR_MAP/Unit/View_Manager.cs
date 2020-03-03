using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Manager : MonoBehaviour {

    #region VARIABLE
    Player_Manager Char;
    public int x, y;
    public int ViewRange = 12;
    Tile_Manager _Tile;
    Tile T;
    List<Tile> T_Tile = new List<Tile>();

    public static List<Transform> Enemys = new List<Transform>();
    int I_X = 0, I_Y = 0;
    #endregion

    List<Tile> Behind_Ob;
    Vector2 V2;

    // Use this for initialization
    void Start() {
        Char = GetComponent<Player_Manager>();
        x = Char.x;
        y = Char.y;
        T = transform.parent.GetComponent<Tile>();
        _Tile = T.transform.parent.GetComponent<Tile_Manager>();
        Camera.main.GetComponent<Input_Manager>().Tile_View();
        Behind_Ob = new List<Tile>();
        TestInView();
        
        Board_Manager.m_Board_Manager.Loading++;

    }


    //public void InView()
    //{

    //    switch (Char.dir)
    //    {
    //        case Action_Manager.Direction.left:
    //            I_X = -1;
    //            break;
    //        case Action_Manager.Direction.right:
    //            I_X = 1;
    //            break;
    //        case Action_Manager.Direction.up:
    //            I_Y = 1;
    //            break;
    //        case Action_Manager.Direction.down:
    //            I_Y = -1;
    //            break;

    //    }

    //    Sight(x, y, x, y, 0);
    //    if (T_Tile != null)
    //    {
    //        for (int i = 0; i < T_Tile.Count; i++)
    //        {
    //            T_Tile[i].Behind_Test = false;
    //            T_Tile[i].View_Check = false;
    //        }
    //        T_Tile.Clear();
    //        T_Tile = new List<Tile>();
    //    }

    //    Char._Input.EnemyView();
    //}

/*
    public void InView()
    {
        int count;

        if (Behind_Ob != null) Behind_Ob.Clear();

        Char.Tile_InSighted = null;
        for (int _x = x - (ViewRange + 1); _x <= x + (ViewRange + 1); _x++)
        {
            count = Mathf.Abs(_x - x);



            if (count > ViewRange) continue;
            if (_x < 0 || _x >= _Tile.X) continue;

            if (Char.dir == Unit_Manager.Direction.left && _x > Char.x) continue;
            else if (Char.dir == Unit_Manager.Direction.right && _x < Char.x) continue;

            for (int _y = y - (ViewRange + 1) + count; _y <= y + (ViewRange + 1) - count; _y++)
            {

                if (Mathf.Abs(_y - y) > ViewRange) continue;
                if (_y < 0 || _y >= _Tile.Y) continue;

                if (Char.dir == Unit_Manager.Direction.down && _y > Char.y) continue;
                else if (Char.dir == Unit_Manager.Direction.up && _y < Char.y) continue;


                Tile _T = _Tile.MY_Tile[_x][_y];

                if (_T.View == Tile.View_Kind.Full)
                {
                    if (Char.dir == Unit_Manager.Direction.left)
                    {
                        for (int j = _x - 1; j >= x - (ViewRange + 1); j--)
                        {
                            if (j < 0 || j >= _Tile.Y) break;
                            Behind_Ob.Add(_Tile.MY_Tile[j][_y]);
                        }
                    }
                    else if (Char.dir == Unit_Manager.Direction.right)
                    {
                        for (int j = _x + 1; j <= x + (ViewRange + 1); j++)
                        {
                            if (j >= _Tile.Y || j < 0) break;
                            Behind_Ob.Add(_Tile.MY_Tile[j][_y]);
                        }
                    }
                    else if (Char.dir == Unit_Manager.Direction.down)
                    {
                        for (int j = _y - 1; j >= y - (ViewRange + 1) + count; j--)
                        {
                            if (j < 0 || j >= _Tile.Y) break;
                            Behind_Ob.Add(_Tile.MY_Tile[_x][j]);
                        }
                    }
                    else
                    {
                        for (int j = _y + 1; j <= y + (ViewRange + 1) - count; j++)
                        {
                            if (j >= _Tile.Y || j < 0) break;
                            Behind_Ob.Add(_Tile.MY_Tile[_x][j]);
                        }
                    }

                }

                if (!_T.View_Char.Contains(Char.Char_Num))
                {
                    _T.View_Char.Add(Char.Char_Num);
                }

                if (_T.transform.Find("Enemy(Clone)") != null)
                {
                    if (_T.transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>()._Posture == Unit_Manager.Posture.Prone && Mathf.Abs(x - _x) + Mathf.Abs(y - _y) > ViewRange / 2) { }
                    else
                    {
                        if (!Enemys.Contains(_T.transform.Find("Enemy(Clone)")))
                        {
                            Char.MeetEnemy = true;
                            Enemys.Add(_T.transform.Find("Enemy(Clone)"));
                        }
                        _T.transform.Find("Enemy(Clone)").GetComponent<Enemy_Manager>().Sight();
                    }
                }

                _T.Action = Char;
                _T.InView();


            }

        }


        if (Behind_Ob != null)
        {
            for (int i = 0; i < Behind_Ob.Count; i++)
            {
                if (Behind_Ob[i].X < 0 || Behind_Ob[i].X >= _Tile.X) continue;
                if (Behind_Ob[i].Y < 0 || Behind_Ob[i].Y >= _Tile.Y) continue;

                Tile _T = Behind_Ob[i];

                if (_T.View_Char.Contains(Char.Char_Num)) _T.View_Char.Remove(Char.Char_Num);

                _T.UnView();
            }
        }
        Char.LookUp();
        Char._Input.EnemyView();
    }*/


    public void TestInView()
    {
        for (int i = -2; i < 3; i++)
        {
            switch (Char.dir)
            {
                case Player_Manager.Direction.left:
                    if (y + i < 0 || y + i >= _Tile.Y || _Tile.MY_Tile[x-1][y].View == Tile.View_Kind.Low && Char._Posture == Unit_Manager.Posture.Prone)
                        return;
                    if (i == -2) LeftSight(1, x, y + i, Char.dir,0);
                    else if (i == 2) RightSight(1, x, y + i, Char.dir,0);
                    else CenterSight(1, x, y + i, Char.dir);
                    break;
                case Player_Manager.Direction.right:
                    if (y - i < 0 || y - i >= _Tile.Y || _Tile.MY_Tile[x+1][y].View == Tile.View_Kind.Low && Char._Posture == Unit_Manager.Posture.Prone)
                        return;

                    if (i == -2) LeftSight(1, x, y - i, Char.dir,0);
                    else if (i == 2) RightSight(1, x, y - i, Char.dir,0);
                    else CenterSight(1, x, y - i, Char.dir);
                    break;
                case Player_Manager.Direction.up:
                    if (x + i < 0 || x + i >= _Tile.X || _Tile.MY_Tile[x][y + 1].View == Tile.View_Kind.Low && Char._Posture == Unit_Manager.Posture.Prone)
                        return;
                    if (i == -2) LeftSight(1, x + i, y, Char.dir,0);
                    else if (i == 2) RightSight(1, x + i, y, Char.dir,0);
                    else CenterSight(1, x + i, y, Char.dir);
                    break;
                case Player_Manager.Direction.down:
                    if (x - i < 0 || x - i >= _Tile.X || _Tile.MY_Tile[x][y - 1].View == Tile.View_Kind.Low && Char._Posture == Unit_Manager.Posture.Prone)
                        return;
                    if (i == -2) LeftSight(1, x - i, y, Char.dir,0);
                    else if (i == 2) RightSight(1, x - i, y, Char.dir,0);
                    else CenterSight(1, x - i, y, Char.dir);
                    break;
            }

        }

        if (Char.Unview_Sighted != null)
            Char.Unview_Sighted(Char);
        Char.Unview_Sighted = null;
    }


    //  public void Sight(int _x,int _y,int px,int py,int deep)
    //  {
    //      if (deep >= ViewRange)
    //          return;

    //      Tile _T = _Tile.MY_Tile[_x][_y];

    //      if (!_T.View_Check && !_T.Behind_Test)
    //      {
    //	if (!_T.View_Char.Contains(Char.Char_Num)) _T.View_Char.Add(Char.Char_Num);
    //	_T.Action = Char;
    //	_T.InView();
    //	T_Tile.Add(_T);

    //}

    //      if (_x - 1 >= 0 && _x - 1 != px) Sight(_x - 1, _y, _x, _y, deep + 1);
    //      if (_x + 1 < _Tile.X && _x + 1 != px) Sight(_x + 1, _y, _x, _y, deep + 1);
    //      if (_y - 1 >= 0 && _y - 1 != py) Sight(_x, _y - 1, _x, _y, deep + 1);
    //      if (_y + 1 < _Tile.Y && _y + 1 != py) Sight(_x, _y + 1, _x, _y, deep + 1);

    //      if (_T.View == Tile.View_Kind.Full && !_T.Behind_Test)
    //      {
    //          _T.Behind_Test = true;
    //          T_Tile.Add(_T);

    //          if (Mathf.Abs(x - _x) > Mathf.Abs(y - _y))
    //          {
    //              I_Y = 0;

    //              if (x > _x) I_X = -1;
    //              else I_X = 1;
    //          }
    //          else
    //          {
    //              I_X = 0;

    //              if (y > _y) I_Y = -1;
    //              else I_Y = 1;
    //          }

    //          //Debug.Log(Mathf.Abs(x - _x) + Mathf.Abs(y - _y));
    //          int count = 1;
    //          for (int i = Mathf.Abs(x - _x) + Mathf.Abs(y - _y); i < ViewRange; i++)
    //          {
    //              if (I_X != 0)
    //              {
    //                  if (_x + (I_X * count) >= 0 && _x + (I_X * count) < _Tile.MY_Tile.Count)
    //                  {
    //                      Tile __T = _Tile.MY_Tile[_x + (I_X * count)][_y];
    //				if (__T.View_Char.Contains(Char.Char_Num)) __T.View_Char.Remove(Char.Char_Num);
    //				__T.UnView();
    //                      __T.Behind_Test = true;
    //				__T.Action = null;
    //                      T_Tile.Add(__T);
    //                  }
    //              }
    //              else if (I_Y != 0)
    //              {
    //                  if (_y + (I_Y * count) >= 0 && _y + (I_Y * count) < _Tile.MY_Tile[_x].Count)
    //                  {
    //                      Tile __T = _Tile.MY_Tile[_x][_y + (I_Y * count)];
    //				if (__T.View_Char.Contains(Char.Char_Num)) __T.View_Char.Remove(Char.Char_Num);
    //				__T.UnView();
    //                      __T.Behind_Test = true;
    //				__T.Action = null;
    //				T_Tile.Add(__T);
    //                  }
    //              }
    //              count++;
    //          }
    //      }
    //  }

    private void CenterSight(int depth, int _x, int _y, Player_Manager.Direction dir)
    {
        if (depth > ViewRange)
        {
            return;
        }

        Tile _T = _Tile.MY_Tile[_x][_y];
        if (!_T.View_Char.Contains(Char.Char_Num))
        {
            _T.View_Char.Add(Char.Char_Num);
        }
        _T.Action = Char;
        _T.InView();

        if (_T.transform.Find("Enemy(Clone)") != null)
        {
            if (CheckFindEnemy(_T.transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>(), _x, _y))
            {
                if (!Enemys.Contains(_T.transform.Find("Enemy(Clone)")))
                {
                    Char.MeetEnemy = true;
                    Enemys.Add(_T.transform.Find("Enemy(Clone)"));
                }
                _T.transform.Find("Enemy(Clone)").GetComponent<Enemy_Manager>().Sight();
            }
        }

        if (_T.View == Tile.View_Kind.Full)
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:

                    if (Char.y > _y)
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y - 1, dir, 1);
                    }
                    else if (Char.y < _y)
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y + 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y - 1, dir, 1);
                        if (_x - 1 > 0 && _y + 1 < _Tile.Y) UnviewRightSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.right:

                    if (Char.y > _y)
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x + 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y - 1 > 0) UnviewRightSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    else if (Char.y < _y)
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y + 1 < _Tile.Y) UnviewLeftSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x + 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y - 1 > 0) UnviewRightSight(depth + 1, _x + 1, _y - 1, dir, 1);
                        if (_x + 1 < _Tile.X && _y + 1 > 0) UnviewLeftSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.up:
                    if (Char.x > _x)
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }
                    else if (Char.x < _x)
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x + 1 < _Tile.X) UnviewRightSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y + 1, dir, 1);
                        if (_y + 1 < _Tile.Y && _x + 1 < _Tile.X) UnviewRightSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.down:
                    if (Char.x > _x)
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x - 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y - 1, dir, 1);
                    }
                    else if (Char.x < _x)
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x + 1 < _Tile.X) UnviewLeftSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    else
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x - 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y - 1, dir, 1);
                        if (_y - 1 >= 0 && _x + 1 < _Tile.X) UnviewLeftSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    break;

            }
        }

        switch (dir)
        {
            case Player_Manager.Direction.left:
                if (_x - 1 < 0)
                    return;

                CenterSight(depth + 1, _x - 1, _y, dir);
                break;
            case Player_Manager.Direction.right:
                if (_x + 1 >= _Tile.X) return;

                CenterSight(depth + 1, _x + 1, _y, dir);
                break;
            case Player_Manager.Direction.up:
                if (_y + 1 >= _Tile.Y) return;

                CenterSight(depth + 1, _x, _y + 1, dir);
                break;
            case Player_Manager.Direction.down:
                if (_y - 1 < 0) return;

                CenterSight(depth + 1, _x, _y - 1, dir);
                break;

        }


    }
    private void LeftSight(int depth, int _x, int _y, Player_Manager.Direction dir, int count)
    {
        if (depth > ViewRange)
        {
            return;
        }

        Tile _T = _Tile.MY_Tile[_x][_y];
        if (!_T.View_Char.Contains(Char.Char_Num))
        {
            _T.View_Char.Add(Char.Char_Num);
        }
        _T.Action = Char;
        _T.InView();

        if (_T.transform.Find("Enemy(Clone)") != null)
        {
            if (CheckFindEnemy(_T.transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>(), _x, _y))
            {
                if (!Enemys.Contains(_T.transform.Find("Enemy(Clone)")))
                {
                    Char.MeetEnemy = true;
                    Enemys.Add(_T.transform.Find("Enemy(Clone)"));
                }
                _T.transform.Find("Enemy(Clone)").GetComponent<Enemy_Manager>().Sight();
            }
        }

        if (_T.View == Tile.View_Kind.Full)
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:

                    if (Char.y > _y)
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y - 1, dir, 1);
                    }
                    else if (Char.y < _y)
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y + 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y - 1, dir, 1);
                        if (_x - 1 > 0 && _y + 1 < _Tile.Y) UnviewRightSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.right:

                    if (Char.y > _y)
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x + 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y - 1 > 0) UnviewRightSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    else if (Char.y < _y)
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y + 1 < _Tile.Y) UnviewLeftSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x + 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y - 1 > 0) UnviewRightSight(depth + 1, _x + 1, _y - 1, dir, 1);
                        if (_x + 1 < _Tile.X && _y + 1 > 0) UnviewLeftSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.up:
                    if (Char.x > _x)
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }
                    else if (Char.x < _x)
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x + 1 < _Tile.X) UnviewRightSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y + 1, dir, 1);
                        if (_y + 1 < _Tile.Y && _x + 1 < _Tile.X) UnviewRightSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.down:
                    if (Char.x > _x)
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x - 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y - 1, dir, 1);
                    }
                    else if (Char.x < _x)
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x + 1 < _Tile.X) UnviewLeftSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    else
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x - 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y - 1, dir, 1);
                        if (_y - 1 >= 0 && _x + 1 < _Tile.X) UnviewLeftSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    break;

            }
        }

        if (count == 2 && depth != 1)
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (_y - 1 >= 0)
                        LeftSight(depth, _x, _y - 1, dir, 0);

                    if (_x - 1 < 0) return;
                    CenterSight(depth + 1, _x - 1, _y, dir);
                    break;
                case Player_Manager.Direction.right:
                    if (_y + 1 < _Tile.Y)
                    LeftSight(depth, _x, _y + 1, dir,0);

                    if (_x + 1 >= _Tile.X) return;
                    CenterSight(depth + 1, _x + 1, _y, dir);
                    break;
                case Player_Manager.Direction.up:
                    if (_x - 1 >= 0)
                    LeftSight(depth, _x - 1, _y , dir,0);

                    if (_y + 1 >= _Tile.Y) return;
                    CenterSight(depth + 1, _x, _y + 1, dir);
                    break;
                case Player_Manager.Direction.down:
                    if (_x + 1 < _Tile.X)
                    LeftSight(depth, _x + 1, _y, dir,0);

                     if (_y - 1 < 0) return;
                    CenterSight(depth + 1, _x, _y - 1, dir);
                    break;
            }
        }
        else
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (_x - 1 < 0)
                        return;

                    LeftSight(depth + 1, _x - 1, _y, dir,count + 1);
                    break;
                case Player_Manager.Direction.right:
                    if (_x + 1 >= _Tile.X) return;

                    LeftSight(depth + 1, _x + 1, _y, dir,count + 1);
                    break;
                case Player_Manager.Direction.up:
                    if (_y + 1 >= _Tile.Y) return;

                    LeftSight(depth + 1, _x, _y + 1, dir,count + 1);
                    break;
                case Player_Manager.Direction.down:
                    if (_y - 1 < 0) return;

                    LeftSight(depth + 1, _x, _y - 1, dir,count + 1);
                    break;

            }
        }
   }
   private void RightSight(int depth,int _x,int _y,Player_Manager.Direction dir,int count)
   {
       if(depth > ViewRange)
       {
           return;
       }

        Tile _T = _Tile.MY_Tile[_x][_y];
        if (!_T.View_Char.Contains(Char.Char_Num))
        {
            _T.View_Char.Add(Char.Char_Num);
        }
        _T.Action = Char;
        _T.InView();

        if (_T.transform.Find("Enemy(Clone)") != null)
        {
            if (CheckFindEnemy(_T.transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>(), _x, _y))
            {
                if (!Enemys.Contains(_T.transform.Find("Enemy(Clone)")))
                {
                    Char.MeetEnemy = true;
                    Enemys.Add(_T.transform.Find("Enemy(Clone)"));
                }
                _T.transform.Find("Enemy(Clone)").GetComponent<Enemy_Manager>().Sight();
            }
        }

        if (_T.View == Tile.View_Kind.Full)
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:

                    if (Char.y > _y)
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y - 1, dir, 1);
                    }
                    else if (Char.y < _y)
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y + 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_x - 1 > 0) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x - 1 > 0 && _y - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y - 1, dir, 1);
                        if (_x - 1 > 0 && _y + 1 < _Tile.Y) UnviewRightSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.right:

                    if (Char.y > _y)
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x + 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y - 1 > 0) UnviewRightSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    else if (Char.y < _y)
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x - 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y + 1 < _Tile.Y) UnviewLeftSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_x + 1 < _Tile.X) UnviewCenterSight(depth + 1, _x + 1, _y, dir);
                        if (_x + 1 < _Tile.X && _y - 1 > 0) UnviewRightSight(depth + 1, _x + 1, _y - 1, dir, 1);
                        if (_x + 1 < _Tile.X && _y + 1 > 0) UnviewLeftSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.up:
                    if (Char.x > _x)
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y + 1, dir, 1);
                    }
                    else if (Char.x < _x)
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x + 1 < _Tile.X) UnviewRightSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }
                    else
                    {
                        if (_y + 1 < _Tile.Y) UnviewCenterSight(depth + 1, _x, _y + 1, dir);
                        if (_y + 1 < _Tile.Y && _x - 1 > 0) UnviewLeftSight(depth + 1, _x - 1, _y + 1, dir, 1);
                        if (_y + 1 < _Tile.Y && _x + 1 < _Tile.X) UnviewRightSight(depth + 1, _x + 1, _y + 1, dir, 1);
                    }

                    break;
                case Player_Manager.Direction.down:
                    if (Char.x > _x)
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x - 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y - 1, dir, 1);
                    }
                    else if (Char.x < _x)
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x + 1 < _Tile.X) UnviewLeftSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    else
                    {
                        if (_y - 1 >= 0) UnviewCenterSight(depth + 1, _x, _y - 1, dir);
                        if (_y - 1 >= 0 && _x - 1 > 0) UnviewRightSight(depth + 1, _x - 1, _y - 1, dir, 1);
                        if (_y - 1 >= 0 && _x + 1 < _Tile.X) UnviewLeftSight(depth + 1, _x + 1, _y - 1, dir, 1);
                    }
                    break;

            }
        }

       if(count == 2 && depth != 1)
       {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (_y + 1 < _Tile.Y)
                    RightSight(depth, _x, _y + 1, dir,0);

                    if (_x - 1 < 0) return;
                    CenterSight(depth + 1, _x - 1, _y, dir);
                    break;
                case Player_Manager.Direction.right:
                    if (_y - 1 >= 0)
                    RightSight(depth, _x, _y - 1, dir,0);

                    if (_x + 1 >= _Tile.X) return;
                    CenterSight(depth + 1, _x + 1, _y, dir);
                    break;
                case Player_Manager.Direction.up:
                    if (_x + 1 < _Tile.X)
                    RightSight(depth, _x + 1, _y, dir,0);

                    if (_y + 1 >= _Tile.Y) return;
                    CenterSight(depth + 1, _x, _y + 1, dir);
                    break;
                case Player_Manager.Direction.down:
                    if (_x - 1 >= 0)
                    RightSight(depth, _x - 1, _y, dir,0);

                    if (_y - 1 < 0) return;
                    CenterSight(depth + 1, _x, _y - 1, dir);
                    break;
            }
        }
        else
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (_x - 1 < 0)
                        return;

                    RightSight(depth + 1, _x - 1, _y, dir,count + 1);
                    break;
                case Player_Manager.Direction.right:
                    if (_x + 1 >= _Tile.X) return;

                    RightSight(depth + 1, _x + 1, _y, dir,count + 1);
                    break;
                case Player_Manager.Direction.up:
                    if (_y + 1 >= _Tile.Y) return;

                    RightSight(depth + 1, _x, _y + 1, dir,count + 1);
                    break;
                case Player_Manager.Direction.down:
                    if (_y - 1 < 0) return;

                    RightSight(depth + 1, _x, _y - 1, dir,count + 1);
                    break;

            }
        }
   }


   private void UnviewCenterSight(int depth,int x,int y,Player_Manager.Direction dir)
   {
       if(depth > ViewRange)
       {
           return;
       }

        Tile _T = _Tile.MY_Tile[x][y];
        Char.Unview_Sighted += _T.UnSight;

        switch (dir)
        {
            case Player_Manager.Direction.left:
                if (x - 1 < 0)
                    return;

                UnviewCenterSight(depth + 1, x - 1, y, dir);
                break;
            case Player_Manager.Direction.right:
                if (x + 1 >= _Tile.X) return;

                UnviewCenterSight(depth + 1, x + 1, y, dir);
                break;
            case Player_Manager.Direction.up:
                if (y + 1 >= _Tile.Y) return;

                UnviewCenterSight(depth + 1, x, y + 1, dir);
                break;
            case Player_Manager.Direction.down:
                if (y - 1 < 0) return;

                UnviewCenterSight(depth + 1, x, y - 1, dir);
                break;

        }


   }
   private void UnviewLeftSight(int depth,int x,int y,Player_Manager.Direction dir,int count)
   {
       if(depth > ViewRange)
       {
           return;
       }

        Tile _T = _Tile.MY_Tile[x][y];
        Char.Unview_Sighted += _T.UnSight;

       if(count == 2&& depth != 1)
       {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (y - 1 >= 0)
                    UnviewLeftSight(depth, x, y - 1, dir,0);

                    if (x - 1 < 0) return;
                    UnviewCenterSight(depth + 1, x - 1, y, dir);
                    break;
                case Player_Manager.Direction.right:
                    if (y + 1 < _Tile.Y)
                    UnviewLeftSight(depth, x, y + 1, dir,0);

                    if (x + 1 >= _Tile.X) return;
                    UnviewCenterSight(depth + 1, x + 1, y, dir);
                    break;
                case Player_Manager.Direction.up:
                    if (x - 1 >= 0)
                    UnviewLeftSight(depth, x - 1, y , dir,0);

                    if (y + 1 >= _Tile.Y) return;
                    UnviewCenterSight(depth + 1, x, y + 1, dir);
                    break;
                case Player_Manager.Direction.down:
                    if (x + 1 < _Tile.X)
                    UnviewLeftSight(depth, x + 1, y, dir,0);

                     if (y - 1 < 0) return;
                    UnviewCenterSight(depth + 1, x, y - 1, dir);
                    break;
            }
        }
        else
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (x - 1 < 0)
                        return;

                    UnviewLeftSight(depth + 1, x - 1, y, dir,count + 1);
                    break;
                case Player_Manager.Direction.right:
                    if (x + 1 >= _Tile.X) return;

                    UnviewLeftSight(depth + 1, x + 1, y, dir,count + 1);
                    break;
                case Player_Manager.Direction.up:
                    if (y + 1 >= _Tile.Y) return;

                    UnviewLeftSight(depth + 1, x, y + 1, dir,count + 1);
                    break;
                case Player_Manager.Direction.down:
                    if (y - 1 < 0) return;

                    UnviewLeftSight(depth + 1, x, y - 1, dir,count + 1);
                    break;

            }
        }
   }

   private void UnviewRightSight(int depth,int x,int y,Player_Manager.Direction dir,int count)
   {
       if(depth > ViewRange)
       {
           return;
       }

       Tile _T = _Tile.MY_Tile[x][y];
        Char.Unview_Sighted += _T.UnSight;

       if(count == 2 && depth != 1)
       {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (y + 1 < _Tile.Y)
                    UnviewRightSight(depth, x, y + 1, dir,0);

                    if (x - 1 < 0) return;
                    UnviewCenterSight(depth + 1, x - 1, y, dir);
                    break;
                case Player_Manager.Direction.right:
                    if (y - 1 >= 0)
                    UnviewRightSight(depth, x, y - 1, dir,0);

                    if (x + 1 >= _Tile.X) return;
                    UnviewCenterSight(depth + 1, x + 1, y, dir);
                    break;
                case Player_Manager.Direction.up:
                    if (x + 1 < _Tile.X)
                    UnviewRightSight(depth, x + 1, y, dir,0);

                    if (y + 1 >= _Tile.Y) return;
                    UnviewCenterSight(depth + 1, x, y + 1, dir);
                    break;
                case Player_Manager.Direction.down:
                    if (x - 1 >= 0)
                    UnviewRightSight(depth, x - 1, y, dir,0);

                    if (y - 1 < 0) return;
                    UnviewCenterSight(depth + 1, x, y - 1, dir);
                    break;
            }
        }
        else
        {
            switch (dir)
            {
                case Player_Manager.Direction.left:
                    if (x - 1 < 0)
                        return;

                    UnviewRightSight(depth + 1, x - 1, y, dir,count + 1);
                    break;
                case Player_Manager.Direction.right:
                    if (x + 1 >= _Tile.X) return;

                    UnviewRightSight(depth + 1, x + 1, y, dir,count + 1);
                    break;
                case Player_Manager.Direction.up:
                    if (y + 1 >= _Tile.Y) return;

                    UnviewRightSight(depth + 1, x, y + 1, dir,count + 1);
                    break;
                case Player_Manager.Direction.down:
                    if (y - 1 < 0) return;

                    UnviewRightSight(depth + 1, x, y - 1, dir, count + 1);
                    break;

            }
        }
    }


    public bool CheckFindEnemy(Unit_Manager _Enemy, int _x, int _y)
    {
        if (_Enemy._Posture == Unit_Manager.Posture.Prone && Mathf.Abs(x - _x) + Mathf.Abs(y - _y) > ViewRange / 2) return false;
        else if (_Enemy._Posture == Unit_Manager.Posture.Prone)
        {
            switch (Char.dir)
            {
                case Unit_Manager.Direction.left:
                    if (_Tile.MY_Tile[_x + 1][_y].View == Tile.View_Kind.Half || _Tile.MY_Tile[x + 1][y].View == Tile.View_Kind.Low) return false;
                    break;
                case Unit_Manager.Direction.right:
                    if (_Tile.MY_Tile[_x - 1][_y].View == Tile.View_Kind.Half || _Tile.MY_Tile[x - 1][y].View == Tile.View_Kind.Low) return false;
                    break;
                case Unit_Manager.Direction.up:
                    if (_Tile.MY_Tile[_x][_y - 1].View == Tile.View_Kind.Half || _Tile.MY_Tile[x][y - 1].View == Tile.View_Kind.Low) return false;
                    break;
                case Unit_Manager.Direction.down:
                    if (_Tile.MY_Tile[_x][_y + 1].View == Tile.View_Kind.Half || _Tile.MY_Tile[x][y + 1].View == Tile.View_Kind.Low) return false;
                    break;
            }
        }

        return true;
    }
}

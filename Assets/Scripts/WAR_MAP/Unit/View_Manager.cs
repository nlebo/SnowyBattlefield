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
        InView();
        
        Board_Manager.m_Board_Manager.Loading++;

    }

    // Update is called once per frame
    void Update() {

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
                    if(_T.transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>()._Posture == Unit_Manager.Posture.Prone && Mathf.Abs(x - _x) + Mathf.Abs(y-_y) > ViewRange /2) {}
                    else{
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
   
}

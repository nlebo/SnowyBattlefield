﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player_Manager : Unit_Manager {

	#region VARIABLE

	
    Unit_Manager            Carry_Unit = null;
    List<Tile>              A_T = null;
    Tile DT;
    public Input_Manager    _Input;
    GameObject              NowChose;
	Color                   original;

    int                     realR = 1000;
    int                     cost = 100;
    int                     where = 100;
    int                     realDeep = 1000;

    bool                    comple = false;
    bool                    selected = false;
    bool                    CanMove = false;
    bool                    Second = false;
    bool                    memento = false;
    bool                    CatchFalg = false;
    bool                    Catching = false;
    bool                    WakeUpFlag = false;
    bool                    LastPositioning = false;
    public bool             MeetEnemy = false;

    private float           enter;
    #endregion

    #region UI_VARIABLE
    
    protected Text          D_Action_Point;
    #endregion

    // Use this for initialization
    protected new void Start()
    {
		base.Start();

       
        D_Action_Point = UI_Manager.D_Action_Point;
        _Input = Camera.main.GetComponent<Input_Manager>();
        _Input.EndTurn += EndTurn;

        View = GetComponent<View_Manager>();

		original = GetComponent<SpriteRenderer>().color;
        ChangeMental_Bar();
	}

    // Update is called once per frame
    void Update()
    {
        if (Now_Move) return;

        Vector2 pos = _Input.pos;
        RaycastHit2D hit = _Input.hit;

        #region  Dig
        if (DigHasty)
        {
            Dig(hit, DT);

        }
        else if (Expanding)
        {
            if (Input_Manager.Highlighted != null)
            {
                Input_Manager.Highlighted();
                Input_Manager.Highlighted = null;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Expanding = false;
            }

            else if (hit.transform != null && hit.transform.tag == "Tile")
            {
                Tile _T = hit.transform.GetComponent<Tile>();

                if (Mathf.Abs(_T.X - T.X) + Mathf.Abs(_T.Y - T.Y) == 1)
                {
                    _T.HighLight(1);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Input_Manager.Highlighted();
                        Input_Manager.Highlighted = null;

                        if (_T.Kind == Tile_Manager.Cover_Kind.CanNot || _T.Kind == Tile_Manager.Cover_Kind.Default)
                        {
                            if (T.Kind == Tile_Manager.Cover_Kind.Slit)
                            {
                                DT = _T;
                                DigHasty = true;
                            }
                            else if (T.Kind == Tile_Manager.Cover_Kind.Standing)
                            {
                                _T.Kind = Tile_Manager.Cover_Kind.Slit;
                                _T.View = Tile.View_Kind.Half;
                                //Destroy(_T._Obstacle);
                                _T._Obstacle = Instantiate(_T.Obstacle_Manager.Obstacles[7], _T.transform);
                                _T.direct = Tile.Direct.Any;
                                Now_Action_Point -= 8;
                            }
                        }
                        Expanding = false;
                    }
                }
            }
        }
        #endregion

        #region  Actions

        if(WakeUpFlag || CatchFalg)
        {
            if (Input_Manager.Highlighted != null)
            {
                Input_Manager.Highlighted();
                Input_Manager.Highlighted = null;
            }
            if (Input.GetMouseButtonDown(1))
            {
                WakeUpFlag = false;
                CatchFalg = false;
            }
            else if (hit.transform != null && hit.transform.tag == "Tile")
            {
                Tile _T = hit.transform.GetComponent<Tile>();

                if (Mathf.Abs(_T.X - T.X) <= 1 && Mathf.Abs(_T.Y - T.Y) <= 1)
                {
                    _T.HighLight(1);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Input_Manager.Highlighted();
                        Input_Manager.Highlighted = null;

                        if (_T.transform.Find("Player(Clone)") != null)
                        {
                            if(WakeUpFlag) WakeUp(_T.transform.Find("Player(Clone)").GetComponent<Unit_Manager>());
                            else if(CatchFalg) Catch(_T.transform.Find("Player(Clone)").GetComponent<Unit_Manager>());
                        }
                        WakeUpFlag = false;
                        CatchFalg = false;
                    }
                }
            }
        }

        if(LastPositioning)
        {
            if (Input_Manager.Highlighted != null)
            {
                Input_Manager.Highlighted();
                Input_Manager.Highlighted = null;
            }
            if (hit.transform != null && hit.transform.tag == "Tile")
            {
                Tile _T = hit.transform.GetComponent<Tile>();

                if (Mathf.Abs(_T.X - T.X) + Mathf.Abs(_T.Y - T.Y) == 1)
                {
                    _T.HighLight(1);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Input_Manager.Highlighted();
                        Input_Manager.Highlighted = null;

                        ChangeDir(_T);
                        _Input.OnClick_EndTurn();
                        LastPositioning = false;
                    }
                }
            }
        }
        #endregion

        #region A_Star(For Move)

        if (T.StartPoint && Input.GetMouseButtonDown(0))
        {

            if (hit.transform != null && hit.transform.tag == "Tile")
            {

                Tile _T = hit.transform.GetComponent<Tile>();
                if (!_T.Check) { }

                else if (_Tile.TileMap[_T.X][_T.Y] != Tile_Manager.Cover_Kind.HalfCover && _Tile.TileMap[_T.X][_T.Y] != Tile_Manager.Cover_Kind.Debris)
                {
                    if (_T.transform.childCount > 0)
                    {
                        for (int i = 0; i < _T.transform.childCount; i++)
                        {
                            if (_T.transform.GetChild(i).CompareTag("Player")) return;
                        }
                    }
                    if (A_T != null)
                    {

                        for (int i = 0; i < A_T.Count; i++)
                        {
                            A_T[i].Highlighted = false;
                            A_T[i].UnHighLight();
                        }

                        if (A_T.Count >= 1 && A_T[A_T.Count - 1].X == _T.X && A_T[A_T.Count - 1].Y == _T.Y)
                        {
                            UI_Manager.Delete_AP_Bar.fillAmount = 0;
                            StartCoroutine(Move());
                            Now_Move = true;

                            return;
                        }


                        A_T.Clear();
                        A_T = null;
                    }

                    A_T = new List<Tile>();
                    A_Star(_T.X, _T.Y, _T.X, _T.Y, 0, 0);
                    UI_Manager.Delete_AP_Bar.fillAmount = cost / (float)Now_Action_Point;
                    comple = false;
                    where = 100;
                    cost = 100;
                    realDeep = 1000;
                    realR = 1000;
                }
            }
        }

        #endregion
    
        
    }


    #region  Move functions
    public void A_Star(int _x, int _y, int px, int py, int count, int deep)
    {
        int r = 10000;
        int[] _Cost = { Now_Move_Point, Now_Move_Point, Now_Move_Point, Now_Move_Point };
        int[] pr = { -1, -1, -1, -1 };
        int rr = 10000;

        if (count > Now_Action_Point || deep >= 1000)
        {
            realR = 1000;
            return;
        }

        if (x == _x && y == _y)
        {
            if (cost <= count) return;

            _Tile.MY_Tile[_x][_y].HighLight();
            cost = count;
            realR = rr;
            realDeep = deep;

            for (int i = 0; i < A_T.Count; i++)
            {
                A_T[i].Highlighted = false;
                A_T[i].UnHighLight();
            }

            A_T.Clear();
            A_T = new List<Tile>();
            comple = true;
            return;
        } //LAST_EXIT FUNCTION

        #region MoveInform
        if (_x - 1 >= 0)
        {
            if (_Tile.TileMap[_x - 1][_y] != Tile_Manager.Cover_Kind.HighCover && _x - 1 != px)
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
            if (_Tile.TileMap[_x + 1][_y] != Tile_Manager.Cover_Kind.HighCover && _x + 1 != px)
            {
                if (_Tile.TileMap[_x + 1][_y] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[1]++;

                pr[1] = Mathf.Abs(x - (_x + 1)) + Mathf.Abs(y - _y) + _Cost[1] + count;
                if (r > pr[1])
                {
                    r = pr[1];
                    rr = Mathf.Abs(x - (_x + 1)) + Mathf.Abs(y - _y);
                }
            }
        }

        if (_y - 1 >= 0)
        {
            if (_Tile.TileMap[_x][_y - 1] != Tile_Manager.Cover_Kind.HighCover && _y - 1 != py)
            {
                if (_Tile.TileMap[_x][_y - 1] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[2]++;

                pr[2] = Mathf.Abs(x - _x) + Mathf.Abs(y - (_y - 1)) + _Cost[2] + count;
                if (r > pr[2])
                {
                    r = pr[2];
                    rr = Mathf.Abs(x - _x) + Mathf.Abs(y - (_y - 1));
                }
            }
        }

        if (_y + 1 < _Tile.TileMap[_x].Count)
        {
            if (_Tile.TileMap[_x][_y + 1] != Tile_Manager.Cover_Kind.HighCover && _y + 1 != py)
            {
                if (_Tile.TileMap[_x][_y + 1] == Tile_Manager.Cover_Kind.HalfCover)
                    _Cost[3]++;

                pr[3] = Mathf.Abs(x - _x) + Mathf.Abs(y - (_y + 1)) + _Cost[3] + count;
                if (r > pr[3])
                {
                    r = pr[3];
                    rr = Mathf.Abs(x - _x) + Mathf.Abs(y - (_y + 1));
                }
            }
        }


        #endregion

        #region InFunction
        if (realR + 2 < rr) { }
        else
        {
            if (realR > rr && !Second)
            {
                realR = rr;
                realDeep = deep;
            }
            if (pr[0] >= 0)
                if (r + 1 >= pr[0] && count + _Cost[0] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                        Second = true;
                        realR = rr;
                    }
                    A_Star(_x - 1, _y, _x, _y, count + _Cost[0], deep + 1);

                    if (where == deep)
                    {
                        comple = true;
                        Second = false;
                        where = 100;
                    }

                    if (deep == 1)
                        deep = 1;
                }

            if (pr[1] >= 0)
                if (r + 1 >= pr[1] && count + _Cost[1] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        Second = true;
                        where = deep;
                        realR = rr;
                    }
                    A_Star(_x + 1, _y, _x, _y, count + _Cost[1], deep + 1);

                    if (where == deep)
                    {
                        comple = true;
                        Second = false;
                        where = 100;
                    }

                    if (deep == 1)
                        deep = 1;
                }

            if (pr[2] >= 0)
                if (r + 1 >= pr[2] && count + _Cost[2] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                        Second = true;
                        realR = rr;
                    }
                    A_Star(_x, _y - 1, _x, _y, count + _Cost[2], deep + 1);

                    if (where == deep)
                    {
                        comple = true;
                        Second = false;
                        where = 100;
                    }

                    if (deep == 1)
                        deep = 1;
                }

            if (pr[3] >= 0)
                if (r + 1 >= pr[3] && count + _Cost[3] < cost)
                {
                    if (comple)
                    {
                        comple = false;
                        where = deep;
                        Second = true;
                        realR = rr;
                    }
                    A_Star(_x, _y + 1, _x, _y, count + _Cost[3], deep + 1);

                    if (where == deep)
                    {
                        comple = true;
                        Second = false;
                        where = 100;
                    }

                    if (deep == 1)
                        deep = 1;
                }
        }
        #endregion

        if (comple)
        {
            A_T.Add(_Tile.MY_Tile[_x][_y]);
            _Tile.MY_Tile[_x][_y].HighLight();
        }
    }

    public void Check_Move()
    {
        int Action = Now_Action_Point;
        T.StartPoint = true;
        CanMove = true;

        if (T.Action != null && T.Check)
            if (!Now_Move) Tile._ReturnOriginal();

        T.Action = this;
        T.Check = true;

        if (x - 1 >= 0)
        {
            _Tile.MY_Tile[x - 1][y].Action = this;
            _Tile.MY_Tile[x - 1][y].CanMove(Action, new int[] { x, y });
        }

        if (x + 1 < _Tile.X)
        {
            _Tile.MY_Tile[x + 1][y].Action = this;
            _Tile.MY_Tile[x + 1][y].CanMove(Action, new int[] { x, y });
        }
        if (y - 1 >= 0)
        {
            _Tile.MY_Tile[x][y - 1].Action = this;
            _Tile.MY_Tile[x][y - 1].CanMove(Action, new int[] { x, y });
        }
        if (y + 1 < _Tile.Y)
        {
            _Tile.MY_Tile[x][y + 1].Action = this;
            _Tile.MY_Tile[x][y + 1].CanMove(Action, new int[] { x, y });
        }

    }

    public IEnumerator Move()
    {
        float _Time = 0;


        for (int i = 0; i < A_T.Count; i++)
        {

            Vector2 PPos = Vector2.zero;
            Vector2 CPos = Vector2.zero;
            Tile PT = null;
            Tile CT = null;

            if (Partner != null)
            {
                Partner.transform.SetParent(transform.parent);
                PPos = Partner.transform.localPosition;
                PT = Partner.transform.parent.GetComponent<Tile>();
                Partner.Now_Move = true;
            }
            if(Carry_Unit != null)
            {
                Carry_Unit.transform.SetParent(transform.parent);
                CPos = Partner.transform.localPosition;
                CT = Partner.transform.parent.GetComponent<Tile>();
                Carry_Unit.Now_Move = true;
            }

            transform.SetParent(A_T[i].transform);
            Vector2 Pos = transform.localPosition;


            Now_Action_Point -= Now_Move_Point;

            if (_Tile.TileMap[A_T[i].X][A_T[i].Y] == Tile_Manager.Cover_Kind.HalfCover)
                Now_Action_Point--;

            if (x > A_T[i].X) dir = Direction.left;
            else if (x < A_T[i].X) dir = Direction.right;
            else if (y > A_T[i].Y) dir = Direction.down;
            else if (y < A_T[i].Y) dir = Direction.up;

            while (_Time < 0.1f)
            {
                transform.localPosition = new Vector2(Mathf.Lerp(Pos.x, 0, 10 * _Time), Mathf.Lerp(Pos.y, 0, 10 * _Time));
                if (Partner != null)
                    Partner.transform.localPosition = new Vector2(Mathf.Lerp(PPos.x, 0, 10 * _Time), Mathf.Lerp(PPos.y, 0, 10 * _Time));
                DrawActionPoint();
                yield return null;
                _Time += Time.deltaTime;
            }
            transform.localPosition = new Vector2(0, 0);
            if (Partner != null)
                Partner.transform.localPosition = new Vector2(0, 0);
            _Time = 0;

            //View.UnView(x - A_T[i].X, y - A_T[i].Y);

            x = A_T[i].X;
            y = A_T[i].Y;

            if (Partner != null)
            {
                Partner.x = PT.X;
                Partner.y = PT.Y;
                Partner.View.x = Partner.x;
                Partner.View.y = Partner.y;
                Partner.T = PT;
                Partner.dir = dir;
                Partner.Tile_InSighted(Partner);

                //Partner.Tile_InSighted = null;
                //Partner.Tile_InSighted = Partner.Tile_InSighted2;
                //Partner.Tile_InSighted2 = null;

                Partner.View.TestInView();
            }
            if (Carry_Unit != null)
            {
                Carry_Unit.x = CT.X;
                Carry_Unit.y = CT.Y;
                Carry_Unit.View.x = Carry_Unit.x;
                Carry_Unit.View.y = Carry_Unit.y;
                Carry_Unit.T = CT;
                Carry_Unit.dir = dir;
                Carry_Unit.Tile_InSighted(Carry_Unit);

                //Partner.Tile_InSighted = null;
                //Partner.Tile_InSighted = Partner.Tile_InSighted2;
                //Partner.Tile_InSighted2 = null;

                Carry_Unit.View.TestInView();
            }

            View.x = x;
            View.y = y;
            T = A_T[i];

            if (Tile_InSighted != null)
                Tile_InSighted(this);
            //Tile_InSighted = null;
            //Tile_InSighted = Tile_InSighted2;
            //Tile_InSighted2 = null;

            View.TestInView();
            if (MeetEnemy)
            {
                MeetEnemy = false;
                break;
            }
        }

        A_T.Clear();
        A_T = null;
        Now_Move = false;
        Tile._ReturnOriginal();

        if (Partner != null)
        {
            Partner.Now_Action_Point = Now_Action_Point;
            Partner.Now_Move = false;
            if (_Tile.TileMap[Partner.x][Partner.y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[Partner.x][Partner.y] != Tile_Manager.Cover_Kind.Default)
            {
                Vector2 PPos;
                Tile PT;
                if (_Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x - 1][y].transform);
                    Fire_Dir = Direction.up;
                }
                else if (_Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x + 1][y].transform);
                    Fire_Dir = Direction.down;
                }
                else if (_Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x][y - 1].transform);
                    Fire_Dir = Direction.left;
                }
                else if (_Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x][y + 1].transform);
                    Fire_Dir = Direction.right;
                }

                PPos = Partner.transform.localPosition;
                PT = Partner.transform.parent.GetComponent<Tile>();
                Partner.Now_Move = true;

                while (_Time < 0.5f)
                {
                    Partner.transform.localPosition = new Vector2(Mathf.Lerp(PPos.x, 0, 4 * _Time), Mathf.Lerp(PPos.y, 0, 4 * _Time));
                    yield return null;
                    _Time += Time.deltaTime;
                }

                Partner.x = PT.X;
                Partner.y = PT.Y;
                Partner.View.x = Partner.x;
                Partner.View.y = Partner.y;
                Partner.T = PT;
                Partner.Tile_InSighted(Partner);
                //Partner.Tile_InSighted = null;
                //Partner.Tile_InSighted = Partner.Tile_InSighted2;
                //Partner.Tile_InSighted2 = null;
                Partner.Now_Move = false;

                Partner.View.TestInView();
            }

        }
        if (Carry_Unit != null)
        {
            Carry_Unit.Now_Action_Point = Now_Action_Point;
            Carry_Unit.Now_Move = false;
            if (_Tile.TileMap[Carry_Unit.x][Carry_Unit.y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[Carry_Unit.x][Carry_Unit.y] != Tile_Manager.Cover_Kind.Default)
            {
                Vector2 CPos;
                Tile CT;
                if (_Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.Default)
                {
                    Carry_Unit.transform.SetParent(_Tile.MY_Tile[x - 1][y].transform);
                    //Fire_Dir = Direction.up;
                }
                else if (_Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x + 1][y].transform);
                    //Fire_Dir = Direction.down;
                }
                else if (_Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x][y - 1].transform);
                    //Fire_Dir = Direction.left;
                }
                else if (_Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.Default)
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x][y + 1].transform);
                    //Fire_Dir = Direction.right;
                }

                CPos = Partner.transform.localPosition;
                CT = Partner.transform.parent.GetComponent<Tile>();
                Carry_Unit.Now_Move = true;

                while (_Time < 0.5f)
                {
                    Carry_Unit.transform.localPosition = new Vector2(Mathf.Lerp(CPos.x, 0, 4 * _Time), Mathf.Lerp(CPos.y, 0, 4 * _Time));
                    yield return null;
                    _Time += Time.deltaTime;
                }

                Carry_Unit.x = CT.X;
                
                Carry_Unit.y = CT.Y;
                Carry_Unit.View.x = Carry_Unit.x;
                Carry_Unit.View.y = Carry_Unit.y;
                Carry_Unit.T = CT;
                Carry_Unit.Tile_InSighted(Carry_Unit);
                //Partner.Tile_InSighted = null;
                //Partner.Tile_InSighted = Partner.Tile_InSighted2;
                //Partner.Tile_InSighted2 = null;
                Carry_Unit.Now_Move = false;

                Carry_Unit.View.TestInView();
            }
        }
        yield return null;
    }
    #endregion

    
    #region  Turn functions
    public void InitializeButton()
    {
		#region UIOFF
        UI_Manager.Action_Point.SetActive(false);
        UI_Manager.Posture.gameObject.SetActive(false);
        UI_Manager._Class.gameObject.SetActive(false);
        UI_Manager.Standing_Button.gameObject.SetActive(false);
        UI_Manager.Crouching_Button.gameObject.SetActive(false);
        UI_Manager.Proneing_Button.gameObject.SetActive(false);
        UI_Manager._Clip.gameObject.SetActive(false);
        UI_Manager.EndTurnButton.gameObject.SetActive(false);
        UI_Manager.DigButton.gameObject.SetActive(false);
        UI_Manager.ExpandButton.gameObject.SetActive(false);
        UI_Manager.CarryButton.gameObject.SetActive(false);
        UI_Manager.DropButton.gameObject.SetActive(false);
        UI_Manager.WakeUpButton.gameObject.SetActive(false);
        UI_Manager.MementoCheckButton.gameObject.SetActive(false);
        for (int i = 0; i < UI_Manager.Weapon.Count; i++)
        {
            if (i < Weapons.Count)
                Weapons[i].Unselect();
        }
        for (int i = 0; i < UI_Manager.Weapon_Button.Count; i++)
        {
            UI_Manager.Weapon_Button[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < UI_Manager.Item.Count; i++)
        {
            UI_Manager.Item[i].gameObject.SetActive(false);
            if (i < Items.Count)
                Items[i].Unselect();
        }
        StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Attack_Toast, "Attack"));
        StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Item_Toast,"Item"));
        StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Action_Toast,"Action"));
        StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Posture_Toast,"Posture"));
        StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Weapon_Toast,"Weapon"));
        


        Tile._ReturnOriginal();
        #endregion
        #region UION
        UI_Manager.Posture.gameObject.SetActive(true);
        UI_Manager._Class.gameObject.SetActive(true);
        UI_Manager.MOVE_BUTTON.gameObject.SetActive(true);
        UI_Manager.Posture_BUTTON.gameObject.SetActive(true);
        UI_Manager.ATTACK_Button.gameObject.SetActive(true);
        UI_Manager.Item_Button.gameObject.SetActive(true);
        UI_Manager.EndTurnButton.gameObject.SetActive(true);

        switch (_Posture)
        {
            case Posture.Standing:
                UI_Manager.Posture.text = "Standing";
                break;

            case Posture.Crouching:
                UI_Manager.Posture.text = "Crouching";
                break;

            case Posture.Prone:
                UI_Manager.Posture.text = "Proneing";
                break;
        }

        switch (_Class)
        {
            case Class.Ammuniation_Soldier:
                UI_Manager._Class.text = "Ammuniation_Soldier";
                break;

            case Class.Gunner:
                UI_Manager._Class.text = "Gunner";
                break;

            case Class.Infantry:
                UI_Manager._Class.text = "Infantry";
                break;

            case Class.Second_Gunner:
                UI_Manager._Class.text = "Second_Gunner";
                break;
        }
        #endregion
        DigHasty = false;
        Expanding = false;
        UI_Manager.Delete_AP_Bar.fillAmount = 0;
	}
	public override void TurnOn()
    {
        Board_Manager.m_Board_Manager.TurnFlag = 0;
        base.TurnOn();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y + 110, -10);

        if(Stuned > 0)
        {
            Stuned --;
            EndTurn();
        }
        else
            Select();
        
    }
    public void EndTurn()
    {
        UnSelect();
        Now_Action_Point = Pull_Action_Point;
    }
    public override void Select()
    {
		if (_Class == Class.Second_Gunner)
		{
			Partner.Select();
			return;
		}
        if (_Input.Selected != this || _Input.Selected == null)
        {
            #region BUTTON_SETTING
            if (UI_Manager.MOVE_BUTTON.onClick != null)
            {
                UI_Manager.MOVE_BUTTON.onClick.RemoveAllListeners();
                UI_Manager.Posture_BUTTON.onClick.RemoveAllListeners();
                UI_Manager.Standing_Button.onClick.RemoveAllListeners();
                UI_Manager.Crouching_Button.onClick.RemoveAllListeners();
                UI_Manager.Proneing_Button.onClick.RemoveAllListeners();
                UI_Manager.ATTACK_Button.onClick.RemoveAllListeners();
                UI_Manager.Item_Button.onClick.RemoveAllListeners();
                UI_Manager.EndTurnButton.onClick.RemoveAllListeners();
                UI_Manager.ReloadButton.onClick.RemoveAllListeners();
                UI_Manager.Action_Button.onClick.RemoveAllListeners();
                UI_Manager.DigButton.onClick.RemoveAllListeners();
                UI_Manager.ExpandButton.onClick.RemoveAllListeners();
                UI_Manager.BackPackButton.onClick.RemoveAllListeners();
                UI_Manager.ChangeWeaponButton.onClick.RemoveAllListeners();
                UI_Manager.CarryButton.onClick.RemoveAllListeners();
                UI_Manager.DropButton.onClick.RemoveAllListeners();
                UI_Manager.WakeUpButton.onClick.RemoveAllListeners();
                UI_Manager.MementoCheckButton.onClick.RemoveAllListeners();

                for (int i = 0; i < UI_Manager.Weapon_Button.Count; i++)
                {
                    UI_Manager.Weapon_Button[i].onClick.RemoveAllListeners();
                }

                for(int i =0; i< UI_Manager.Weapon.Count;i++)
                {
                    UI_Manager.Weapon[i].onClick.RemoveAllListeners();
                }

                for (int i = 0; i < UI_Manager.Item.Count; i++)
                {
                    UI_Manager.Item[i].onClick.RemoveAllListeners();
                }
            }
            UI_Manager.MOVE_BUTTON.onClick.AddListener(() => {
                if (DigHasty || Overpowered) return;
                InitializeButton();

                Check_Move();
            });
            UI_Manager.Posture_BUTTON.onClick.AddListener(() =>
            {
                if (DigHasty) return;
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();


                if(UI_Manager.PostureToasting) return;

                UI_Manager.PostureToasting = true;
                StartCoroutine(UI_Manager.BarUpToast(UI_Manager.Posture_Toast,"Posture"));
                UI_Manager.Standing_Button.gameObject.SetActive(true);
                UI_Manager.Crouching_Button.gameObject.SetActive(true);
                UI_Manager.Proneing_Button.gameObject.SetActive(true);
            });
            UI_Manager.Standing_Button.onClick.AddListener(() => { Change_Posture(Posture.Standing);  StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Posture_Toast, "Posture"));});
            UI_Manager.Crouching_Button.onClick.AddListener(() => { Change_Posture(Posture.Crouching); StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Posture_Toast, "Posture"));});
            UI_Manager.Proneing_Button.onClick.AddListener(() => { Change_Posture(Posture.Prone); StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Posture_Toast, "Posture")); });
            UI_Manager.ATTACK_Button.onClick.AddListener(() =>
            {
                if (DigHasty) return;
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();

                if (UI_Manager.AttackToasting) return;

                UI_Manager.AttackToasting = true;
                StartCoroutine(UI_Manager.BarUpToast(UI_Manager.Attack_Toast, "Attack"));
                Weapons[ChooseWeapon].Select();
            });
            UI_Manager.Item_Button.onClick.AddListener(() =>
            {
                if (DigHasty) return;
                if (!Now_Move) Tile._ReturnOriginal();
                InitializeButton();
                if (UI_Manager.ItemToasting) return;

                UI_Manager.ItemToasting = true;
                StartCoroutine(UI_Manager.BarUpToast(UI_Manager.Item_Toast, "Item"));

                for (int i = 0; i < Items.Count; i++)
                {
                    UI_Manager.Item[i].gameObject.SetActive(true);
                    UI_Manager.Item[i].transform.GetChild(0).GetComponent<Text>().text = Items[i].Item_Name;
                }

                UI_Manager.Item[0].onClick.AddListener(() =>
                {
                    Items[0].Use();
                    StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Item_Toast, "Item"));
                });
                if (Items.Count > 1)
                    UI_Manager.Item[1].onClick.AddListener(() =>
                    {
                        Items[1].Use();
                        StartCoroutine(UI_Manager.BarDownToast(UI_Manager.Item_Toast, "Item"));
                    });


            });
            UI_Manager.EndTurnButton.onClick.AddListener(() =>
            {
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();
                LastPositioning = true;
            });
            UI_Manager.ReloadButton.onClick.AddListener(() => { Weapons[ChooseWeapon].Reload(); });
            UI_Manager.Action_Button.onClick.AddListener(() =>
            {
                InitializeButton();


                if (UI_Manager.ActionToasting) return;

                UI_Manager.ActionToasting = true;
                StartCoroutine(UI_Manager.BarUpToast(UI_Manager.Action_Toast, "Action"));

                UI_Manager.DigButton.gameObject.SetActive(true);
                UI_Manager.ExpandButton.gameObject.SetActive(true);
                UI_Manager.CarryButton.gameObject.SetActive(true);
                UI_Manager.DropButton.gameObject.SetActive(true);
                UI_Manager.WakeUpButton.gameObject.SetActive(true);
                UI_Manager.MementoCheckButton.gameObject.SetActive(true);

                UI_Manager.DigButton.onClick.AddListener(() =>
                {
                    if (T.Kind == Tile_Manager.Cover_Kind.Skimisher || T.Kind == Tile_Manager.Cover_Kind.Slit)
                        Dig_Depper();
                    else
                        Dig_hasty_fighting_position();

                    InitializeButton();
                    if (!Now_Move) Tile._ReturnOriginal();
                });

                UI_Manager.ExpandButton.onClick.AddListener(() =>
                {
                    Expand();
                    InitializeButton();
                    if (!Now_Move) Tile._ReturnOriginal();
                });
            });
            UI_Manager.BackPackButton.onClick.AddListener(() =>
            {
                if (DigHasty) return;
                InitializeButton();

                if (!UI_Manager.Inventory.activeInHierarchy)
                {
                    UI_Manager.Inventory.SetActive(true);
                    UI_Manager.Chest.SetActive(true);
                    UI_Manager.Vest.SetActive(true);
                    UI_Manager.Bag.SetActive(true);
                }
                else
                    UI_Manager.Inventory.SetActive(false);
            });
            UI_Manager.ChangeWeaponButton.onClick.AddListener(() =>
            {
                if (DigHasty) return;
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();

                if (UI_Manager.AttackToasting) return;

                UI_Manager.WeaponToasting = true;
                StartCoroutine(UI_Manager.BarUpToast(UI_Manager.Weapon_Toast, "Weapon"));

                UI_Manager.Weapon[0].onClick.AddListener(() =>
                {
                    if (ChooseWeapon == 0 || Now_Action_Point < 1) return;
                    Now_Action_Point--;
                    ChooseWeapon = 0;
                    InitializeButton();
                    if (!Now_Move) Tile._ReturnOriginal();
                });

                UI_Manager.Weapon[0].GetComponentInChildren<Text>().text = Weapons[0].Weapon_Name;
                UI_Manager.Weapon[0].gameObject.SetActive(true);
                if (Weapons.Count > 1)
                {
                    UI_Manager.Weapon[1].onClick.AddListener(() =>
                    {
                        if (ChooseWeapon == 1 || Now_Action_Point < 1) return;

                        Now_Action_Point--;
                        ChooseWeapon = 1;
                        InitializeButton();
                        if (!Now_Move) Tile._ReturnOriginal();
                    });
                    UI_Manager.Weapon[1].GetComponentInChildren<Text>().text = Weapons[1].Weapon_Name;
                    UI_Manager.Weapon[1].gameObject.SetActive(true);
                }
            });
            UI_Manager.CarryButton.onClick.AddListener(() =>
            {
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();
                Catch();
            });
            UI_Manager.DropButton.onClick.AddListener(() =>
            {
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();
                Drop();
            });
            UI_Manager.WakeUpButton.onClick.AddListener(() =>
            {
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();
                WakeUp();
            });
            UI_Manager.MementoCheckButton.onClick.AddListener(() =>
            {
                InitializeButton();
                if (!Now_Move) Tile._ReturnOriginal();
                CheckMemento();
            });
            UI_Manager._Unit = this;

            #endregion
        }
        if (_Input.Selected != null) _Input.Selected.UnSelect();
        

        _Input.Selected = this;


        NowChose = Instantiate(ChooseTile,transform,false);
        GetComponent<SpriteRenderer>().color -= new Color(0.2f, 0.2f, 0.2f,0);
        DrawActionPoint();
        selected = true;


        #region UI_SETTING
        UI_Manager.Name.text = Name;
        UI_Manager.Posture.gameObject.SetActive(true);
		UI_Manager._Class.gameObject.SetActive(true);
        UI_Manager._Clip.text = Weapons[ChooseWeapon].Bullet.ToString() + " / " + Weapons[ChooseWeapon].MaxBullet.ToString();
        UI_Manager.EndTurnButton.gameObject.SetActive(true);
        UI_Manager.PosImageChange(_Posture);
        Weapons[ChooseWeapon].Select();

		switch (_Posture)
        {
            case Posture.Standing:
                UI_Manager.Posture.text = "Standing";
                break;

            case Posture.Crouching:
                UI_Manager.Posture.text = "Crouching";
                break;

            case Posture.Prone:
                UI_Manager.Posture.text = "Proneing";
                break;
        }

		switch (_Class)
		{
			case Class.Ammuniation_Soldier:
				UI_Manager._Class.text = "Ammuniation_Soldier";
				break;

			case Class.Gunner:
				UI_Manager._Class.text = "Gunner";
				break;

			case Class.Infantry:
				UI_Manager._Class.text = "Infantry";
				break;

			case Class.Second_Gunner:
				UI_Manager._Class.text = "Second_Gunner";
				break;
		}
        #endregion

       

	}
    public void UnSelect()
    {
        if (!selected) return;

        if (NowChose != null) Destroy(NowChose);
        InitializeButton();
        NowChose = null;
        CanMove = false;
        UI_Manager.Posture.gameObject.SetActive(false);
		UI_Manager._Class.gameObject.SetActive(false);
		UI_Manager.Standing_Button.gameObject.SetActive(false);
        UI_Manager.Crouching_Button.gameObject.SetActive(false);
        UI_Manager.Proneing_Button.gameObject.SetActive(false);
		UI_Manager._Clip.gameObject.SetActive(false);
        UI_Manager.EndTurnButton.gameObject.SetActive(false);
        UI_Manager.DigButton.gameObject.SetActive(false);
        UI_Manager.ExpandButton.gameObject.SetActive(false);
        if(Weapons[ChooseWeapon] != null)
        Weapons[ChooseWeapon].Unselect();
        else{
            Weapons.RemoveAt(ChooseWeapon);
            ChooseWeapon = 0;
        }
		
		for (int i = 0; i < UI_Manager.Weapon_Button.Count; i++)
		{
			UI_Manager.Weapon_Button[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < UI_Manager.Item.Count; i++)
		{
			UI_Manager.Item[i].gameObject.SetActive(false);
            if (i < Items.Count)
                Items[i].Unselect();
        }

		_Input.Selected = null;
        selected = false;
		GetComponent<SpriteRenderer>().color = original;

		if (!Now_Move) Tile._ReturnOriginal();
    }
    
    #endregion

    
	public override void DrawActionPoint()
	{
		D_Action_Point.text = Now_Action_Point.ToString();
	}
    public override bool Hit(Weapon_Manager _Weapon)
    {
        bool result = base.Hit(_Weapon);
        if (result)
        {
            for (int i = 0; i < Tactical_Head.GetPlayerCount(); i++)
            {
                Unit_Manager Team = Tactical_Head.GetPlayer(i);
                if (Mathf.Abs(x - Team.x) <= 1 && Mathf.Abs(y - Team.y) <= 1)
                    Team.pressure += 30;

                Team.ChangeMental_Bar();

            }
        }
        ChangeMental_Bar();
        return result;
    }
    public override void Death()
    {
        for (int i = 0; i < Tactical_Head.GetPlayerCount(); i++)
        {
            Unit_Manager Team = Tactical_Head.GetPlayer(i);
            if (Mathf.Abs(x - Team.x) <= 1 && Mathf.Abs(y - Team.y) <= 1)
                Team.pressure += 30;

                Team.ChangeMental_Bar();

        }

        for (int i = 0; i < Tactical_Head.GetEnemyCount(); i++)
        {
            Unit_Manager Enemy = Tactical_Head.GetEnemy(i);
            if (Mathf.Abs(x - Enemy.x) <= 3 && Mathf.Abs(y - Enemy.y) <= 3)
                Enemy.pressure -= 10;
        }
        base.Death();
    }
    public void ChangeDir(Tile _T)
    {
        if(_T.X > x) dir = Direction.right;
        else if(_T.X < x) dir = Direction.left;
        else if(_T.Y > y) dir = Direction.up;
        else if(_T.Y < y) dir = Direction.down;

        if (Tile_InSighted != null)
            Tile_InSighted(this);
        View.TestInView();
    }

    #region  Action functions
    public bool Change_Posture(Player_Manager.Posture CPosture)
    {
        if (_Posture == CPosture || Now_Action_Point <= 0) return false;
        switch (CPosture)
        {
            case Posture.Crouching:
                Now_Action_Point--;
                _Posture = CPosture;
                Now_Move_Point = Move_Point;
                UI_Manager.Posture.text = "Crouching";
                GetComponent<SpriteRenderer>().sprite = _Pos[1];
                View.ViewRange = 12;
				PostureBonus = 5;
                break;
            case Posture.Prone:
                Now_Action_Point--;
                _Posture = CPosture;
                Now_Move_Point = Move_Point;
                UI_Manager.Posture.text = "Proneing";
                GetComponent<SpriteRenderer>().sprite = _Pos[2];
                View.ViewRange = 12;
				PostureBonus = 10;
                break;
            case Posture.Standing:
                Now_Action_Point--;
                _Posture = CPosture;
                Now_Move_Point = Move_Point;
                UI_Manager.Posture.text = "Standing";
                GetComponent<SpriteRenderer>().sprite = _Pos[0];
                View.ViewRange = 12;
				PostureBonus = 0;
                break;

        }
        UI_Manager.PosImageChange(_Posture);
        Tile_InSighted(this);

        View.TestInView();

        UI_Manager.Standing_Button.gameObject.SetActive(false);
        UI_Manager.Crouching_Button.gameObject.SetActive(false);
        UI_Manager.Proneing_Button.gameObject.SetActive(false);
        DrawActionPoint();
        return true;
    }
    public void Dig(RaycastHit2D hit, Tile T1 = null)
    {
        if (T1 == null) T1 = T;
        if (Input_Manager.Highlighted != null)
        {
            Input_Manager.Highlighted();
            Input_Manager.Highlighted = null;
        }
        if (Input.GetMouseButtonDown(1))
        {
            DigHasty = false;
            DT = T;
        }

        else if (hit.transform != null && hit.transform.tag == "Tile")
        {
            Tile _T = hit.transform.GetComponent<Tile>();

            if (Mathf.Abs(_T.X - T1.X) + Mathf.Abs(_T.Y - T1.Y) == 1)
            {
                _T.HighLight(1);
                if (Input.GetMouseButtonDown(0))
                {
                    Input_Manager.Highlighted();
                    Input_Manager.Highlighted = null;

                    if (_T.X > x)
                    {
                        T1.direct = Tile.Direct.Right;
                        T1._Obstacle = Instantiate(T1.Obstacle_Manager.Obstacles[3], T1.transform);
                        T1.Kind = Tile_Manager.Cover_Kind.Skimisher;
                    }
                    else if (_T.X < x)
                    {
                        T1.direct = Tile.Direct.Left;
                        T1._Obstacle = Instantiate(T1.Obstacle_Manager.Obstacles[4], T1.transform);
                        T1.Kind = Tile_Manager.Cover_Kind.Skimisher;
                    }
                    else if (_T.Y > y)
                    {
                        T1.direct = Tile.Direct.Up;
                        T1._Obstacle = Instantiate(T1.Obstacle_Manager.Obstacles[5], T1.transform);
                        T1.Kind = Tile_Manager.Cover_Kind.Skimisher;
                    }
                    else if (_T.Y < y)
                    {
                        T1.direct = Tile.Direct.Down;
                        T1._Obstacle = Instantiate(T1.Obstacle_Manager.Obstacles[6], T1.transform);
                        T1.Kind = Tile_Manager.Cover_Kind.Skimisher;
                    }
                    T1.View = Tile.View_Kind.Low;
                    Now_Action_Point -= 7;
                    DigHasty = false;
                    DT = T;
                }
            }
        }
    }
    public override bool Dig_hasty_fighting_position()
    {
        DT = T;
        return base.Dig_hasty_fighting_position();
    }
    public bool CheckMemento()
    {
        if(memento || Now_Action_Point < 5) return false;

        pressure = pressure - 50 < 0 ? 0 : pressure - 50;
        memento = true;
        Now_Action_Point -= 5;
        return true;
    }
    public bool Catch()
    {
        if(Now_Action_Point < 1 || Catching) return false;

        CatchFalg = true;

        return true;
    }
    public bool Catch(Unit_Manager P)
    {
        if(Now_Action_Point < 1 || Catching || !CatchFalg || P.Stuned <= 0) 
        {
            CatchFalg = false;
            return false;
        } 

        Now_Action_Point -= 1;

        CatchFalg = false;
        Catching = true;
        Carry_Unit = P;

        return true;
    }

    public bool Drop()
    {
        if(Now_Action_Point < 1 || !Catching || Carry_Unit == null) return false;

        Now_Action_Point -=1;
        Catching = false;
        Carry_Unit = null;

        return true;
    }

    public bool WakeUp()
    {
        if(Now_Action_Point < 4 || WakeUpFlag) return false;

        WakeUpFlag = true;
        return true;
    }

    public bool WakeUp(Unit_Manager P)
    {
         if(Now_Action_Point < 4 || WakeUpFlag || P.Stuned <= 0)
        {
            WakeUpFlag = false;
            return false;
        }

        Now_Action_Point -=4;
         P.Stuned = 0;
         return true;
    }
    #endregion

}

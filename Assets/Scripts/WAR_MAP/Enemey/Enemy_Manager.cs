using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Enemy_Manager : Unit_Manager
{
	#region VARIABLE
	public static UnityAction           SEE_ALL;
    public static List<Unit_Manager>    SEE_PLAYER;

    public Input_Manager                _Input;
    public Tactical_Head                _Tactical;

    public UnityAction                  UnHighlight;
    public Coroutine                    Move_now;


    List<Unit_Manager>                  PLAYER;
    List<Tile>                          A_T = null;
    List<Vector2Int>                    Behind_Ob;

    int                                 beforeLayer;
    Vector2                             Goal;
    Vector2Int                          V2;


    public int                          cost = 20000;
    public int                          where = 20000;
    public int                          realR = 10000;
    public int                          realDeep = 10000;
    public int                          stay = 0;
    public int                          ViewRange = 12;


    public bool                         comple = false;
    public bool                         seek = false;
    public bool                         Tracking = false;
    public bool                         MovingNow = false;
    public bool                         Watching = false;
    public bool                         Watch_Stop = false;
	#endregion

	// Use this for initialization
	new void Start()
    {
        base.Start();
        A_T = new List<Tile>();
        _Input = Camera.main.GetComponent<Input_Manager>();
        _Input.EnemyView += Sight;
        Goal = new Vector2(Random.Range(
            ((int)_Tile.SpawnPoint[0].x - 6) > 0 ? (int)_Tile.SpawnPoint[0].x - 6 : 0,
            ((int)_Tile.SpawnPoint[0].x + 7) >= _Tile.X ? _Tile.X - 1 : (int)_Tile.SpawnPoint[0].x + 7),
            Random.Range(
                ((int)_Tile.SpawnPoint[0].y - 6) > 0 ? (int)_Tile.SpawnPoint[0].y - 6 : 0,
                ((int)_Tile.SpawnPoint[0].y + 7) >= _Tile.Y ? _Tile.Y - 1 : (int)_Tile.SpawnPoint[0].y + 7));




        PLAYER = new List<Unit_Manager>();

        if (SEE_PLAYER == null) SEE_PLAYER = new List<Unit_Manager>();
        SEE_ALL += Seek;

        Watch_Stop = true;

        Behind_Ob = new List<Vector2Int>();
        Board_Manager.m_Board_Manager.Recive_Tactical += ReciveTactical;
        Health = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Sight()
    {

        if (T.ViewState == Tile.View_State.InSight)
        {
            if (gameObject.layer != 11)
                gameObject.layer = 11;
        }
        else
        {
            if (gameObject.layer != 9)
                gameObject.layer = 9;
        }
    }

    public override void Death()
    {
        for (int i = 0; i < Tactical_Head.GetPlayerCount(); i++)
        {
            Unit_Manager Enemy = Tactical_Head.GetPlayer(i);
            if (Mathf.Abs(x - Enemy.x) <= 3 && Mathf.Abs(y - Enemy.y) <= 3)
                Enemy.pressure -= 10;

        }

        for (int i = 0; i < Tactical_Head.GetEnemyCount(); i++)
        {
            Unit_Manager Team = Tactical_Head.GetEnemy(i);
            if (Mathf.Abs(x - Team.x) <= 1 && Mathf.Abs(y - Team.y) <= 1)
                Team.pressure += 30;
        }

        _Input.EnemyView -= Sight;
        SEE_ALL -= Seek;

        Tactical_Head.DeleteEnemy(this);
        base.Death();
        
    }

    public override void TurnOn()
    {
        Board_Manager.m_Board_Manager.TurnFlag = 1;
        base.TurnOn();

        if (gameObject.layer != beforeLayer)
            beforeLayer = gameObject.layer;

        StartCoroutine(Behavior());

    }

    public void A_Star(int _x, int _y, int px, int py, int count, int deep)
    {
        int r = 10000;
        int[] _Cost = { Now_Move_Point, Now_Move_Point, Now_Move_Point, Now_Move_Point };
        int[] pr = { -1, -1, -1, -1 };
        int rr = 10000;

        if (count >= cost || deep >= 1000)
            return;

        if (x == _x && y == _y)
        {

            // _Tile.MY_Tile[_x][_y].HighLight();
            cost = count;

            //for (int i = 0; i < A_T.Count; i++)
            //{
            //    A_T[i].Highlighted = false;
            //    A_T[i].UnHighLight();
            //}

            A_T.Clear();
            A_T = new List<Tile>();
            comple = true;
            return;
        }

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
                    rr = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y);
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
                    rr = Mathf.Abs(x - (_x - 1)) + Mathf.Abs(y - _y);
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
    public void MakeNav(Vector2 _Goal)
    {
        realR = Mathf.Abs(x - (int)_Goal.x) + Mathf.Abs(y - (int)_Goal.y);
        A_Star((int)_Goal.x, (int)_Goal.y, (int)_Goal.x, (int)_Goal.y, 0, 0);
        cost = 20000;
        where = 20000;
        realR = 10000;
        realDeep = 10000;
        comple = false;
    }
    public List<Tile> GetNav()
    {
        return A_T;
    }
    public List<Unit_Manager> GetMeetPlayer()
    {
        return PLAYER;
    }
    public bool EveryView()
    {
        if (SEE_PLAYER.Count > 0) return true;
        return false;
    }

    public int CloserPlayer()
    {
        int minDistance = 20;
        int _where = -1;

        for (int i = 0; i < PLAYER.Count; i++)
        {
            if (Mathf.Abs(PLAYER[i].x - x) + Mathf.Abs(PLAYER[i].y - y) <= minDistance)
            {
                minDistance = Mathf.Abs(PLAYER[i].x - x) + Mathf.Abs(PLAYER[i].y - y);
                _where = i;
            }
        }

        return _where;
    }

    
    public bool StartMove()
    {
        Move_now = StartCoroutine(Move());
        return true;
    }

    public void ReciveTactical()
    {
        Tactical_Manager.m_Tactical.ReciveTactical(this);
    }

    public virtual bool Attack()
    {
        
        return Weapons[0].Rand(PLAYER[CloserPlayer()]);
    }


    public virtual IEnumerator Behavior()
    {
        UI_MANAGER.m_UI_MANAGER.Notice.text = "적의 턴입니다.";
        UI_MANAGER.m_UI_MANAGER.Notice.gameObject.SetActive(true);

        if (gameObject.layer == 11)
        {
            Camera.main.transform.position = transform.position;
            Camera.main.transform.position -= new Vector3(0, 0, 110);
        }

        SEE_PLAYER.Clear();
        SEE_PLAYER = new List<Unit_Manager>();
        Watching = false;

        SEE_ALL();
        while (!Watching)
        {
            yield return null;
        }
        Watching = false;

        if (!seek)
        {
            _Tactical.Idle();
            while (!_Tactical._Idle) { yield return null; }
            _Tactical._Idle = false;

        }
        else
        {
            _Tactical.Meet();
            while (!_Tactical._Meet) { yield return null; }
            _Tactical._Meet = false;
        }




        yield return new WaitForSeconds(2f);


        _Tactical._Idle = false;
        _Tactical._Meet = false;
        MovingNow = false;
        Watching = false;
        Now_Action_Point = Pull_Action_Point;
        UI_MANAGER.m_UI_MANAGER.Notice.gameObject.SetActive(false);
        Board_Manager.m_Board_Manager.EndTurn();


    }
    public virtual IEnumerator Move()
    {
        float _Time = 0;
        bool change = false;

        int px = 0, py = 0;

        if (stay > 0 && A_T.Count <= 0)
        {
            MovingNow = true;
            A_T.Clear();
            A_T = null;
            StopCoroutine(Move_now);

        }
        yield return null;


        for (int i = 0; 0 < Now_Action_Point; i++)
        {
            Now_Action_Point -= Now_Move_Point;
            if (A_T.Count <= 0) break;

            if (_Tile.TileMap[A_T[0].X][A_T[0].Y] == Tile_Manager.Cover_Kind.HalfCover)
                Now_Action_Point--;

            if (Now_Action_Point < 0) break;



            if (A_T[0].Kind == Tile_Manager.Cover_Kind.HalfCover || A_T[0].Kind == Tile_Manager.Cover_Kind.Debris)
            {

                if (A_T.Count > 1 && (A_T[1].Kind == Tile_Manager.Cover_Kind.HalfCover || A_T[1].Kind == Tile_Manager.Cover_Kind.Debris) && Now_Action_Point >= Now_Move_Point + Now_Move_Point + 1)
                {
                }
                else
                {
                    if (Now_Action_Point < Now_Move_Point + 1)
                    {
                        int o = Random.Range(0, 5);
                        int _count = 0;
                        while (o != 5)
                        {
                            switch (o)
                            {
                                case 0:
                                    if (x - 1 >= 0 &&
                                        (_Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x - 1][y].transform.childCount == 0 && px != -1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x - 1][y];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                                case 1:
                                    if (x + 1 < _Tile.X &&
                                        (_Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x + 1][y].transform.childCount == 0 && px != 1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x + 1][y];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                                case 2:
                                    if (y - 1 >= 0 &&
                                        (_Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x][y - 1].transform.childCount == 0 && py != -1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x][y - 1];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                                case 3:
                                    if (y + 1 < _Tile.Y &&
                                        (_Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x][y + 1].transform.childCount == 0 && py != 1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x][y + 1];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                            }



                            if (o != 5)
                            {
                                o++;
                                if (o == 5) o = 0;
                                _count++;

                                if (_count >= 5)
                                {
                                    A_T[0] = _Tile.MY_Tile[x + px][y + py];
                                    change = true;
                                    break;
                                }
                            }
                            else
                            {
                                for (int j = 0; j < A_T[0].transform.childCount; j++)
                                {
                                    if (A_T[0].transform.GetChild(j).CompareTag("Enemy") || A_T[0].transform.GetChild(j).CompareTag("Player"))
                                    {
                                        o = Random.Range(0, 5);
                                        break;
                                    }
                                }
                            }


                        }

                    }
                }
            }

            else if (Now_Action_Point <= 0 || A_T.Count == 1)
            {
                for (int j = 0; j < A_T[0].transform.childCount; j++)
                {
                    if (A_T[0].transform.GetChild(j).CompareTag("Enemy"))
                    {
                        int o = Random.Range(0, 5);
                        int count = 0;
                        while (o != 5)
                        {
                            switch (o)
                            {
                                case 0:
                                    if (x - 1 >= 0 &&
                                        (_Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x - 1][y].transform.childCount == 0 && px != -1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x - 1][y];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                                case 1:
                                    if (x + 1 < _Tile.X &&
                                        (_Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x + 1][y].transform.childCount == 0 && px != 1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x + 1][y];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                                case 2:
                                    if (y - 1 >= 0 &&
                                        (_Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x][y - 1].transform.childCount == 0 && py != -1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x][y - 1];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                                case 3:
                                    if (y + 1 < _Tile.Y &&
                                        (_Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.CanNot ||
                                        _Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.Default) && _Tile.MY_Tile[x][y + 1].transform.childCount == 0 && py != 1)
                                    {
                                        A_T[0] = _Tile.MY_Tile[x][y + 1];
                                        o = 5;
                                        change = true;
                                    }
                                    break;
                            }

                            if (o != 5)
                            {
                                o++;
                                if (o >= 5)
                                    o = 0;

                                if (count >= 5)
                                {
                                    A_T[0] = _Tile.MY_Tile[x + px][y + py];
                                    change = true;
                                    break;
                                }
                                break;
                            }
                            else
                            {
                                for (int q = 0; q < A_T[0].transform.childCount; q++)
                                {
                                    if (A_T[0].transform.GetChild(q).CompareTag("Enemy") || A_T[0].transform.GetChild(q).CompareTag("Player"))
                                    {
                                        o = Random.Range(0, 5);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            px = x - A_T[0].X;
            py = y - A_T[0].Y;

            Vector2 PPos = Vector2.zero;
            Tile PT = null;
            if (Partner != null)
            {
                Partner.transform.SetParent(transform.parent);
                PPos = Partner.transform.localPosition;
                PT = Partner.transform.parent.GetComponent<Tile>();
                Partner.Now_Move = true;
            }
            transform.SetParent(A_T[0].transform);
            Vector2 Pos = transform.localPosition;




            if (x > A_T[0].X) dir = Direction.right;
            else if (x < A_T[0].X) dir = Direction.left;
            else if (y > A_T[0].Y) dir = Direction.down;
            else if (y < A_T[0].Y) dir = Direction.up;

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

            //View.UnView(x - A_T[0].X, y - A_T[0].Y);

            x = A_T[0].X;
            y = A_T[0].Y;

            if (Partner != null)
            {
                Partner.x = PT.X;
                Partner.y = PT.Y;
                Partner.T = PT;
            }

            T = A_T[0];
            A_T.RemoveAt(0);

            if (change)
            {
                if (A_T.Count > 0)
                    MakeNav(new Vector2(A_T[A_T.Count - 1].X, A_T[A_T.Count - 1].Y));

                change = false;
            }
            Seek();
            if (stay > 0) continue;

            if (seek && Watch_Stop) break;
            seek = false;
            
            yield return null;
        }

        Now_Move = false;

        if (Partner != null)
        {
            Partner.Now_Action_Point = Now_Action_Point;
            Partner.Now_Move = false;
            if (_Tile.TileMap[Partner.x][Partner.y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[Partner.x][Partner.y] != Tile_Manager.Cover_Kind.Default)
            {
                Vector2 PPos;
                Tile PT;
                if (x-1 >= 0 && (_Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.Default))
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x - 1][y].transform);
                    Fire_Dir = Direction.up;
                }
                else if (x+1 < _Tile.X && (_Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.Default))
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x + 1][y].transform);
                    Fire_Dir = Direction.down;
                }
                else if (y - 1 >= 0 && (_Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.Default))
                {
                    Partner.transform.SetParent(_Tile.MY_Tile[x][y - 1].transform);
                    Fire_Dir = Direction.left;
                }
                else if (y + 1 < _Tile.Y && (_Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.Default))
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
                Partner.T = PT;
                Partner.Now_Move = false;
            }

        }
        MovingNow = true;

        StopCoroutine(Move_now);
        yield return null;
    }
    public void Seek()
    {
        seek = false;
        PLAYER.Clear();
        PLAYER = new List<Unit_Manager>();
        int count = -1;
        bool Continue = false;

        if (Behind_Ob != null) Behind_Ob.Clear();


        for (int _x = x - (ViewRange + 1); _x <= x + ViewRange + 1; _x++)
        {
            count = Mathf.Abs(_x - x);

            if (count > ViewRange) continue;
            if (_x < 0 || _x >= _Tile.X) continue;

            for (int _y = y - (ViewRange + 1) + count; _y <= y + (ViewRange + 1) - count; _y++)
            {

                if (Mathf.Abs(_y - y) > ViewRange) continue;
                if (_y < 0 || _y >= _Tile.Y) continue;

                for (int i = 0; i < Behind_Ob.Count; i++)
                {
                    if (_x == Behind_Ob[i].x && _y == Behind_Ob[i].y)
                    {
                        Continue = true;
                        break;
                    }
                }

                if (Continue) continue;

                Tile _T = _Tile.MY_Tile[_x][_y];
                
                if (_T.transform.childCount > 0)
                {
                    for (int i = 0; i < _T.transform.childCount; i++)
                    {
                        if (_T.transform.GetChild(i).CompareTag("Player"))
                        {

                            _T.transform.GetChild(i).GetComponent<Unit_Manager>().View.InView();

                            if(_T.transform.GetChild(i).GetComponent<Unit_Manager>()._Posture == Posture.Prone && Mathf.Abs(x - _x) + Mathf.Abs(y - _y) > ViewRange / 2) break;  
                            
                            PLAYER.Add(_T.transform.GetChild(i).GetComponent<Unit_Manager>());
                            if (!SEE_PLAYER.Contains(PLAYER[PLAYER.Count - 1])) SEE_PLAYER.Add(PLAYER[PLAYER.Count - 1]);
                            seek = true;
                            

                           
                        }

                    }
                }

            }
        }
        Sight();
        Watching = true;
    }

    public bool FindObstacle()
    {
        return true;
    }

    public void SharingVison(int minDistance = 20)
    {

        int _where = -1;

        for (int i = 0; i < SEE_PLAYER.Count; i++)
        {
            if (Mathf.Abs(SEE_PLAYER[i].x - x) + Mathf.Abs(SEE_PLAYER[i].y - y) <= minDistance)
            {
                minDistance = Mathf.Abs(SEE_PLAYER[i].x - x) + Mathf.Abs(SEE_PLAYER[i].y - y);
                _where = i;
            }
        }

        if (_where != -1)
        {
            MakeNav(new Vector2(SEE_PLAYER[_where].x, SEE_PLAYER[_where].y));
            Tracking = true;
        }
    }


    public virtual bool CoverCheck(Unit_Manager _Unit,int behind = 0)
    {
        int X = 0, Y = 0;
        List<Tile> _Cover = new List<Tile>();
        if (Mathf.Abs(_Unit.x - x) >= Mathf.Abs(_Unit.y - y))
        {
            if (x - _Unit.x > 0)
            {
                if (_Tile.TileMap[x - 1][y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[x - 1][y] != Tile_Manager.Cover_Kind.Default)
                    return true;

                if(behind != 1)
                    X = 1; 
                
            }
            else
            {
                if (_Tile.TileMap[x + 1][y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[x + 1][y] != Tile_Manager.Cover_Kind.Default)
                    return true;
                X = -1;
            }
        }
        else
        {
            if (y - _Unit.y > 0)
            {
                if (_Tile.TileMap[x][y - 1] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[x][y - 1] != Tile_Manager.Cover_Kind.Default)
                    return true;

                Y = 1;
            }
            else
            {
                if (_Tile.TileMap[x][y - 1] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[x][y - 1] != Tile_Manager.Cover_Kind.Default)
                    return true;

                Y = -1;
            }
        }
        int count = -1;
        for (int _x = x - Pull_Action_Point; _x <= x + Pull_Action_Point; _x++)
        {
            count = Mathf.Abs(_x - x);
            if (_x < 0 || _x >= _Tile.X) continue;

            for (int _y = y - Pull_Action_Point + count; _y <= y + Pull_Action_Point - count; _y++)
            {
                if (_y < 0 || _y >= _Tile.Y) continue;

                if (_Tile.TileMap[_x][_y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[_x][_y] != Tile_Manager.Cover_Kind.Default)
                {
                    _Cover.Add(_Tile.MY_Tile[_x][_y]);
                }
            }
        }

        int[] _where = new int[2] { -1, -1 };
        int[] length = new int[2] { 10, 10 };
        for (int i = 0; i < _Cover.Count; i++)
        {

            int w = 0; // 0 = behind , 1 = front
            if (X > 0 && _Cover[i].X < x) w = 1;
            else if (X < 0 && _Cover[i].X > x) w = 1;
            else if (Y > 0 && _Cover[i].Y < y) w = 1;
            else if (Y < 0 && _Cover[i].Y > y) w = 1;

            if (Mathf.Abs(_Cover[i].X - x) + Mathf.Abs(_Cover[i].Y - y) <= length[w])
            {
                if (_Tile.MY_Tile[_Cover[i].X + X][_Cover[i].Y + Y].transform.Find("Enemy(Clone)") != null ||
                    (_Tile.TileMap[_Cover[i].X + X][_Cover[i].Y + Y] != Tile_Manager.Cover_Kind.CanNot && _Tile.TileMap[_Cover[i].X + X][_Cover[i].Y + Y] != Tile_Manager.Cover_Kind.Default))
                {
                    _Cover.RemoveAt(i);
                    i--;
                }
                else
                {
                    length[w] = Mathf.Abs(_Cover[i].X - x) + Mathf.Abs(_Cover[i].Y - y);
                    _where[w] = i;
                }
            }
        }

        if(behind == 0){
            if (_where[0] != -1)
            {
                realR = Mathf.Abs(x - _Cover[_where[0]].X) + Mathf.Abs(y - _Cover[_where[0]].Y);
                A_Star(_Cover[_where[0]].X + X, _Cover[_where[0]].Y + Y, _Cover[_where[0]].X + X, _Cover[_where[0]].Y + Y, 0, 0);
                cost = 20000;
                where = 20000;
                realR = 10000;
                realDeep = 10000;
                comple = false;

                return false;
            }
            else if (_where[1] != -1)
            {
                realR = Mathf.Abs(x - _Cover[_where[1]].X) + Mathf.Abs(y - _Cover[_where[1]].Y);
                A_Star(_Cover[_where[1]].X + X, _Cover[_where[1]].Y + Y, _Cover[_where[1]].X + X, _Cover[_where[1]].Y + Y, 0, 0);
                cost = 20000;
                where = 20000;
                realR = 10000;
                realDeep = 10000;
                comple = false;

                return false;
            }
        }
        else
        {
            if (_where[1] != -1)
            {
                realR = Mathf.Abs(x - _Cover[_where[0]].X) + Mathf.Abs(y - _Cover[_where[0]].Y);
                A_Star(_Cover[_where[0]].X + X, _Cover[_where[0]].Y + Y, _Cover[_where[0]].X + X, _Cover[_where[0]].Y + Y, 0, 0);
                cost = 20000;
                where = 20000;
                realR = 10000;
                realDeep = 10000;
                comple = false;

                return false;
            }
            else if (_where[0] != -1)
            {
                realR = Mathf.Abs(x - _Cover[_where[1]].X) + Mathf.Abs(y - _Cover[_where[1]].Y);
                A_Star(_Cover[_where[1]].X + X, _Cover[_where[1]].Y + Y, _Cover[_where[1]].X + X, _Cover[_where[1]].Y + Y, 0, 0);
                cost = 20000;
                where = 20000;
                realR = 10000;
                realDeep = 10000;
                comple = false;

                return false;
            }
        }




        return true;
    }

    public bool ChangePos(Posture CPosture)
    {
        if (_Posture == CPosture || Now_Action_Point <= 0)
            return false;

        switch (CPosture)
        {
            case Posture.Crouching:
                Now_Action_Point--;
                _Posture = CPosture;
                Now_Move_Point = Move_Point;
                GetComponent<SpriteRenderer>().sprite = _Pos[1];
                PostureBonus = 5;
                break;
            case Posture.Prone:
                Now_Action_Point--;
                _Posture = CPosture;
                Now_Move_Point = Move_Point;
                GetComponent<SpriteRenderer>().sprite = _Pos[2];
                PostureBonus = 10;
                break;
            case Posture.Standing:
                Now_Action_Point--;
                _Posture = CPosture;
                Now_Move_Point = Move_Point;
                GetComponent<SpriteRenderer>().sprite = _Pos[0];
                PostureBonus = 0;
                break;

        }
        return true;
    }

}

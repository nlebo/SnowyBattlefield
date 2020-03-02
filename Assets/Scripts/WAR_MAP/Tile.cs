using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour{

    #region VARIABLE
    public enum View_Kind { Full=0,Half,Low,None};
    public View_Kind View = View_Kind.None;

    public enum Direct { Any = 0,Up,Down,Left,Right};
    public Direct direct = Direct.Any;

    public enum View_State { UnKnown=0,InSight,OutSight};
    public View_State ViewState = View_State.UnKnown;

    public GameObject _Obstacle;
    public Obstacle _obstacle;
    public Obstacle_Manager Obstacle_Manager;

	public List<int> View_Char;
    public int X, Y;
    public int RealCost;
    public bool Check;
    public bool StartPoint;
    public bool Highlighted;
    public bool View_Check;
    public bool Behind_Test = false;
    public Tile_Manager.Cover_Kind Kind;
    public Player_Manager Action;

    public static UnityAction _ReturnOriginal;

    int prevLayer;
    bool CompleteStart = false;
    public Tile_Manager _Tile;
    Color orginal_color;
    Color Prev_Highlighted;
    #endregion

    public void Start()
    {
        _Tile = transform.parent.GetComponent<Tile_Manager>();
        Obstacle_Manager = transform.parent.GetComponent<Obstacle_Manager>();
        Check = false;
        RealCost = -1;
        Highlighted = false;
        orginal_color = GetComponent<SpriteRenderer>().color;
        Prev_Highlighted = GetComponent<SpriteRenderer>().color;
        View_Check = false;
        Camera.main.GetComponent<Input_Manager>().Tile_View += LateStart;
		View_Char = new List<int>();
        Action = null;
        Board_Manager.m_Board_Manager.Loading++;
        _ReturnOriginal += ReturnOrginal;

    }

    public void LateStart()
    {
        

        Kind = _Tile.TileMap[X][Y];
        if (_Obstacle != null) return;

        switch(Kind)
        {
            case Tile_Manager.Cover_Kind.CanNot:
            case Tile_Manager.Cover_Kind.Default:
            case Tile_Manager.Cover_Kind.Others:
                View = View_Kind.None;
                break;
            case Tile_Manager.Cover_Kind.Debris:
                View = View_Kind.Low;
                _Obstacle = Instantiate(Obstacle_Manager.Obstacles[0], transform);
                break;
            case Tile_Manager.Cover_Kind.HalfCover:
                _Obstacle = Instantiate(Obstacle_Manager.Obstacles[1], transform);
                View = View_Kind.Half;
                break;
            case Tile_Manager.Cover_Kind.HighCover:
                _Obstacle = Instantiate(Obstacle_Manager.Obstacles[2], transform);
                View = View_Kind.Full;
                break;
        }

        if (_Obstacle != null)
        {
            _obstacle = _Obstacle.GetComponent<Obstacle>();
            _obstacle.Change();
        }

        //gameObject.layer = 9;
        CompleteStart = true;
    }


    public void Init(int x,int y)
    {
        X = x;
        Y = y;
    }
    public bool CanMove(int ActionPoint,int[] Prev)
    {
        Tile_Manager.Cover_Kind Kind;
        Kind = _Tile.TileMap[X][Y];

        if (Kind == Tile_Manager.Cover_Kind.HighCover || StartPoint)
            return false;


        if (Kind == Tile_Manager.Cover_Kind.HalfCover)
            ActionPoint--;


		if (ActionPoint - Action.Now_Move_Point < 0) { 
            if(!Check) 
                Action = null; 
            
            return false; 
        }

        ActionPoint -= Action.Now_Move_Point;
        if (!Check)
        {
            GetComponent<SpriteRenderer>().color -= new Color(0.3f, 0.3f, 0.3f, -1);
            Prev_Highlighted = GetComponent<SpriteRenderer>().color;

            if (_obstacle != null)
                _obstacle.Change();
        }

        Check = true;
		if (X - 1 >= 0 && X - 1 != Prev[0])
		{
			_Tile.MY_Tile[X - 1][Y].Action = Action;
			_Tile.MY_Tile[X - 1][Y].CanMove(ActionPoint, new int[] { X, Y },0);
		}
		if (X + 1 < _Tile.X && X + 1 != Prev[0])
		{
			_Tile.MY_Tile[X + 1][Y].Action = Action;
			_Tile.MY_Tile[X + 1][Y].CanMove(ActionPoint, new int[] { X, Y },0);
		}
		if (Y - 1 >= 0 && Y - 1 != Prev[1])
		{
			_Tile.MY_Tile[X][Y - 1].Action = Action;
			_Tile.MY_Tile[X][Y - 1].CanMove(ActionPoint, new int[] { X, Y },0);
		}
		if (Y + 1 < _Tile.Y&& Y + 1 != Prev[1])
		{
			_Tile.MY_Tile[X][Y + 1].Action = Action;
			_Tile.MY_Tile[X][Y + 1].CanMove(ActionPoint, new int[] { X, Y },0);
		}

        return true;
    }

    public bool CanMove(int ActionPoint, int[] Prev,int cost)
    {
        Tile_Manager.Cover_Kind Kind;
        Kind = _Tile.TileMap[X][Y];

        if (Kind == Tile_Manager.Cover_Kind.HighCover || StartPoint)
            return false;

        if (RealCost != -1 && RealCost <= cost) return false;

        if (Kind == Tile_Manager.Cover_Kind.HalfCover) {
            cost++;
            ActionPoint--;
        }
        if (ActionPoint - Action.Now_Move_Point < 0)
        {
            if (!Check)
                Action = null;

            return false;
        }

        ActionPoint -= Action.Now_Move_Point;
        cost += Action.Now_Move_Point;

        RealCost = cost;
        if (!Check)
        {
            GetComponent<SpriteRenderer>().color -= new Color(0.3f, 0.3f, 0.3f, -1);
            Prev_Highlighted = GetComponent<SpriteRenderer>().color;

            if (_obstacle != null)
                _obstacle.Change();
        }

        Check = true;
        if (X - 1 >= 0 && X - 1 != Prev[0])
        {
            _Tile.MY_Tile[X - 1][Y].Action = Action;
            _Tile.MY_Tile[X - 1][Y].CanMove(ActionPoint, new int[] { X, Y },cost);
        }
        if (X + 1 < _Tile.X && X + 1 != Prev[0])
        {
            _Tile.MY_Tile[X + 1][Y].Action = Action;
            _Tile.MY_Tile[X + 1][Y].CanMove(ActionPoint, new int[] { X, Y }, cost);
        }
        if (Y - 1 >= 0 && Y - 1 != Prev[1])
        {
            _Tile.MY_Tile[X][Y - 1].Action = Action;
            _Tile.MY_Tile[X][Y - 1].CanMove(ActionPoint, new int[] { X, Y },cost);
        }
        if (Y + 1 < _Tile.Y && Y + 1 != Prev[1])
        {
            _Tile.MY_Tile[X][Y + 1].Action = Action;
            _Tile.MY_Tile[X][Y + 1].CanMove(ActionPoint, new int[] { X, Y }, cost);
        }

        return true;
    }
    public void HighLight(int a =0)
    {
        prevLayer = gameObject.layer;
        if (gameObject.layer != 8)
            gameObject.layer = 8;
        Prev_Highlighted = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = Color.red + new Color(0, 0.4f, 0);
        Highlighted = true;
        if(_obstacle != null)
        _obstacle.Change();

        if (a == 1)
        {
            Input_Manager.Highlighted += UnHighLight;
        }
    }
    public void UnHighLight()
    {

        GetComponent<SpriteRenderer>().color = Prev_Highlighted;
        Highlighted = false;
        if (gameObject.layer != prevLayer)
            gameObject.layer = prevLayer;
        if (_obstacle != null)
            _obstacle.Change();


    }
    public void UnView()
    {
		if (View_Char.Count > 0) return;

        View_Check = false;
        ViewState = View_State.OutSight;
        GetComponent<SpriteRenderer>().color = new Color(1 -orginal_color.r, 1 -orginal_color.g, 1 - orginal_color.b, 1f);
        if (_obstacle != null)
            _obstacle.Change();
    }
    public void InView()
    {
		
        ViewState = View_State.InSight;
        View_Check = true;

        if (gameObject.layer != 8)
            gameObject.layer = 8;

        GetComponent<SpriteRenderer>().color = orginal_color;
        if (_obstacle != null)
            _obstacle.Change();
        Action.Tile_InSighted += UnSight;
    }
    public void UnSight(Unit_Manager _Action)
    {
		//Action = GameObject.Find("Player(Clone)").GetComponent<Action_Manager>();
		//Debug.Log(Action.x + "/" + Action.y + " - " + X +"/" + Y + " = " + (Mathf.Abs(Action.x - X) + Mathf.Abs(Action.y - Y)));
		if (Mathf.Abs(_Action.x - X) + Mathf.Abs(_Action.y - Y) >= _Action.View.ViewRange)
		{
            
			if (View_Char.Contains(_Action.Char_Num)) View_Char.Remove(_Action.Char_Num);
			UnView();
		}

        if ((_Action.dir == Unit_Manager.Direction.left && X > _Action.x) ||
            (_Action.dir == Unit_Manager.Direction.right && X < _Action.x) ||
            (_Action.dir == Unit_Manager.Direction.down && Y > _Action.y) ||
            (_Action.dir == Unit_Manager.Direction.up && Y < _Action.y))
        {
            if (View_Char.Contains(_Action.Char_Num)) View_Char.Remove(_Action.Char_Num);
            UnView();
        }

        //Action = null;
    }
	public void DownGrade()
	{
		switch (Kind)
		{
			case Tile_Manager.Cover_Kind.Debris:
				View = View_Kind.None;
				Kind = Tile_Manager.Cover_Kind.Default;
				_Tile.TileMap[X][Y] = Tile_Manager.Cover_Kind.Default;
                Destroy(_Obstacle);
				break;
			case Tile_Manager.Cover_Kind.HalfCover:
				View = View_Kind.Low;
				Kind = Tile_Manager.Cover_Kind.Debris;
				_Tile.TileMap[X][Y] = Tile_Manager.Cover_Kind.Debris;
                Destroy(_Obstacle);
                _Obstacle = Instantiate(Obstacle_Manager.Obstacles[0],transform);
                Prev_Highlighted = orginal_color;
				break;
			case Tile_Manager.Cover_Kind.HighCover:
				View = View_Kind.Half;
				Kind = Tile_Manager.Cover_Kind.HalfCover;
				_Tile.TileMap[X][Y] = Tile_Manager.Cover_Kind.HalfCover;
                Destroy(_Obstacle);
                _Obstacle = Instantiate(Obstacle_Manager.Obstacles[1], transform);
				break;
			default:
				break;
		}

        if (_Obstacle != null) _obstacle = _Obstacle.GetComponent<Obstacle>();
        else _obstacle = null;
    }

    public void ReturnOrginal()
    {
        StartPoint = false;
        RealCost = -1;
        Check = false;
        if (ViewState == View_State.InSight)
            GetComponent<SpriteRenderer>().color = orginal_color;
        else if (ViewState == View_State.OutSight)
        {
            GetComponent<SpriteRenderer>().color = new Color(1 - orginal_color.r, 1 - orginal_color.g, 1 - orginal_color.b, 1f);
        }
        Prev_Highlighted = orginal_color;
        if (_obstacle != null)
            _obstacle.Change();
        Action = null;
    }
}

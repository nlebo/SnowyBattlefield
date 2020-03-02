using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Unit_Manager : MonoBehaviour {


	#region VARIABLE
	protected Tile_Manager				_Tile;
	protected UI_MANAGER				UI_Manager;
	public Tile							T;
	public View_Manager					View;
	public Inventory_Manager			Inventory;
	public Unit_Manager					Partner;

	public UnityAction<Unit_Manager>	Tile_InSighted;
	public UnityAction<Unit_Manager>	Tile_InSighted2;
	


	public enum Posture					{ Standing = 0, Crouching, Prone };
	public enum Direction				{ left = 0, right = 1, up = 2, down = 3 };
	public enum Class					{ Infantry = 0, Gunner, Second_Gunner, Ammuniation_Soldier }

	public Posture						_Posture = Posture.Standing;
	public Direction					dir = Direction.left;
	public Direction					Fire_Dir = Direction.left;
	public Class						_Class;


	public int							Pull_Action_Point = 10;
	public int							Now_Action_Point;
	public int							Now_Move_Point;
	public int							Move_Point = 1;
	public int							ChoosWeapon = 0;
	public int							Set = 0;
	public int							x, y;
	public int							Char_Num;
	public int							Painkiller_Count = 0;
	public int							PostureBonus = 0;
	public int							CoverBonus = 0;

	#region Stat
	public float						aim = 40;  //전투시 명중률 % , 이벤트시 x * 3/2 (내림) 
	public int							DynamicVisualAcuity; // 동체시력
	public int							Health = 100; // 체력
	public int							pressure = 15; // 압박

	public float						HeadShotPercent = 10; // 헤드샷확률
	public float						potential = 1; // 잠재력
	public int							will = 30; // 의지

	#endregion

	public bool							Now_Move = false;
	public bool							DigHasty = false;
	public bool							Expanding = false;

	public GameObject					ChooseTile;
	public List<Weapon_Manager>			Weapons;
	public List<Item_Manager>			Items;

	public string						Name;

	public Sprite[] _Pos;

	#endregion

	protected void Start()
	{
		DynamicVisualAcuity = Random.Range(13, 20);
		Now_Action_Point = Pull_Action_Point;
		Now_Move_Point = Move_Point;

		UI_Manager = GameObject.Find("Canvas").GetComponent<UI_MANAGER>();
		Inventory = GameObject.Find("Inventory").GetComponent<Inventory_Manager>();
		T = transform.parent.GetComponent<Tile>();
		_Tile = transform.parent.parent.GetComponent<Tile_Manager>();
		x = T.X;
		y = T.Y;

		Init();
		Board_Manager.m_Board_Manager.AddUnit(this);

		Board_Manager.m_Board_Manager.Loading++;
		Name = UI_Manager.EUFirstName[Random.Range(0, UI_Manager.EUFirstName.Length)] + " " + UI_Manager.EULastName[Random.Range(0, UI_Manager.EULastName.Length)];

	}

	public void Init()
	{

		Weapons.Clear();
		Items.Clear();
		switch (_Class)
		{
			case Class.Infantry:
				Weapons.Add(Inventory.SVD);
				Weapons.Add(Inventory.RPG);
				Items.Add(Inventory.PainKiller);
				Items.Add(Inventory.Granade);
				break;
			case Class.Gunner:
				if (Inventory.Squad == null) Inventory.Squad += SQUAD;
				else Inventory.Squad(this);
				Weapons.Add(Inventory.PK);
				Weapons.Add(Inventory.DSHK);
				Items.Add(Inventory.PainKiller);
				Items.Add(Inventory.Granade);
				break;
			case Class.Second_Gunner:
				if (Inventory.Squad == null) Inventory.Squad += SQUAD;
				else Inventory.Squad(this);
				break;
			case Class.Ammuniation_Soldier:
				Weapons.Add(Inventory.SUAK_47);
				Weapons.Add(Inventory.Torkarev);
				break;
		}

		for (int i = 0; i < Weapons.Count; i++)
		{
			Weapons[i] = Instantiate(Weapons[i].gameObject, transform).GetComponent<Weapon_Manager>();
			Weapons[i].Unit = this;
			if (i < Items.Count)
			{
				Items[i] = Instantiate(Items[i].gameObject, transform).GetComponent<Item_Manager>();
				Items[i].Unit = this;
			}
		}
	}
	public void SQUAD(Unit_Manager _Partner)
	{
		Partner = _Partner;
		_Partner.Partner = this;
		Inventory.Squad = null;
		bool check = false;
		int r = Random.Range(0, 3);

		while (!check)
		{
			switch (r)
			{
				case 0:
					if (x - 1 >= 0 && (_Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x - 1][y] == Tile_Manager.Cover_Kind.Default))
					{
						Partner.x = x - 1;
						Partner.y = y;
						Partner.transform.SetParent(_Tile.MY_Tile[x - 1][y].transform);
						Partner.T = Partner.transform.parent.GetComponent<Tile>();
						check = true;
					}
					break;
				case 1:
					if (x + 1 < _Tile.X && (_Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x + 1][y] == Tile_Manager.Cover_Kind.Default))
					{
						Partner.x = x + 1;
						Partner.y = y;
						Partner.transform.SetParent(_Tile.MY_Tile[x + 1][y].transform);
						Partner.T = Partner.transform.parent.GetComponent<Tile>();
						check = true;
					}
					break;
				case 2:
					if (y - 1 >= 0 && (_Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y - 1] == Tile_Manager.Cover_Kind.Default))
					{
						Partner.x = x;
						Partner.y = y - 1;
						Partner.transform.SetParent(_Tile.MY_Tile[x][y - 1].transform);
						Partner.T = Partner.transform.parent.GetComponent<Tile>();
						check = true;
					}
					break;
				case 3:
					if (y + 1 < _Tile.Y && (_Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.CanNot || _Tile.TileMap[x][y + 1] == Tile_Manager.Cover_Kind.Default))
					{
						Partner.x = x;
						Partner.y = y + 1;
						Partner.transform.SetParent(_Tile.MY_Tile[x][y + 1].transform);
						Partner.T = Partner.transform.parent.GetComponent<Tile>();
						check = true;
					}
					break;
			}

			r++;
			if (r >= 4)
				r = 0;
		}

		Partner.transform.localPosition = Vector2.zero;
	}

	public bool Hit(Weapon_Manager _Weapon)
	{
		float Total = _Weapon.Unit.aim + _Weapon.Unit.PostureBonus + _Weapon.Aim_Bonus;
		int Pressure_Bonus = pressure - ((pressure / 100) * (will / 2));
		int E_Pressure_Bonus = _Weapon.Unit.pressure - ((_Weapon.Unit.pressure / 100) * (_Weapon.Unit.will / 2));
		Total -= (DynamicVisualAcuity + PostureBonus);
		int I_Y = 0, I_X = 0;

		#region CoverBonus
		if (Mathf.Abs(x - _Weapon.Unit.x) > Mathf.Abs(y - _Weapon.Unit.y))
		{
			I_Y = 0;

			if (x > _Weapon.Unit.x) I_X = -1;
			else I_X = 1;
		}
		else
		{
			I_X = 0;

			if (y > _Weapon.Unit.y) I_Y = -1;
			else I_Y = 1;
		}

		if (_Tile.MY_Tile[x + I_X][y + I_Y].View == Tile.View_Kind.Full)
		{
			CoverBonus = 50;
			Total -= (CoverBonus - PostureBonus);
		}
		else if (_Tile.MY_Tile[x + I_X][y + I_Y].View == Tile.View_Kind.Half && (_Posture == Posture.Crouching || _Posture == Posture.Prone))
		{
			CoverBonus = 30;
			Total -= (CoverBonus - PostureBonus);
		}
		else if (_Tile.MY_Tile[x + I_X][y + I_Y].View == Tile.View_Kind.Low && _Posture == Posture.Prone)
		{
			CoverBonus = 70;
			Total -= (CoverBonus - PostureBonus);
		}

		//enemy
		if (_Tile.MY_Tile[_Weapon.Unit.x - I_X][_Weapon.Unit.y - I_Y].View == Tile.View_Kind.Half && _Weapon.Unit._Posture == Posture.Crouching)
		{
			Total += (10 - _Weapon.Unit.PostureBonus);
		}
		else if (_Tile.MY_Tile[_Weapon.Unit.x - I_X][_Weapon.Unit.y - I_Y].View == Tile.View_Kind.Full)
		{
			Total -= _Weapon.Unit.PostureBonus;
		}
		#endregion

		#region Pressure Bonus
		if (Pressure_Bonus >= 90) Total -= 8;
		else if (Pressure_Bonus >= 80) Total -= 5;
		else if (Pressure_Bonus >= 60) Total -= 2;

		if (E_Pressure_Bonus >= 90) Total -= 20;
		else if (E_Pressure_Bonus >= 80) Total -= 10;
		else if (E_Pressure_Bonus >= 60) Total -= 5;
		#endregion

		Damage_UI _UI = Instantiate(UI_Manager.Damage.gameObject, transform.position, Quaternion.identity).GetComponent<Damage_UI>();
		int rand = Random.Range(0, 100);

		if (rand > Total)
		{
			_UI.SetText("MISS");
			pressure += 10;
			_Weapon.Unit.pressure -= 5;
			return false;
		}
		else
		{
			rand = Random.Range(0, 100);


			if (rand < _Weapon.HeadShot_Percentage + _Weapon.Unit.HeadShotPercent + _Weapon.HeadShot_Bonus)
			{
				Health -= _Weapon.HeadShot_Damage;
				_UI.SetText("HeadShot " + _Weapon.HeadShot_Damage.ToString());
			}
			else
			{
				Health -= _Weapon.Damage;
				_UI.SetText(_Weapon.Damage.ToString());
			}
		}
		_Weapon.Unit.pressure -= 20;
		pressure += 20;
		if (Health <= 0) Death();

		return true;
	}

	public virtual void DrawActionPoint()
	{
	}
	public virtual void Select() { }

	public virtual void Death()
	{
		Board_Manager.m_Board_Manager.Death(this);
		Destroy(gameObject);
	}

	public virtual void UsePainKiller()
	{
		Painkiller_Count++;
		Health += 20;
		if (Health > 100) Health = 100;

		if (Painkiller_Count >= 3)
		{
		}
	}

	public virtual void TurnOn()
	{
		Set++;
		pressure -= 6;
		if (pressure < 0) pressure = 0;
		else if (pressure > 100) pressure = 100;
	}

	public virtual void LookUp()
	{
		switch (dir)
		{
			case Direction.right:
				GetComponent<SpriteRenderer>().flipX = false;
				break;
			case Direction.left:
				GetComponent<SpriteRenderer>().flipX = true;
				break;
			case Direction.down:
				break;
			case Direction.up:
				break;
		}
	}

	public virtual bool Dig_hasty_fighting_position()
	{
		if (T.Kind != Tile_Manager.Cover_Kind.Default && T.Kind != Tile_Manager.Cover_Kind.CanNot) return false;
		if (Now_Action_Point < 7) return false;
		DigHasty = true;


		
		return true;
	}

	public virtual bool Dig_Depper()
	{
		if (Now_Action_Point < 7) return false;

		if (T.Kind == Tile_Manager.Cover_Kind.Skimisher)
		{
			T.Kind = Tile_Manager.Cover_Kind.Slit;
			T.View = Tile.View_Kind.Half;
			Destroy(T._Obstacle);
			T._Obstacle = Instantiate(T.Obstacle_Manager.Obstacles[7], T.transform);
			T.direct = Tile.Direct.Any;
			Now_Action_Point -= 7;
		}

		else if (T.Kind == Tile_Manager.Cover_Kind.Slit)
		{
			T.Kind = Tile_Manager.Cover_Kind.Standing;
			T.View = Tile.View_Kind.Full;
			Destroy(T._Obstacle);
			T._Obstacle = Instantiate(T.Obstacle_Manager.Obstacles[8], T.transform);
			Now_Action_Point -= 7;
		}
		else
			return false;

		return true;
	}
	public virtual bool Expand()
	{
		if (Now_Action_Point < 8) return false;
		if (T.Kind == Tile_Manager.Cover_Kind.Slit || T.Kind == Tile_Manager.Cover_Kind.Standing)
			Expanding = true;

		return true;
	}


}

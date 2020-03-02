using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyWeapon_Manager : Weapon_Manager {

	public bool Action = false;
	public bool Btn1 = false;
	public bool Btn2 = false;
	public int Boom_Range;


	Input_Manager _Input;
	// Use this for initialization
	protected void Start()
	{
		Btn_Name[0] = "FIRE [3]";
		Btn_Name[1] = "AIMED SHOT [5]";
		Btn_Name[2] = null;
		Btn_Name[3] = null;
		Btn_Name[4] = null;

		_Kind = Kind.HEAVY_WEAPON;
		_Title = Title.Sub;

		_Input = Camera.main.GetComponent<Input_Manager>();
		_UI = GameObject.Find("Canvas").GetComponent<UI_MANAGER>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (!transform.parent.CompareTag("Player")) return;
		Vector2 pos = _Input.pos;
		RaycastHit2D hit = _Input.hit_Tile;
		Tile _Tile;


		
		if (Btn1 || Btn2)
		{
			if (Input_Manager.Highlighted != null)
			{
				Input_Manager.Highlighted();
				Input_Manager.Highlighted = null;
			}
			Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[0]);

			if (hit.transform != null && hit.transform.tag == "Tile")
			{
				_Tile = hit.transform.GetComponent<Tile>();

				if (Mathf.Abs(_Tile.X - Unit.x) + Mathf.Abs(_Tile.Y - Unit.y) <= Range)
				{
					for (int x = _Tile.X - 1; x <= _Tile.X + 1; x++)
					{
						for (int y = _Tile.Y - 1; y <= _Tile.Y + 1; y++)
						{
							if (x < 0 || x >= _Tile._Tile.X || y < 0 || y >= _Tile._Tile.Y) continue;

							Tile N_Tile = Tile_Manager.m_Tile_Manager.MY_Tile[x][y];

							N_Tile.HighLight(1);
						}
					}
				}
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			if (Btn1 || Btn2)
			{
				

				if (hit.transform != null && hit.transform.tag == "Tile")
				{
					 _Tile = hit.transform.GetComponent<Tile>();
					
					if (Mathf.Abs(_Tile.X - Unit.x) + Mathf.Abs(_Tile.Y - Unit.y) <= Range)
					{
						for (int x = _Tile.X - 1; x <= _Tile.X + 1; x++)
						{
							for (int y = _Tile.Y - 1; y <= _Tile.Y + 1; y++)
							{
								HeadShot_Bonus = 0;
								if (x < 0 || x >= _Tile._Tile.X || y < 0 || y >= _Tile._Tile.Y) continue;

								_Tile._Tile.MY_Tile[x][y].DownGrade();

								if (_Tile._Tile.MY_Tile[x][y].transform.childCount > 0)
								{
									if (_Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)") != null)
									{
										Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>();
										if (_Unit != null)
										{
											if (x == _Tile.X && y == _Tile.Y) HeadShot_Bonus = 100;

											_Unit.Hit(this);
										}
									}

								}
							}
						}

						Unit.Tile_InSighted = null;
						Unit.View.TestInView();

						Unit.Now_Action_Point -= 3;
						if (Btn2) Unit.Now_Action_Point -= 2;
						Unit.DrawActionPoint();

						Btn1 = false;
						Btn2 = false;
						Action = false;
						Unit.Weapons.Remove(this);
						Unit.Select();
						Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[1]);
						Destroy(gameObject);
					}
				}
			}

		}
	}

	public override void Select()
	{
		base.Select();
		for (int i = 0; i < _UI.Weapon_Button.Count; i++)
		{
			if (Btn_Name[i] == null) continue;
			_UI.Weapon_Button[i].gameObject.SetActive(true);
			_UI.Weapon_Button[i].transform.GetChild(0).GetComponent<Text>().text = Btn_Name[i];

		}

		_UI.Weapon_Button[0].onClick.AddListener(BTN1);
		_UI.Weapon_Button[1].onClick.AddListener(BTN2);

	}

	public override void BTN1()
	{
		if (Unit.Now_Action_Point < 3 || Action || Bullet <= 0) return;

		Action = true;
		Btn1 = true;
		Aim_Bonus = -5;

	}

	public override void BTN2()
	{
		if (Unit.Now_Action_Point < 5 || Action || Bullet <= 0) return;

		Action = true;
		Btn2 = true;
		Aim_Bonus = 5;

	}

	public override void Unselect()
	{
		base.Unselect();
		Btn1 = false;
		Btn2 = false;
		Action = false;
	}
}

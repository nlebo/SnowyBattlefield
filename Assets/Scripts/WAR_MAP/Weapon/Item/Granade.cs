using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Item_Manager {


	public bool Action, Btn1;

	// Use this for initialization
	new void Start () {
		base.Start();
		Weapon = new Weapon_Manager();
		Weapon.Unit = Unit;
		Weapon.Damage = 20;
		Weapon.HeadShot_Damage = 90;
		Weapon.Range = 10;
		Weapon._Tile = Tile_Manager.m_Tile_Manager;
		count = 3;
		Item_Name = "Granade";
		Action = false;
		Btn1 = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!transform.parent.CompareTag("Player")) return;
		Vector2 pos = _Input.pos;
		RaycastHit2D hit = _Input.hit_Tile;

		
		if (Btn1)
		{
			if (Input_Manager.Highlighted != null)
			{
				Input_Manager.Highlighted();
				Input_Manager.Highlighted = null;
			}
			Cursor_Manager.m_Cursor_Manager.SetCursor(UI_MANAGER.m_UI_MANAGER.Cursors[0]);



			if (hit.transform != null && hit.transform.tag == "Tile")
			{
				Tile _Tile = hit.transform.GetComponent<Tile>();

				if (Mathf.Abs(_Tile.X - Unit.x) + Mathf.Abs(_Tile.Y - Unit.y) <= Weapon.Range)
				{
					for (int x = _Tile.X - 1; x <= _Tile.X + 1; x++)
					{
						
						if (x < 0 || x >= Tile_Manager.m_Tile_Manager.X) continue;
						for (int y = _Tile.Y - 1; y <= _Tile.Y + 1; y++)
						{
							if (y < 0 || y >= Tile_Manager.m_Tile_Manager.Y) continue;
							if (x != _Tile.X && y != _Tile.Y) continue;

							Tile N_Tile = Tile_Manager.m_Tile_Manager.MY_Tile[x][y];

							N_Tile.HighLight(1);
						}
					}
				}
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (Btn1)
			{

				if (hit.transform != null && hit.transform.tag == "Tile")
				{
					Tile _Tile = hit.transform.GetComponent<Tile>();

					if (Mathf.Abs(_Tile.X - Unit.x) + Mathf.Abs(_Tile.Y - Unit.y) <= Weapon.Range)
                    {
                        if (Weapon.Hit())
                        {
                            for (int x = _Tile.X - 1; x <= _Tile.X + 1; x++)
                            {
                                for (int y = _Tile.Y - 1; y <= _Tile.Y + 1; y++)
                                {
                                    Weapon.HeadShot_Bonus = 0;
                                    if (x < 0 || x >= _Tile._Tile.X || y < 0 || y >= _Tile._Tile.Y) continue;
                                    if (x != _Tile.X && y != _Tile.Y) continue;

                                    _Tile._Tile.MY_Tile[x][y].DownGrade();
									_Tile._Tile.MY_Tile[x][y].Flash(2);

                                    if (_Tile._Tile.MY_Tile[x][y].transform.childCount > 0)
                                    {
                                        if (_Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)") != null)
                                        {
                                            Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>();
                                            if (_Unit != null)
                                            {
                                                if (x == _Tile.X && y == _Tile.Y) Weapon.HeadShot_Bonus = 100;

												int Aim_Bonus = Weapon.Aim_Bonus;
												Weapon.Aim_Bonus = 400;
                                                _Unit.Hit(Weapon);
												Weapon.Aim_Bonus = Aim_Bonus;
                                            }
                                        }
                                        else if (_Tile._Tile.MY_Tile[x][y].transform.Find("Player(Clone)") != null)
                                        {
                                            Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Player(Clone)").GetComponent<Unit_Manager>();
                                            if (_Unit != null)
                                            {
                                                if (x == _Tile.X && y == _Tile.Y) Weapon.HeadShot_Bonus = 100;

                                                int Aim_Bonus = Weapon.Aim_Bonus;
												Weapon.Aim_Bonus = 400;
                                                _Unit.Hit(Weapon);
												Weapon.Aim_Bonus = Aim_Bonus;
                                            }
                                        }

                                    }
                                }
                            }
                        }

						else
						{
							int PX = 0,PY= 0;

                            while (PX == 0 && PY == 0)
                            {
                                PX = Random.Range(-2, 3);
                                PY = Random.Range(-2, 3);

                                if (PX == 0 && PY == 0) PY++;

                                if (_Tile.X + PX < 0) PX = -_Tile.X;
                                else if (_Tile.X + PX >= _Tile._Tile.X) PX = _Tile._Tile.X - _Tile.X - 1;

                                if (_Tile.Y + PY < 0) PY = -_Tile.Y;
                                else if (_Tile.Y + PY >= _Tile._Tile.Y) PY = _Tile._Tile.Y - _Tile.Y - 1;
                            }

							for (int x = (_Tile.X - 1) + PX; x <= (_Tile.X + 1) + PX; x++)
                            {
                                for (int y = (_Tile.Y - 1) + PY; y <= (_Tile.Y + 1) + PY; y++)
                                {
                                    Weapon.HeadShot_Bonus = 0;
                                    if (x < 0 || x >= _Tile._Tile.X || y < 0 || y >= _Tile._Tile.Y) continue;
                                    if (x != _Tile.X && y != _Tile.Y) continue;

                                    _Tile._Tile.MY_Tile[x][y].DownGrade();
									_Tile._Tile.MY_Tile[x][y].Flash(2);

                                    if (_Tile._Tile.MY_Tile[x][y].transform.childCount > 0)
                                    {
                                        if (_Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)") != null)
                                        {
                                            Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>();
                                            if (_Unit != null)
                                            {
                                                if (x == _Tile.X && y == _Tile.Y) Weapon.HeadShot_Bonus = 100;
													
                                                int Aim_Bonus = Weapon.Aim_Bonus;
												Weapon.Aim_Bonus = 400;
                                                _Unit.Hit(Weapon);
												Weapon.Aim_Bonus = Aim_Bonus;
                                            }
                                        }
                                        else if (_Tile._Tile.MY_Tile[x][y].transform.Find("Player(Clone)") != null)
                                        {
                                            Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Player(Clone)").GetComponent<Unit_Manager>();
                                            if (_Unit != null)
                                            {
                                                if (x == _Tile.X && y == _Tile.Y) Weapon.HeadShot_Bonus = 100;

                                                int Aim_Bonus = Weapon.Aim_Bonus;
												Weapon.Aim_Bonus = 400;
                                                _Unit.Hit(Weapon);
												Weapon.Aim_Bonus = Aim_Bonus;
                                            }
                                        }

                                    }
                                }
                            }
						}

						Unit.Tile_InSighted = null;
						Unit.View.TestInView();

						Unit.Now_Action_Point -= 3;
						Unit.DrawActionPoint();

						Btn1 = false;
						Action = false;
						count--;

						Cursor_Manager.m_Cursor_Manager.SetCursor(UI_MANAGER.m_UI_MANAGER.Cursors[1]);

						if (count <= 0)
						{
							Unit.Items.Remove(this);
							Unit.Select();
							Destroy(gameObject);
						}
					}
				}
			}
		}
	}

	public override void Use()
	{
		base.Use();

		if (Unit.Now_Action_Point < 3 || Action) return;

		Action = true;
		Btn1 = true;
	}
	public override bool Rand(Enemy_Manager _this)
	{
		base.Rand(_this);
		if (Unit.Now_Action_Point < 3 || Action) return false;

		Unit_Manager CloserPlayer = _this.GetMeetPlayer()[_this.CloserPlayer()];
		if (Mathf.Abs(CloserPlayer.x - _this.x) + Mathf.Abs(CloserPlayer.y - _this.y) <= Weapon.Range)
		{
			Tile _Tile = CloserPlayer.T;

			for (int x = _Tile.X - 1; x <= _Tile.X + 1; x++)
			{
				for (int y = _Tile.Y - 1; y <= _Tile.Y + 1; y++)
				{
					Weapon.HeadShot_Bonus = 0;
					if (x < 0 || x >= _Tile._Tile.X || y < 0 || y >= _Tile._Tile.Y) continue;
					if (x != _Tile.X && y != _Tile.Y) continue;

					_Tile._Tile.MY_Tile[x][y].DownGrade();

					if (_Tile._Tile.MY_Tile[x][y].transform.childCount > 0)
					{
						if (_Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)") != null)
						{
							Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Enemy(Clone)").GetComponent<Unit_Manager>();
							if (_Unit != null)
							{
								if (x == _Tile.X && y == _Tile.Y) Weapon.HeadShot_Bonus = 100;

								_Unit.Hit(Weapon);
							}
						}
						else if (_Tile._Tile.MY_Tile[x][y].transform.Find("Player(Clone)") != null)
						{
							Unit_Manager _Unit = _Tile._Tile.MY_Tile[x][y].transform.Find("Player(Clone)").GetComponent<Unit_Manager>();
							if (_Unit != null)
							{
								if (x == _Tile.X && y == _Tile.Y) Weapon.HeadShot_Bonus = 100;

								_Unit.Hit(Weapon);
							}
						}

					}
				}
			}

			_this.Now_Action_Point -= 3;

			Btn1 = false;
			Action = false;
			count--;

			Cursor_Manager.m_Cursor_Manager.SetCursor(UI_MANAGER.m_UI_MANAGER.Cursors[1]);

			if (count <= 0)
			{
				_this.Items.Remove(this);
				Destroy(gameObject);
			}
		}
		return true;
	}

	public override void Unselect()
	{
		base.Unselect();
		Action = false;
		Btn1 = false;
	}
}

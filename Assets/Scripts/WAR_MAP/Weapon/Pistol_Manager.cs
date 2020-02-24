using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol_Manager : Weapon_Manager {

	public bool Action = false;
	public bool Btn1 = false;
	public bool Btn2 = false;
	public bool Btn3 = false;

	Input_Manager _Input;

	// Use this for initialization
	protected void Start()
	{
		Btn_Name[0] = "FIRE [3]";
		Btn_Name[1] = "AIMED SHOT [5]";
		Btn_Name[2] = "HAIL MARY [6]";
		Btn_Name[3] = "RERLOAD [2]";
		Btn_Name[4] = null;

		_Kind = Kind.PISTOL;
		_Title = Title.Sub;

		_Input = Camera.main.GetComponent<Input_Manager>();
		_UI = GameObject.Find("Canvas").GetComponent<UI_MANAGER>();
	}

	private void Update()
	{
		if (!transform.parent.CompareTag("Player")) return;
		if (Btn1 || Btn2 || Btn3) {
			Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[0]);
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (Btn1 || Btn2 || Btn3)
			{
				Vector2 pos = _Input.pos;
				RaycastHit2D hit = _Input.hit_Player;

				if (hit.transform != null && hit.transform.tag == "Enemy")
				{
					Unit_Manager _Unit = hit.transform.GetComponent<Unit_Manager>();
					if (Mathf.Abs(_Unit.x - Unit.x) + Mathf.Abs(_Unit.y - Unit.y) <= Range)
					{
						if (Btn1)
						{
							_Unit.Hit(this);
							Bullet--;
							Unit.Now_Action_Point -= 3;
						}
						else if (Btn2)
						{
							_Unit.Hit(this);
							Bullet--;
							Unit.Now_Action_Point -= 5;
						}
						else
						{
							_Unit.Hit(this);
							Bullet--;
							Unit.Now_Action_Point -= 6;
						}

						_UI._Clip.text = Bullet.ToString();

						Action = false;
						Btn1 = false;
						Btn2 = false;
						Btn3 = false;
						Aim_Bonus = 0;
						HeadShot_Bonus = 0;

						Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[1]);
						Unit.DrawActionPoint();
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
		_UI.Weapon_Button[2].onClick.AddListener(BTN3);
		_UI.Weapon_Button[3].onClick.AddListener(BTN4);
	}

	public override void BTN1()
	{
		if (Unit.Now_Action_Point < 3 || Action || Bullet <= 0) return;

		Action = true;
		Btn1 = true;


	}

	public override void BTN2()
	{
		if (Unit.Now_Action_Point < 5 || Action || Bullet <= 0) return;

		Action = true;
		Btn2 = true;
		Aim_Bonus = 10;

	}

	public override void BTN3()
	{
		if (Unit.Now_Action_Point < 6 || Action || Bullet <= 0) return;

		Action = true;
		Btn3 = true;
		Aim_Bonus = -40;
		HeadShot_Bonus = 100;
	}

	public override void BTN4()
	{
		if (Unit.Now_Action_Point < 2 || Action || Bullet == MaxBullet) return;

		Unit.Now_Action_Point -= 2;
		Bullet = MaxBullet;
		Unit.DrawActionPoint();
		_UI._Clip.text = Bullet.ToString();
	}

	public override void Reload()
	{
		base.Reload();
		BTN4();
	}

	public override void Unselect()
	{
		base.Unselect();
		Btn1 = false;
		Btn2 = false;
		Btn3 = false;
		Action = false;
	}

}

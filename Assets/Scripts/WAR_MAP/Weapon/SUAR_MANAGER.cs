using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SUAR_MANAGER : Weapon_Manager {

	public bool Action = false;
	public bool Btn1 = false;
	public bool Btn2 = false;
	public bool Btn3 = false;
	public bool Shooting = false;

	Input_Manager _Input;

	// Use this for initialization
	protected void Start () {
		Btn_Name[0] = "FIRE 3 TIMES [3]";
		Btn_Name[1] = "HALF CLIP [6]";
		Btn_Name[2] = "FULL CLIP [6]";
		Btn_Name[3] = "RERLOAD [3]";
		Btn_Name[4] = null;

		_Kind = Kind.SU_AR;
		_Title = Title.Main;

		_Input = Camera.main.GetComponent<Input_Manager>();
		_UI = GameObject.Find("Canvas").GetComponent<UI_MANAGER>();
	}

	private void Update()
	{
		if (!transform.parent.CompareTag("Player")) return;
		if (Btn1 || Btn2 || Btn3)
		{
			Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[0]);

			Vector2 pos = _Input.pos;
			RaycastHit2D hit = _Input.hit_Player;

            if (hit.transform != null && hit.transform.tag == "Enemy")
            {
                Unit_Manager _Unit = hit.transform.GetComponent<Unit_Manager>();

                if (!_UI.HitRate.gameObject.activeInHierarchy)
                {
                    _UI.HitRate.gameObject.SetActive(true);
                    _UI.HitRateText.text = _Unit.GetHitRate(this).ToString() + "%";
                }
            }

            else
            {
                if (_UI.HitRate.gameObject.activeInHierarchy) _UI.HitRate.gameObject.SetActive(false);
            }
		}
			if (Input.GetMouseButtonDown(0))
		{
			if ((Btn1 || Btn2 || Btn3) && !Shooting)
			{
				Vector2 pos = _Input.pos;
				RaycastHit2D hit = _Input.hit_Player;

				if (hit.transform != null && hit.transform.tag == "Enemy")
				{
					Unit_Manager _Unit = hit.transform.GetComponent<Unit_Manager>();
						if (Btn1)
						{
							StartCoroutine(_BTN1(_Unit,Bullet));
						}
						else if (Btn2)
						{
							StartCoroutine(_BTN2(_Unit));
						}
						else
						{
							StartCoroutine(_BTN3(_Unit, Bullet));
						}

						if (Btn2 || Btn3) Unit.Now_Action_Point -= 3;
						Unit.Now_Action_Point -= 3;

						Unit.DrawActionPoint();
						Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[1]);
						if (_UI.HitRate.gameObject.activeInHierarchy) _UI.HitRate.gameObject.SetActive(false);
					

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
		if (Unit.Now_Action_Point < 6 || Action || Bullet <= 14) return;

		Action = true;
		Btn2 = true;
		Aim_Bonus = -20;

	}

	public override void BTN3()
	{
		if (Unit.Now_Action_Point < 6 || Action || Bullet <= 19) return;

		Action = true;
		Btn3 = true;
		Aim_Bonus = -30;
	}

	public override void BTN4()
	{
		if (Unit.Now_Action_Point < 3 || Action || Bullet == MaxBullet) return;

		Unit.Now_Action_Point -= 3;
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

	IEnumerator _BTN1(Unit_Manager _Unit,int _Bullet)
	{
		Shooting = true;
		for (int i = 0; i < _Bullet; i++)
		{
			if (i >= 3) break;

			if (_Unit != null)
			{
				_Unit.Hit(this);
				Bullet--;
				_UI._Clip.text = Bullet.ToString();
			}
			yield return new WaitForSeconds(0.1f);
		}
		Aim_Bonus = 0;
		Action = false;
		Btn1 = false;
		Shooting = false;
		yield return null;
	}

	IEnumerator _BTN2(Unit_Manager _Unit)
	{
		Shooting = true;
		for (int i = 0; i < 15; i++)
		{
			if (_Unit != null)
			{
				_Unit.Hit(this);
				Bullet--;
				_UI._Clip.text = Bullet.ToString();
			}
			yield return new WaitForSeconds(0.1f);
		}
		Aim_Bonus = 0;
		Shooting = false;
		Action = false;
		Btn2 = false;
	}

	IEnumerator _BTN3(Unit_Manager _Unit,int _Bullet)
	{
		Shooting = true;
		for (int i = 0; i < _Bullet; i++)
		{
			if (_Unit != null)
			{
				_Unit.Hit(this);
				Bullet--;
				_UI._Clip.text = Bullet.ToString();
			}
			yield return new WaitForSeconds(0.1f);
		}
		Aim_Bonus = 0;
		Shooting = false;
		Action = false;
		Btn3 = false;
	}

	public override bool Rand(Unit_Manager _Unit)
	{
		base.Rand(_Unit);

		if (Bullet > 19 && Unit.Now_Action_Point >= 6)
		{
			switch (Random.Range(1, 4))
			{
				case 1:
					BTN1();
					break;
				case 2:
					BTN2();
					break;
				case 3:
					BTN3();
					break;
			}
		}
		else if (Bullet >= 15 && Unit.Now_Action_Point >= 6)
		{
			switch (Random.Range(1, 3))
			{
				case 1:
					BTN1();
					break;
				case 2:
					BTN2();
					break;
			}
		}
		else if (Bullet > 0 && Unit.Now_Action_Point >= 3)
        {
            BTN1();
        }
        else if (Unit.Now_Action_Point >= 3)
            BTN4();
        else return false;

        if (Btn1 || Btn2 || Btn3)
        {


            if (Btn1)
            {
                StartCoroutine(_BTN1(_Unit, Bullet));
            }
            else if (Btn2)
            {
                StartCoroutine(_BTN2(_Unit));
            }
            else
            {
                StartCoroutine(_BTN3(_Unit, Bullet));
            }

            if (Btn2 || Btn3) Unit.Now_Action_Point -= 3;
            Unit.Now_Action_Point -= 3;

        }

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rifle_Manager : Weapon_Manager {

	public bool Action = false;
	public bool Btn1 = false;
	public bool Btn2 = false;

	Input_Manager _Input;

	// Use this for initialization
	protected void Start () {
		Btn_Name[0] = "FIRE [3]";
		Btn_Name[1] = "AIMED SHOT [6]";
		Btn_Name[2] = "REROALD [3]";
		Btn_Name[3] = null;
		Btn_Name[4] = null;

		_Kind = Kind.RIFLE;
		_Title = Title.Main;

		_Input = Camera.main.GetComponent<Input_Manager>();
		_UI = GameObject.Find("Canvas").GetComponent<UI_MANAGER>();
	}

	private void Update()
	{
		if (!transform.parent.CompareTag("Player") || CameraFlag[0]) return;

		if (Btn1 || Btn2)
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
			if (Btn1 || Btn2)
			{
				Vector2 pos = _Input.pos;
				RaycastHit2D hit = _Input.hit_Player;

				if (hit.transform != null && hit.transform.tag == "Enemy")
				{
					Unit_Manager _Unit = hit.transform.GetComponent<Unit_Manager>();

						Shot(_Unit);
						CameraFlag[0] = true;
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

	}

	public override void BTN1()
	{
		if (Unit.Now_Action_Point < 3 || Action || Bullet <= 0) return;

		Action = true;
		Btn1 = true;


	}

	public override void BTN2()
	{
		if (Unit.Now_Action_Point < 6 || Action || Bullet <= 0) return;

		Action = true;
		Btn2 = true;
		Aim_Bonus = 20;

	}

	public override void BTN3()
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
		BTN3();
	}

	public override void Unselect()
	{
		base.Unselect();
		Btn1 = false;
		Btn2 = false;
		Action = false;

	}

	public override bool Rand(Unit_Manager _Unit)
	{
		base.Rand(_Unit);

		if (Bullet >= 1 && Unit.Now_Action_Point >= 6)
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
		else if (Bullet >= 1 && Unit.Now_Action_Point >= 3)
			BTN1();
		else if (Unit.Now_Action_Point >= 3)
			BTN3();
		else
			return false;

		if (Btn1 || Btn2)
		{
				_Unit.Hit(this);
				Action = false;

				if (Btn2) Unit.Now_Action_Point -= 3;
				Unit.Now_Action_Point -= 3;

				Btn2 = false;
				Btn1 = false;

				Bullet--;
				Aim_Bonus = 0;

		}

		return true;
	}

    public override void Shot(Unit_Manager _Unit)
    {
        StartCoroutine(_Shot(_Unit));
    }

    IEnumerator _Shot(Unit_Manager _Unit)
	{
		Camera_Move C = Camera_Move.m_Camera_Move;

		Cursor.lockState = CursorLockMode.Locked;
		C.Event = true;
		C.ActionZoomIn(Unit.transform.position);
		yield return new WaitForSeconds(1.3f);

        C.StartCoroutine(C.CameraMove(Unit.transform.position, _Unit.transform.position, 0.7f));
        yield return new WaitForSeconds(1.3f);

        if(_Unit.Hit(this))
		{
			yield return null;
			if(_Unit == null)
			{
				yield return new WaitForSeconds(2.5f);
			}
			else
				yield return new WaitForSeconds(1.5f);
		}
		else
		{
			yield return new WaitForSeconds(1.5f);
		}
        Action = false;

        if (Btn2) Unit.Now_Action_Point -= 3;
        Unit.Now_Action_Point -= 3;

        Btn2 = false;
        Btn1 = false;

        Bullet--;
        _UI._Clip.text = Bullet.ToString();
        Aim_Bonus = 0;
        Unit.DrawActionPoint();
		
		C.ActionZoomOut();
		yield return new WaitForSeconds(0.2f);

		Cursor.lockState = CursorLockMode.None;
		CameraFlag[0] = false;
		C.Event = false;
		yield return null;
	}
}

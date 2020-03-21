using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Manager : MonoBehaviour {

	public Unit_Manager Unit;
	public UI_MANAGER _UI;
	public Tile_Manager _Tile;

	public enum Title { Main = 0, Sub };
	public Title _Title;

	public enum Kind { RIFLE=0,SU_AR,EU_AR,PISTOL,HEAVY_WEAPON,HEAVY_MACHINE_GUN,MORTAR};
	public Kind _Kind;

	public string[] Btn_Name = new string[5];
	public string Weapon_Name;

	public int Damage, HeadShot_Damage, HeadShot_Percentage, Stun_Percentage;
	public int Aim_Bonus, HeadShot_Bonus, Range;
	public int Bullet, MaxBullet;

	protected bool[] CameraFlag;


	public void Awake()
	{
		Aim_Bonus = 0;
		HeadShot_Bonus = 0;
		Range = 20;

		CameraFlag = new bool[5]{false,false,false,false,false};
	}

	public virtual void Select()
	{
		_UI._Clip.gameObject.SetActive(true);
		_UI._Clip.text = Bullet.ToString() +  " / "  + MaxBullet.ToString();
		_Tile = Tile_Manager.m_Tile_Manager;
		_UI.Weapon_Name.gameObject.SetActive(true);
		_UI.Weapon_Name.text = Weapon_Name;
	}

	public virtual void BTN1()
	{

	}

	public virtual void BTN2()
	{

	}

	public virtual void BTN3()
	{

	}

	public virtual void BTN4()
	{

	}

	public virtual void BTN5()
	{

	}

	public virtual void Unselect()
	{

		Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[1]);
		_UI.Weapon_Name.gameObject.SetActive(false);
	}

	public virtual bool Rand(Unit_Manager _Unit)
	{
		if (Unit.Partner != null) Unit.Now_Action_Point = 0;

		return false;
	}

	public virtual void Reload()
	{
	}

	public bool Hit()
	{
		float Total = Unit.aim + Unit.PostureBonus + Aim_Bonus;
		int Pressure_Bonus = Unit.pressure - ((Unit.pressure / 100) * (Unit.will / 2));
		int I_X = 0,I_Y = 0;

        switch (Unit.dir)
        {
            case Unit_Manager.Direction.left:
                I_X = -1;
                break;
            case Unit_Manager.Direction.right:
                I_X = +1;
                break;
            case Unit_Manager.Direction.up:
                I_Y = +1;
                break;
            case Unit_Manager.Direction.down:
                I_Y = -1;
                break;
        }

        if (Unit.x + I_X >= 0 && Unit.x + I_X < _Tile.X && Unit.y + I_Y >= 0 && Unit.y + I_Y < _Tile.Y)
        {
            if (_Tile.MY_Tile[Unit.x + I_X][Unit.y + I_Y].View == Tile.View_Kind.Half && (Unit._Posture == Unit_Manager.Posture.Crouching || Unit._Posture == Unit_Manager.Posture.Prone))
            {
                Total += (10 - Unit.PostureBonus);
            }
            else if (_Tile.MY_Tile[Unit.x + I_X][Unit.y + I_Y].View == Tile.View_Kind.Full)
            {
                Total -= Unit.PostureBonus;
            }
        }

		if (Pressure_Bonus >= 90) Total -= 20;
		else if (Pressure_Bonus >= 80) Total -= 10;
		else if (Pressure_Bonus >= 60) Total -= 5;

		int rand = Random.Range(0, 100);

		if (rand > Total)
			return false;
		else return true;
	}

	public virtual void Shot(Unit_Manager _Unit)
	{
		StartCoroutine(_Shot(_Unit));
	}

	IEnumerator _Shot(Unit_Manager _Unit)
	{
		Camera_Move C = Camera_Move.m_Camera_Move;
		C.Event = true;
		C.ActionZoomIn(Unit.transform.position);
		yield return new WaitForSeconds(1.3f);

		C.StartCoroutine(C.CameraMove(Unit.transform.position,_Unit.transform.position,0.7f));
		yield return new WaitForSeconds(1.3f);

		C.ActionZoomOut();
		C.Event = false;
		yield return null;
	}
}

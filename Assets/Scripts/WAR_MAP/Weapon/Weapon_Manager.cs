using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Manager : MonoBehaviour {

	public Unit_Manager Unit;
	public UI_MANAGER _UI;

	public enum Title { Main = 0, Sub };
	public Title _Title;

	public enum Kind { RIFLE=0,SU_AR,EU_AR,PISTOL,HEAVY_WEAPON,HEAVY_MACHINE_GUN,MORTAR};
	public Kind _Kind;

	public string[] Btn_Name = new string[5];
	public string Weapon_Name;

	public int Damage, HeadShot_Damage, HeadShot_Percentage, Stun_Percentage;
	public int Aim_Bonus, HeadShot_Bonus, Range;
	public int Bullet, MaxBullet;

	public void Awake()
	{
		Aim_Bonus = 0;
		HeadShot_Bonus = 0;
		Range = 20;
	}

	public virtual void Select()
	{
		_UI._Clip.gameObject.SetActive(true);
		_UI._Clip.text = Bullet.ToString() +  " / "  + MaxBullet.ToString();
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
	}

	public virtual bool Rand(Unit_Manager _Unit)
	{
		if (Unit.Partner != null) Unit.Now_Action_Point = 0;

		return false;
	}

	public virtual void Reload()
	{
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Manager : MonoBehaviour {

	public Unit_Manager Unit;
	public Input_Manager _Input;
	public Weapon_Manager Weapon;

	public int count;
	public string Item_Name;

	protected void Start()
	{
		_Input = Camera.main.GetComponent<Input_Manager>();
	}

	public virtual void Use()
	{
	}

	public virtual bool Rand(Enemy_Manager _this)
	{
		return true;
	}

	public virtual void Unselect()
	{
		Cursor_Manager.m_Cursor_Manager.SetCursor(UI_MANAGER.m_UI_MANAGER.Cursors[1]);
	}
}

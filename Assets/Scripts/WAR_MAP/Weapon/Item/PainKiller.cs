using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainKiller : Item_Manager {

	public int effect = 20;

	 new void Start()
	{
		base.Start();
		Item_Name = "PainKiller";
		count = 3;
	}

	public override void Use()
	{
		base.Use();
		if (Unit.Health >= 100 || Unit.Now_Action_Point < 3) return;

		Unit.UsePainKiller();
		Unit.Now_Action_Point -= 3;
		Unit.DrawActionPoint();
		count--;


		if (count <= 0)
		{
			Unit.Items.Remove(this);
			Unit.Select();
			Destroy(gameObject);
		} 
	}
}

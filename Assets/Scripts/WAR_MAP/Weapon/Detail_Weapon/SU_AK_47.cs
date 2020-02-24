using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SU_AK_47 : SUAR_MANAGER {

	public int Model = 1;

	// Use this for initialization
	new void Start () {
		base.Start();
		Weapon_Name = "AK_47";
		switch (Model)
		{
			case 1:
				Damage = 10;
				HeadShot_Damage = 20;
				break;
			case 2:
				Damage = 15;
				HeadShot_Damage = 15;
				break;
			case 3:
				Damage = 10;
				HeadShot_Damage = 15;
				break;
		}

		MaxBullet = 30;
		Bullet = MaxBullet;

	}

}

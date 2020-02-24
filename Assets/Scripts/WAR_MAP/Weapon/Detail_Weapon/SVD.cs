using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVD : Rifle_Manager {

	public int Model = 1;

	// Use this for initialization
	new void Start () {
		base.Start();
		Weapon_Name = "SVD";
		switch (Model)
		{
			case 1:
				Damage = 40;
				HeadShot_Damage = 70;
				HeadShot_Percentage = 20;
				break;
			case 2:
				Damage = 55;
				HeadShot_Damage = 60;
				break;
			case 3:
				Damage = 30;
				HeadShot_Damage = 40;
				HeadShot_Percentage = 40;
				Stun_Percentage = 40;
				break;
		}

		MaxBullet = 10;
		Bullet = MaxBullet;
	}
}

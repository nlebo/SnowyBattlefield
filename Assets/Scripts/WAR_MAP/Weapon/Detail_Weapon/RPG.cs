using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPG : HeavyWeapon_Manager {



	// Use this for initialization
	new void Start () {
		base.Start();
		Weapon_Name = "RPG";

		Damage = 30;
		HeadShot_Damage = 50;

		MaxBullet = 1;
		Bullet = MaxBullet;

		Range = 10;
		Boom_Range = 3;
	}
	
}

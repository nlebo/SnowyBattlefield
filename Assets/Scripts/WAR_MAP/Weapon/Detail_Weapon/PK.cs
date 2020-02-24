using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PK : Heavy_MachinGun_Manager {

	// Use this for initialization
	new void Start () {
		base.Start();
		Weapon_Name = "PK";
		Damage = 15;
		HeadShot_Bonus = -100;

		MaxBullet = 100;
		Bullet = MaxBullet;

	}
	
}

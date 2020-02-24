using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tokarev : Pistol_Manager {


	// Use this for initialization
	new void Start()
	{
		base.Start();
		Weapon_Name = "Tokarev";
		Damage = 15;
		HeadShot_Damage = 50;

		MaxBullet = 8;
		Bullet = MaxBullet;
	}
}

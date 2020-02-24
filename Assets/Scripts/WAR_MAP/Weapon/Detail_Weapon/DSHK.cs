using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSHK : Heavy_MachinGun_Manager
{

	// Use this for initialization
	new void Start()
	{
		base.Start();
		Weapon_Name = "DSHK";
		Damage = 30;
		HeadShot_Bonus = -100;

		MaxBullet = 50;
		Bullet = MaxBullet;

	}
}

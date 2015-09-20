﻿using UnityEngine;
using System.Collections;

public class ProjectileWeapon : MonoBehaviour, IWeapon {

	public string name = null;

	[SerializeField]
	private float fireRate;
	private float coolDown;

	private Transform parent;

	[SerializeField]
	private WeaponClass weaponClass;
	[SerializeField]
	private WeaponType weaponType;
	[SerializeField]
	private Transform weaponProjectile;
	private Transform projectileSpawn;

	void Start () {
		this.projectileSpawn = transform.FindChild ("ProjectileSpawn");
		this.parent = transform.root;
		Debug.Log (this.projectileSpawn.transform.root.name + ", " + this.transform.root);

	}

	void Update () {
		if (coolDown > 0) {
			this.coolDown -= Time.deltaTime;
		}
	}

	public bool CanFire () {
		if (coolDown <= 0) {
			return true;
		}
		return false;
	}

	public WeaponClass GetWeaponClass () {
		return this.weaponClass;
	}
	
	public WeaponType GetWeaponType () {
		return this.weaponType;
	}

	public void Fire () {
		this.coolDown = 1 / fireRate;
		Instantiate (weaponProjectile, this.projectileSpawn.transform.position, this.transform.root.rotation);
		Debug.Log ("Entity " + parent.name + " is firing Weapon: " + this.name);
	}
}

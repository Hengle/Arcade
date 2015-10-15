using UnityEngine;
using System.Collections;

public class MissileWeapon : MonoBehaviour, IWeapon {

	public string name = null;
	
	[SerializeField]
	private float fireRate;
	private float coolDown;
	
	[SerializeField]
	private WeaponClass weaponClass;
	[SerializeField]
	private WeaponType weaponType;
	[SerializeField]
	private Transform weaponEffect;
	private AudioSource audio;
	
	private Transform projectileSpawn;

	public WeaponType GetWeaponType {
		get {return weaponType;}
	}
	
	public WeaponClass GetWeaponClass {
		get {return weaponClass;}
	}
	
	public bool CanFire {
		get {return (coolDown <= 0) ? true : false;}
	}

	void Awake () {
		projectileSpawn = transform.FindChild ("ProjectileSpawn");
		//audio = GetComponent<AudioSource> ();
	}

	public void Fire () {
		if (coolDown <= 0) {
			Transform t = (Transform) Instantiate (weaponEffect, projectileSpawn.position, projectileSpawn.rotation);
			t.GetComponent<Rigidbody> ().velocity = transform.forward * 100f;
		}
	}
}

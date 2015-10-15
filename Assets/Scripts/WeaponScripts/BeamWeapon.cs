using UnityEngine;
using System.Collections;

public class BeamWeapon : MonoBehaviour, IWeapon {

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

	
	public bool CanFire {
		get {return (coolDown <= 0) ? true : false;}
	}
	
	public WeaponClass GetWeaponClass {
		get {return weaponClass;}
	}
	
	public WeaponType GetWeaponType {
		get {return weaponType;}
	}

	void Start () {
		projectileSpawn = transform.FindChild ("ProjectileSpawn");
		audio = GetComponent<AudioSource> ();
	}
	
	void Update () {
		if (coolDown > 0) {
			this.coolDown -= Time.deltaTime;
		}
		Debug.DrawLine (transform.position, transform.position + transform.forward * 100);
	}

	public void Fire () {
		if (coolDown <= 0) {
			coolDown = 1 / fireRate;
			audio.Play ();
			Transform t = (Transform) Instantiate (weaponEffect, projectileSpawn.transform.position, transform.rotation);
			t.SetParent (projectileSpawn.transform);
		}
	}
}

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
		if (coolDown <= 0) {
			coolDown = 1 / fireRate;
			audio.Play ();
			Instantiate (weaponEffect, projectileSpawn.transform.position, transform.root.rotation);
		}
	}
}

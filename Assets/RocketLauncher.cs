using UnityEngine;
using System.Collections;

public class RocketLauncher : MonoBehaviour, IWeapon {

	[SerializeField]
	private float fireRate;
	private float coolDown = 0f;
	
	[SerializeField]
	private WeaponType weaponType;
	[SerializeField]
	private WeaponClass weaponClass;

	public Transform[] projectileSpawn;
	public Transform missile;

	public bool CanFire {
		get {return (coolDown <= 0) ? true : false;}
	}

	public WeaponType GetWeaponType{
		get {return weaponType;}
	}

	public WeaponClass GetWeaponClass {
		get {return weaponClass;}
	}

	void Update () {
		if (coolDown > 0) {
			coolDown -= Time.deltaTime;
		}
	}

	public void Fire () {
		if (CanFire) {
			coolDown = 1 / fireRate;

			foreach (Transform tr in projectileSpawn) {
				Transform t;
				t = (Transform) Instantiate (missile, tr.position, transform.rotation);
				t.GetComponent<Rigidbody> ().AddForce (-transform.root.up * 3f);
			}
		}
	}
}

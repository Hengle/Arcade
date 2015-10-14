using UnityEngine;
using System.Collections;

public class NPCWeaponManager : MonoBehaviour {

	public float fireAtBaseRange = 1000f;

	public GameObject[] weaponMounts;
	private IWeapon[] weaponSystems;

	public bool testWeapons = false;

	// Use this for initialization
	void Start () {
		weaponSystems = new IWeapon[weaponMounts.Length];

		for (int i = 0; i < weaponMounts.Length; i++) {
			weaponSystems[i] = weaponMounts[i].transform.GetChild (0).GetComponent<IWeapon> ();
		}

		if (testWeapons) {
			StartCoroutine ("WeaponTest");
		}
	}

	void FixedUpdate () {
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, fireAtBaseRange)) {
			if (hit.transform.tag.Equals ("PlayerBase")) {
				Fire ();
			}
		}
	}

	public void Fire () {

		foreach (IWeapon wep in weaponSystems) {
			Debug.DrawLine (transform.position, transform.position + transform.forward * 200, Color.red);

			wep.Fire ();
		}
	}

	IEnumerator WeaponTest () {
		while (true) {
			weaponSystems[0].Fire ();
			yield return new WaitForSeconds (0.1f);
		}
	}

}

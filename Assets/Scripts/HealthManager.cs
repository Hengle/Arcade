using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour, IDamageable, ILiving {

	[SerializeField]
	private float maxHealth = 100;
	[SerializeField]
	private float currentHealth;
	private bool isAlive = true;
	public bool isPlayer = false;
	public float modelRemoveDelay = 1f;
	private bool deathInitialized = false;

	[SerializeField]
	private ResistanceProfile resistanceProfile = new ResistanceProfile ();
	public EnergyBar healthBar;
	public Transform deathEffect;

	bool deathMessegeSent = false;

	void Start () {
		currentHealth = maxHealth;
		if (isPlayer) {
			healthBar = GameObject.FindWithTag ("Player1Health").GetComponent<EnergyBar> ();

		}
		if (healthBar != null) {
			healthBar.valueMax = (int) maxHealth;
		}
	}

	void Update () {
		if (maxHealth < currentHealth) {
			currentHealth = maxHealth;
		}

		if (currentHealth <= 0) {
			isAlive = false;

			if (!isPlayer) {
				deathInitialized = true;

				if (!deathMessegeSent) {
					SendMessage ("OnDeath", SendMessageOptions.DontRequireReceiver);
					deathMessegeSent = true;
				}

				if (modelRemoveDelay < 0) {
					Destroy (this.gameObject);
				}
				modelRemoveDelay -= Time.deltaTime;
			} else  {
				transform.gameObject.SetActive (false);
				GetComponent<PlayerData> ().Respawn ();
			}
		}
		if (healthBar != null) {
			if (currentHealth < 0) {
				healthBar.valueCurrent = 0;
			}
			healthBar.valueCurrent = (int) currentHealth;
		}
	}

	void OnEnable () {
		isAlive = true;
		deathMessegeSent = false;
		currentHealth = maxHealth;
	}

	void OnStartGame () {
		currentHealth = maxHealth;
	}

	void OnDeath () {
		
		if (deathEffect != null) {
			Transform t = (Transform) Instantiate (deathEffect, transform.position, transform.rotation);
			t.SetParent (transform);
		}
	}

	public void Damage (DamageProfile dp) {
		currentHealth -= CalculateDamage (resistanceProfile, dp, 1);
	}

	public void DamageWithFallOff (DamageProfile dp, float reduction) {
		if (1 - reduction > 0) {
			currentHealth -= CalculateDamage (resistanceProfile, dp, 1 - reduction);
		}
	}

	public void SetHealthMod (float mod) {
		maxHealth *= mod;
	}

	public bool Respawn () {
		return true;
	}

	public bool IsAlive () {
		return isAlive;
	}

	private static float CalculateDamage (ResistanceProfile rp, DamageProfile dp, float multip) {
		float damage = 0;
		float explosionDamage = dp.ExplosionDamage, laserDamage = dp.LaserDamage, plasmaDamage = dp.PlasmaDamage, piercingDamage = dp.PiercingDamage;
		if (explosionDamage > 0) {
			damage += explosionDamage * (rp.ExplosionResistance / (100 + rp.ExplosionResistance));
		}
		if (laserDamage > 0) {
			damage += laserDamage * (rp.LaserResistance / (100 + rp.LaserResistance));
		}
		if (plasmaDamage > 0) {
			damage += plasmaDamage * (rp.PlasmaResistance / (100 + rp.PlasmaResistance));
		}
		if (piercingDamage > 0) {
			damage += piercingDamage * (rp.PiercingResistance / (100 + rp.PlasmaResistance));
		}
		Debug.Log ("Damage dealt: " + damage * multip);
		return damage * multip;
	}
}

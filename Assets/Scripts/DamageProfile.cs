using UnityEngine;
using System.Collections;

[System.Serializable]
public class DamageProfile {

	[SerializeField]
	private float damageExplosive;
	[SerializeField]
	private float damageLaser;
	[SerializeField]
	private float damagePlasma;
	[SerializeField]
	private float damagePiercing;
	
	public float ExplosionDamage {
		get {return damageExplosive;}
	}
	public float PiercingDamage {
		get {return damagePiercing;}
	}
	public float PlasmaDamage {
		get {return damagePlasma;}
	}
	public float LaserDamage {
		get {return damageLaser;}
	}
}

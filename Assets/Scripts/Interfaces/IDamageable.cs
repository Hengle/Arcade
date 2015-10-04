using UnityEngine;
using System.Collections;

public interface IDamageable {

	void Damage (DamageProfile amount);

	void DamageWithFallOff (DamageProfile amout, float reduction);
}

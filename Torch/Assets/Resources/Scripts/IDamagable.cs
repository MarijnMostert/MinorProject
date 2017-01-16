using UnityEngine;

public interface IDamagable {
	/// <summary>
	/// Object takes Damage
	/// </summary>
	/// <param name="damage">The amount of damage inflicted</param>
	/// <param name="crit">If set to <c>true</c>, it does twice the damage</param>
	/// <param name="source">The gameObject that caused the damage</param>
	void takeDamage(int damage, bool crit, GameObject source);
	void Die();
}

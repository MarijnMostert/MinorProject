using UnityEngine;

public interface IDamagable {
	void takeDamage(int damage, bool crit);
	void Die();
}

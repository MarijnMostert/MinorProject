using UnityEngine;
using System.Collections;

public class nest_trigger : MonoBehaviour,IDamagable {
    Nest nest;

	// Use this for initialization
	void Start () {
        nest = GetComponentInParent<Nest>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void takeDamage(int damage, bool crit, GameObject source)
    {
        nest.takeDamage(damage,crit,source);
    }

    public void Die()
    {
        nest.Die();
    }
}

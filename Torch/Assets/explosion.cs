using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
    public int damage;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void Update () {
	    
	}

    
    void OnTriggerEnter(Collider other)
    {
        IDamagable damagableObject = other.GetComponent<IDamagable>();
        GameObject objectHitted = other.gameObject;

        float distance = Vector3.Distance(objectHitted.transform.position, transform.position);
        float splash_damage = (Mathf.Abs(distance - 1.5f)*damage);
        Debug.Log("Debug explosion: " + objectHitted.name + " distance: " + distance + " damage: "+splash_damage);


        if (damagableObject != null)
        {
            damagableObject.takeDamage(damage,false);
        }
        else if (objectHitted.CompareTag("Player"))
        {
            objectHitted.transform.FindChild("Torch").GetComponent<IDamagable>().takeDamage(damage,false);
        }
    }

    void setDamage(int damage)
    {
        this.damage = damage;
    }
}

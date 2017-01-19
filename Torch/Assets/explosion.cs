using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {
    public int damage;
    float start_time;

	// Use this for initialization
	void Start () {
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update () {
	    
	}


    void OnTriggerEnter(Collider other)
    {
        if ((Time.time - start_time) < 0.5) {
            IDamagable damagableObject = other.GetComponent<IDamagable>();
            GameObject objectHitted = other.gameObject;

            float distance = Vector3.Distance(objectHitted.transform.position, transform.position);
            float splash_damage = (Mathf.Abs(distance - 1.5f) * damage);
//            Debug.Log("Debug explosion: " + objectHitted.name + " distance: " + distance + " damage: " + splash_damage);


            if (damagableObject != null)
            {
				damagableObject.takeDamage(damage, false, gameObject);
            }
            else if (objectHitted.CompareTag("Player"))
            {
				objectHitted.transform.FindChild("Torch").GetComponent<IDamagable>().takeDamage(damage, false, gameObject);
            }
        }
    }

    void setDamage(int damage)
    {
        this.damage = damage;
    }
}

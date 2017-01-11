using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler Instance;

	[Serializable]
	public struct PooledObjects {
		public GameObject objectToPool;
		public int poolAmount;
		public bool willGrow; 
		public List<GameObject> objects;
	}

	public List<PooledObjects> pooledObjects;


	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		for (int i = 0; i < pooledObjects.Count; i++) {
			//pooledObjects [i].objects = new List<GameObject> ();
			for(int j = 0; j < pooledObjects[i].poolAmount; j++){
				GameObject obj = Instantiate (pooledObjects [i].objectToPool, transform) as GameObject;
				obj.SetActive (false);
				pooledObjects[i].objects.Add(obj);
			}
		}
	}

	/// <summary>
	/// <para>Gets the object</para>
	/// <para>0 = damagePopUp</para>
	/// <para>1 = fireBall projectile</para>
	/// <para>2 = iceBall projectile</para>
	/// <para>3 = piercing Projectile</para>
	/// <para>4 = dark Projectile</para>
	/// <para>5 = fireBall particles OnHit</para>
	/// <para>6 = iceBall particles OnHit</para>
	/// <para>7 = Piercing particles OnHit</para>
	/// <para>8 = Dark particles OnHit</para>
	/// </summary>
	/// <returns>The object</returns>
	public GameObject GetObject(int type, bool setActive){
		for (int i = 0; i < pooledObjects [type].objects.Count; i++) {
			GameObject obj = pooledObjects [type].objects [i];
			if (obj != null && !obj.activeInHierarchy) {
				if (setActive) {
					obj.SetActive (true);
				}
				return pooledObjects [type].objects [i];
			}
		}

		if (pooledObjects[type].willGrow) {
			GameObject obj = Instantiate (pooledObjects [type].objectToPool, transform) as GameObject;
			pooledObjects [type].objects.Add (obj);
			if (!setActive) {
				obj.SetActive (false);
			}
			Debug.Log ("Created " + obj);
			return obj;
		}

		Debug.Log ("The requested object is already being used. \n Consider making this pool larger or growable.");
		return null;
	}

	public GameObject GetObject(int type, bool setActive, Vector3 position){
		GameObject obj = GetObject (type, setActive);
		obj.transform.position = position;
		return obj;
	}

	public GameObject GetObject(int type, bool setActive, Vector3 position, Quaternion rotation){
		GameObject obj = GetObject (type, setActive, position);
		obj.transform.rotation = rotation;
		return obj;
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.O)) {
			GetObject (0, true);
		}
		if (Input.GetKeyDown (KeyCode.Equals)) {
			GetObject (1, true);
		}
	}

}

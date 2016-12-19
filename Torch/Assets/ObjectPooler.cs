﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler current;

	public GameObject ObjectToPool;
	public int poolAmount = 20;
	public bool willGrow = true;

	[SerializeField] List<GameObject> pooledObjects;

	void Awake(){
		current = this;
	}

	// Use this for initialization
	void Start () {
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < poolAmount; i++) {
			GameObject obj = Instantiate (ObjectToPool, transform) as GameObject;
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}
	
	public GameObject GetObject(){
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy) {
				return pooledObjects [i];
			}
		}

		if (willGrow) {
			GameObject obj = Instantiate (ObjectToPool, transform) as GameObject;
			pooledObjects.Add (obj);
			poolAmount++;
			return obj;
		}

		return null;
	}

	void Update(){
		/*
		if (Input.GetKeyDown (KeyCode.O)) {
			GameObject obj = GetObject ();
			obj.SetActive (true);
		}
		*/
	}

}

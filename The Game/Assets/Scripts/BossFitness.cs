using UnityEngine;
using System.Collections;

public class BossFitness : MonoBehaviour {

	public float timeAlive = 0f;
	public int damageDealt = 0;
	public int usedAttacks = 0;
	public int usedSpecAttacks = 0;

	public float CalculateRatio(){
		return usedAttacks / usedSpecAttacks;
	}
}

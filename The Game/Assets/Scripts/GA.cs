using UnityEngine;
using System.Collections;

public class GA : MonoBehaviour {

	public float[,] population;
	public float[,] chromosomesToReproduce;
	public float[] fitness;

	public int sizePopulation;
	public int sizeChromosomes;
	public int NReproduce;

	// Use this for initialization
	void Start () {
		CreatePopulation ();
		//Make every Chromosome fight
		//Calculate Fitness
		//Pick chromosomes to reproduce
		//Reproduce and replace population
	}

	void CreatePopulation(){
		population = new float[sizePopulation, sizeChromosomes];
		for (int i = 0; i < sizePopulation; i++) {
			float[] x = RandomChromosome ();
			for (int j = 0; j < x.Length; i++) {
				population [i, j] = x [j];
			}
		}
	}

	float[] RandomChromosome(){
		float[] chrom = new float[sizeChromosomes];
		for (int i = 0; i < sizeChromosomes; i++) {
			chrom[i] = Random.value;
		}
		return chrom;
	}
		 

	void ChooseChromosomes(){
		for (int i = 0; i < sizePopulation; i++) {
			fitness [i] = DefineFitness (i);
		}
	}

	float DefineFitness(int i){
		
		return 0f;
	}
		
}

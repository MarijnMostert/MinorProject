using UnityEngine;
using System.Collections;

public class GA {

	public float[,] population;
	public float[,] chromosomesToReproduce;
	public float[] fitness;

	public int sizePopulation;
	public int sizeChromosomes;
	public int NReproduce;

	public GA(int sizePopulation, int sizeChromosomes){
		this.sizePopulation = sizePopulation;
		this.sizeChromosomes = sizeChromosomes;
		this.fitness = new float[sizePopulation];
	}

	public float[] getChrom(int index){
		float[] temp = new float[sizeChromosomes] ;
		for (int i = 0; i < sizeChromosomes; i++) {
			temp [i] = population [index, i];
		}
		return temp;
	}


	// Use this for initialization
	void Start () {
		CreatePopulation ();
		//Make every Chromosome fight
		//Calculate Fitness
		//Pick chromosomes to reproduce
		//Reproduce and replace population
	}

	public void CreatePopulation(){
		Debug.Log ("Create population");
		population = new float[sizePopulation, sizeChromosomes];
		for (int i = 0; i < sizePopulation; i++) {
			float[] randchrom = RandomChromosome ();
			for (int j = 0; j < randchrom.Length; j++) {
				population [i, j] = randchrom[j];
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

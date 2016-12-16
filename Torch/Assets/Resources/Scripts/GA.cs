using UnityEngine;
using System.Collections;

public class GA {

	public float[,] population;
	public float[,] chromosomesToReproduce;
	public float[] fitness;

	public int numberOfGenerations;
	public int sizePopulation;
	public int sizeChromosomes;
	public int NReproduce;
	public float deviation;

	public GA(int numberOfGenerations, int sizePopulation, int sizeChromosomes){
		this.sizePopulation = sizePopulation;
		this.sizeChromosomes = sizeChromosomes;
		this.fitness = new float[sizePopulation];
		this.numberOfGenerations = numberOfGenerations;
		this.NReproduce = 5;
		this.deviation = 0.2f;
	}

	public float[] getChrom(int index){
		float[] temp = new float[sizeChromosomes] ;
		for (int i = 0; i < sizeChromosomes; i++) {
			temp [i] = population [index, i];
		}
		return temp;
	}

	//create a population with random chromosomes
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

	//create a random chromosome
	float[] RandomChromosome(){
		float[] chrom = new float[sizeChromosomes];
		for (int i = 0; i < sizeChromosomes; i++) {
			chrom[i] = Random.value;
		}
		return chrom;
	}
		 
	//choose a specified number of chromosomes
	int[] ChooseChromosomes(int N){
		float totalFitness = 0;
		float[] prob = new float[sizePopulation];
		float cumulTarget;
		float cumul;
		int j;
		int[] indices = new int[N];
	
		for (int i = 0; i < sizePopulation; i++) {
			totalFitness += fitness [i];
		}
		if (totalFitness == 0) {
			Debug.Log ("this generation is shit");
			Debug.Log ("We'll start over...");
			CreatePopulation ();
			return null;
		}
		for (int i = 0; i < sizePopulation; i++) {
			prob [i] = fitness [i] / totalFitness;
		}

		for (int i = 0; i < N; i++) {
			cumulTarget = Random.value;
			cumul = 0;
			j = -1;
			while (cumul < cumulTarget){
				j++;
				cumul += prob [j];
			}
			indices [i] = j;
		}
		return indices;
	}

	public void Reproduce(){
		int[] indices = ChooseChromosomes (NReproduce);
		if (indices != null) {
			chromosomesToReproduce = new float[NReproduce, sizeChromosomes];
			for (int i = 0; i < NReproduce; i++) {
				for (int j = 0; j < sizeChromosomes; j++) {
					chromosomesToReproduce [i, j] = population [indices [i], j];
				}
				Debug.Log ("Reproducing chromosome " + indices [i]);
			}
			Mutate ();
		}
	}
	

	void Mutate(){
		for (int i = 0; i < NReproduce; i++) {
			for (int k = 0; k < sizeChromosomes; k++) {
				population [i, k] = (normalDist (chromosomesToReproduce [i, k], deviation));
			}
		}
		deviation -= (deviation / numberOfGenerations);
	}

	float normalDist(float mean, float stdDev){
		float u1 = Random.value;
		float u2 = Random.value;
		float randStdNormal = Mathf.Sqrt (-2.0f * Mathf.Log (u1)) * Mathf.Sin (2.0f * Mathf.PI * u2);
		float randNormal = mean + stdDev * randStdNormal;
		return randNormal;
	}
}

using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public int mapMinX, mapMaxX;
	public int mapMinZ, mapMaxZ;
    public float min_radius;
    public float max_radius;

	public Enemy[] enemiesToSpawn;
	public int enemiesPerWave;

	private string waveButton;
	private string spawnEnemyButton;
    private GameObject torch;

    Floors floors;
	public float timeBetweenEnemySpawn = 3f;
    public bool dead;
//	private int rangeX, rangeZ;

	// Use this for initialization
	void Start () {
        dead = false;
		waveButton = "SpawnWave";
		spawnEnemyButton = "SpawnEnemy";
        torch = GameObject.FindGameObjectWithTag("Torch");
        StartCoroutine(SpawnEnemy());
    }
    

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown (spawnEnemyButton)) {
			spawnEnemy ();
		}
		if (Input.GetButtonDown (waveButton)) {
			spawnWave ();
		}
	}

	void spawnEnemy(){
		Enemy enemy = enemiesToSpawn [Random.Range (0, enemiesToSpawn.Length)];
		Vector3 position = getPosition ();
		Instantiate (enemy, position, transform.rotation);
	}

    Vector3 getPosition()
    {
        Vector3 player = torch.transform.position;
        float Random_radius = Random.Range(min_radius, max_radius);
        float Random_radial = Random.Range(0, 2 * Mathf.PI);
        Vector3 point = player + new Vector3(Random_radius * Mathf.Cos(Random_radial), 0, Random_radius * Mathf.Sin(Random_radial));
        if (validPosition(point.x, point.z)) return new Vector3(point.x, 1f, point.z);
        return getPosition();
    }

    bool inBounds(float x,float z)
    {
        return (mapMinX < x && x < mapMaxX) && (mapMinZ < z && z < mapMaxZ);
    }

    bool validPosition(float x, float z)
    {
        return floors.validPosition(x, z);
    }

    void spawnWave(){
		for (int i = 0; i < enemiesPerWave; i++) {
			spawnEnemy ();
		}
	}

    public void importMaze(bool[,] maze, int[] mazeSize)
    {
        Debug.Log("importMaze: " + mazeSize[0]+" "+mazeSize[1]); 
        floors = new Floors();
        floors.importFloorList(maze, mazeSize);
        Debug.Log(floors.print());
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            if (!dead)
            {
                spawnEnemy();
            }
			yield return new WaitForSecondsRealtime(timeBetweenEnemySpawn);

        }
    }
}

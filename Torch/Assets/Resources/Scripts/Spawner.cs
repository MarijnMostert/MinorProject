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
	public GameManager gameManager;

    Floors floors;
	public float timeTillSpawning = 5f;
	public float timeBetweenEnemySpawn;
    public bool dead = false;
	public bool activated = false;
//	private int rangeX, rangeZ;

	// Use this for initialization
	void Start () {
		gameManager = GameManager.Instance;
		timeBetweenEnemySpawn = 15.0f - (gameManager.dungeonLevel);
		if (timeBetweenEnemySpawn < 1.0f) {
			timeBetweenEnemySpawn = 1.0f;
		}
        dead = false;
		activated = false;
		waveButton = "SpawnWave";
		spawnEnemyButton = "SpawnEnemy";
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
		if (Input.GetKeyDown (KeyCode.M)) {
			if (dead) {
				dead = false;
				Debug.Log ("Spawner has been turned on");
			} else {
				dead = true;
				Debug.Log ("Spawner has been turned off");
			}
		}
	}

	public void spawnEnemy(){
		Enemy enemy = enemiesToSpawn [Random.Range (0, enemiesToSpawn.Length)];
		Vector3 position = getPosition ();
		Instantiate (enemy, position, transform.rotation);
	}

    Vector3 getPosition()
    {
		Vector3 player = gameManager.torch.transform.position;
        float Random_radius = Random.Range(min_radius, max_radius);
        float Random_radial = Random.Range(0, 2 * Mathf.PI);
        Vector3 point = player + new Vector3(Random_radius * Mathf.Cos(Random_radial), 0, Random_radius * Mathf.Sin(Random_radial));
		if (validPosition (point.x, point.z))
			return new Vector3 (point.x, 0, point.z);
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
		//yield return new WaitForSeconds (timeTillSpawning);
        while(true)
        {
			if (!dead && activated && !gameManager.paused)
            {
                spawnEnemy();
            }
			yield return new WaitForSecondsRealtime(timeBetweenEnemySpawn);

        }
    }
}

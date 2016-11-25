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
//	private int rangeX, rangeZ;

	// Use this for initialization
	void Start () {
		waveButton = "Spawn Wave Button";
		spawnEnemyButton = "Spawn Enemy Button";
        torch = GameObject.FindGameObjectWithTag("Torch").transform.parent.gameObject;

        //		rangeX = mapMaxX - mapMinX;
        //		rangeZ = mapMaxZ - mapMinZ;
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
		Instantiate (enemy, getPosition(), transform.rotation);
	}

    Vector3 getPosition()
    {
        Vector3 player = torch.transform.position;
        float Random_radius = Random.Range(min_radius, max_radius);
        float Random_radial = Random.Range(0, 2 * Mathf.PI);
        Vector3 point = player + new Vector3(Random_radius * Mathf.Cos(Random_radial), 0, Random_radius * Mathf.Sin(Random_radial));
        if (inBounds(point.x, point.z)) return new Vector3(point.x, 1f, point.z);
        return getPosition();
    }

    bool inBounds(float x,float z)
    {
        //Debug.Log("position " + x + " " + z);
        bool bol = (mapMinX < x && x < mapMaxX) && (mapMinZ < z && z < mapMaxZ);
        //Debug.Log("bool" + bol);

        return (mapMinX < x && x < mapMaxX) && (mapMinZ < z && z < mapMaxZ);
    }

    void spawnWave(){
		for (int i = 0; i < enemiesPerWave; i++) {
			spawnEnemy ();
		}
	}

}

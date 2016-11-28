using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class endPortal : MonoBehaviour {
    public string next_scene;
    bool toHome;
    [SerializeField]
    bool loading, start_game;
    [SerializeField]
    string scene;
    AsyncOperation async;
    GameObject continueText;
    Text winText;

    // Use this for initialization
    void Start () {
        loading = false;
        start_game = false;
        continueText = GameObject.Find("ContinueText") as GameObject;
        winText = GameObject.Find("UI").transform.FindChild("Win Text").GetComponent<Text>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("endportal");
            GameObject[] chests = GameObject.FindGameObjectsWithTag("chest");
            Debug.Log("chest length: "+ chests.Length);
            if (chests == null || chests.Length == 0)
            {
                onWin();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("."))
        {
            onWin();
        }

        if (toHome)
        {
            if (!loading)
            {
                StartCoroutine(LoadNewScene());
                loading = true;
            }
            if (async != null && loading && async.progress >= .9f)
            {
                if (Input.GetKeyUp("space"))
                {
                    async.allowSceneActivation = true;
                }
            }
        }
    }

    void onWin()
    {
        winText.gameObject.SetActive(true);
        GameObject.Find("Spawner").GetComponent<Spawner>().dead = true;
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            enemy.GetComponent<Enemy>().die();
        }
        Torch torch = GameObject.Find("Torch").GetComponent<Torch>();
        torch.onDead();
        toHome = true;
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(1);
        async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            yield return null;
        }
    }
}

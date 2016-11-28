using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour {
    bool loading,start_game;
    [SerializeField]
    string scene;
    [SerializeField]
    Text loadingText;
    AsyncOperation async;

    // Use this for initialization
    void Start () {
        loading = false;
        start_game = false;
    }

    // Update is called once per frame
    void Update () {
	    if(!loading)
        {
            StartCoroutine(LoadNewScene());
            loading = true;
            loadingText.text = "Loading...";
        }
        if (loading && async != null && !start_game)
        {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
            loadingText.text = "Loading... ("+async.progress.ToString()+"%)"; 
        }
        if (async.progress >= .9f)
        {
            loadingText.transform.localPosition = new Vector3(-1.4f, -29, 0);
            loadingText.text = "press space to continue";
            if (Input.GetKeyUp("space"))
            {
                start_game = true;
                loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b);
                loadingText.transform.localPosition = new Vector3(32, -29, 0);
                loadingText.text = "Starting game...";
                async.allowSceneActivation = true;
            }
        }

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

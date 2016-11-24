using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadLevel : MonoBehaviour {
    [SerializeField]
    Canvas canvas;
    bool loading = false;
    [SerializeField]
    int scene;
    [SerializeField]
    Text loadingText;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyUp(KeyCode.Space)&& !loading)
        {
            loading = true;
            loadingText.text = "Loading...";
            StartCoroutine(LoadNewScene());
        }
        if (loading)
        {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
	}

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(3);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
        {
            GameObject.Destroy(canvas);
            yield return null;
        }
    }
}

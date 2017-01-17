using UnityEngine;
using System.Collections;

public class Wallspike : MonoBehaviour, Trap {
    public GameObject projectile;
    bool player;

    public GameObject startMarker;
    public GameObject endMarker;
    public float speed = 50000.0F;
    private float startTime;
    private float journeyLength;
    bool shoot;

    private float startOffset;

    // Use this for initialization
    void Start () {
        player = false;
        shoot = false;
        startOffset = Random.Range(.1f,3f);
        startTime = Time.time;
        projectile.transform.position = startMarker.transform.position;
        journeyLength = Vector3.Distance(startMarker.transform.position, endMarker.transform.position);
    }

    // Update is called once per frame
    void Update () {
        if (shoot)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            projectile.transform.position = Vector3.Lerp(startMarker.transform.position, endMarker.transform.position, fracJourney);
        }
    }

    public void trigger()
    {
        player = true;
        if (!shoot)
        {
            StartCoroutine(shootProjectile());
        }
    }

    public void endtrigger()
    {
        player = false;
    }

    private IEnumerator shootProjectile()
    {
        yield return new WaitForSecondsRealtime(startOffset);
        shoot = true;
        startTime = Time.time;

        yield return new WaitForSecondsRealtime(1.5f);
        shoot = false;
        if (player)
        {
            StartCoroutine(shootProjectile());
        }
        yield return null;   
    }
}

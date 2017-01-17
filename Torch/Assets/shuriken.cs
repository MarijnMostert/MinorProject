using UnityEngine;
using System.Collections;

public class shuriken : MonoBehaviour, Trap
{
    public GameObject projectile;
    bool player;

    public GameObject startMarker;
    public GameObject endMarker;
    public float speed = 50000.0F;
    private float startTime;
    private float journeyLength;
    bool shoot;
    bool direct;

    private float startOffset;

    // Use this for initialization
    void Start()
    {
        player = false;
        direct = false;
        startOffset = .1f;
        startTime = Time.time;
        projectile.transform.position = startMarker.transform.position;
        journeyLength = Vector3.Distance(startMarker.transform.position, endMarker.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot && direct)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            projectile.transform.Rotate(Vector3.forward *90* Time.deltaTime);
            projectile.transform.position = Vector3.Lerp(startMarker.transform.position, endMarker.transform.position, fracJourney);
        }
        else if (shoot && !direct)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            projectile.transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
            projectile.transform.position = Vector3.Lerp(endMarker.transform.position, startMarker.transform.position, fracJourney);
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
        direct = true;
        startTime = Time.time;

        yield return new WaitForSecondsRealtime(1.5f);
        direct = false;
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

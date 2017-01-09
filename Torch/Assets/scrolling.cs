using UnityEngine;
using System.Collections;

public class scrolling : MonoBehaviour
{


    public float scrollx = 0.20f;
    public float scrolly = 0.20f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {

        var offset = Time.time * scrollx;
        var offset2 = Time.time * scrolly;
        rend.material.SetTextureOffset("_MainTex",new Vector2(offset,offset2));
    }
}

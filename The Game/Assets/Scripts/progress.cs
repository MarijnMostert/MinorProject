using UnityEngine;
using System.Collections;

public class progress : MonoBehaviour {
    float deltaPos;
    float deltaScale;
    RectTransform rect_trans;

	// Use this for initialization
	void Start () {
        float start_scale_x = 0.0f;
        float start_pos = -158.6f;
        float max_scale_x = 2.810733f;
        float max_pos = -26.5f;
        rect_trans = GetComponent<RectTransform>();
        rect_trans.anchoredPosition = new Vector3(start_pos, -55.5f, 0);
        rect_trans.localScale= new Vector3(start_scale_x, 0.2035986f, 1);

        deltaScale = max_scale_x - start_scale_x;
        deltaPos = max_pos - start_pos;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateProgress(float deltaPercentage)
    {
        rect_trans.localPosition = new Vector3(deltaPercentage*deltaPos+rect_trans.localPosition.x
            ,rect_trans.localPosition.y,rect_trans.localPosition.z);
        rect_trans.localScale = new Vector3(deltaPercentage*deltaScale+rect_trans.localScale.x
            ,rect_trans.localScale.y,rect_trans.localScale.z);
    }
}

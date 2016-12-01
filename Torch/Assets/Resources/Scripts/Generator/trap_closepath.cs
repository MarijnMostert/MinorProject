using UnityEngine;
using System.Collections;

public class trap_closepath : MonoBehaviour {
    public GameObject block;
    Vector3 pos, block_pos, block_rot, opp_block_pos, opp_block_rot, rot;
    float step;

	// Use this for initialization
	void Start () {
        step = 2f;
        pos = transform.position;
        rot = transform.rotation.eulerAngles;
        block_rot = Vector3.Cross(rot, new Vector3(0, 1, 0));
        block_pos = block_rot * step;
        opp_block_rot = -block_rot;
        opp_block_pos = -block_pos;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void createWalls()
    {
        for(int i = 0; i < 7; i++)
        {
            Instantiate(block, block_pos, Quaternion.LookRotation(block_rot));
            Instantiate(block, opp_block_pos, Quaternion.LookRotation(opp_block_rot));
        }
    }
}

﻿using UnityEngine;
using System.Collections;

public class toggleTrapBox : MonoBehaviour {
    public GameObject centerFloor;
    bool toggle;
    float diffPos, speed;
    Vector3 Target_pos,org_pos,end_pos,Target_scale, org_scale,end_scale;

	// Use this for initialization
	void Start () {
        toggle = false;
        diffPos = 2f;
        speed = .7f;
        Vector3 direction = determineDirection();
        org_pos = transform.position - direction*getAngle(direction)*0.05f;
        end_pos = transform.position + direction * diffPos;
        transform.position = org_pos;
        Target_pos = org_pos;

        org_scale = transform.localScale;
        end_scale= new Vector3(1, 1, 1) + new Vector3(3f,0,0);
        Debug.Log(name + " end_scale" + end_scale.x + " " + end_scale.y);
        Target_scale = org_scale;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("t")) {
            toggleTrap();
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target_pos, step);
        transform.localScale = Vector3.Lerp(transform.localScale,Target_scale,step);
	}

    void toggleTrap() {
        if (toggle) {
            Target_pos = org_pos;
            Target_scale = org_scale;
        } else {
            Target_pos = end_pos;
            Target_scale = end_scale;
        }
        toggle = !toggle;
    }

    float getAngle(Vector3 dir)
    {
        return Vector3.Angle(getDiff(),dir);
    }

    Vector3 determineDirection()
    {
        Vector3 diff = getDiff();
        if (diff.x>diff.z && diff.x>-diff.z) {
            return new Vector3(1, 0, 0);
        } else if (diff.x < diff.z && diff.x > -diff.z)  {
            return new Vector3(0, 0, 1);
        } else if (diff.x < diff.z && -diff.x > diff.z) {
            return new Vector3(-1, 0, 0);
        } else {
            return new Vector3(0, 0, -1);
        }
    }

    Vector3 getDiff()
    {
        return centerFloor.transform.position - transform.position;
    }
}
using UnityEngine;
using System.Collections;

public class toggleTrapBox : MonoBehaviour {
    int num_blocks, blocks_ready;
    Block[] block_scripts;
    float close_time;
    bool ready;

    void Start()
    {
        close_time = 2f;
        num_blocks = 10;
        blocks_ready = 0;
        ready = false;
        block_scripts = transform.parent.GetComponentsInChildren<Block>();
        setTriggerSiblings();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ToggleSiblings();
        }
    }

    public void setTrapReady()
    {
        blocks_ready++;
        //Debug.Log("Blocks ready: "+blocks_ready);
        if (blocks_ready == num_blocks)
        {
            blocks_ready = 0;
            ready = true;
        }
    }

    void ToggleSiblings()
    {
        foreach (Block block in block_scripts)
        {
            block.toggleTrap();
        }
    }

    void setTriggerSiblings()
    {
        foreach (Block block in block_scripts)
        {
            block.setTrigger(gameObject);
        }
    }

    void Update()
    {
        if (ready)
        {
            close_time -= Time.deltaTime;
            if (close_time < 0)
            {
                ToggleSiblings();
                ready = false;
                close_time = 2f;
            }
        }
    }
}


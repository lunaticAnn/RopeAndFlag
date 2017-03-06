using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRope : MonoBehaviour {
    const int SUBDIV = 30;
    const float THREAD = 0.1f;
    //start is a fixed position where the rope fixed to
    public GameObject start;

    public GameObject end;

    private LineRenderer rend;
    private Vector3[] my_rope_nodes;
    private Vector3 last_position;
    private float rope_dis;

	void Start () {
        rend = GetComponent<LineRenderer>();
        instantiate_rope();
	}
	
	// Update is called once per frame
	void Update () {

        if (start.transform.position != last_position)
        {
            my_rope_nodes[0] = start.transform.position;
            update_my_rope();
            end.transform.position = my_rope_nodes[SUBDIV-1];
        }
        last_position = start.transform.position;
    }

    void instantiate_rope() {
        rend.numPositions = SUBDIV;
        my_rope_nodes = new Vector3[SUBDIV]; 
        int i;
        //simple bilinear interpolation
        for (i = 0; i < SUBDIV; i++)
        {
            Vector3 tmp = ((SUBDIV - 1 - i) * start.transform.position + i * end.transform.position) / (SUBDIV - 1);
            if (Vector3.Distance(tmp, my_rope_nodes[i]) > THREAD)
                my_rope_nodes[i] = tmp;        
        }
        rend.SetPositions(my_rope_nodes);
        rope_dis = Vector3.Distance(my_rope_nodes[0], my_rope_nodes[SUBDIV - 1]) / (SUBDIV - 1);
    }

    /*---------Rope UPdate Function----------------------------------------------------
     * Drag the rope from a place to another place will update of rope
     *    dis = node[x+1] - node[x]
     *    node[x+1] = (node[x+2] - node[x]).norm() * dis + node[x];   
     * No spring first  
     ----------------------------------------------------------------------------------*/
    void update_my_rope(bool start_move = true) {
        int i = 1;
        Vector3 dir;
        while (i < SUBDIV -1) {
            dir = (my_rope_nodes[i + 1] - my_rope_nodes[i-1]).normalized;
            my_rope_nodes[i] = my_rope_nodes[i - 1] + dir * rope_dis;
            i++;  
        }

        dir = (my_rope_nodes[i] - my_rope_nodes[i - 1]).normalized;
        my_rope_nodes[i] = my_rope_nodes[i-1] + rope_dis *dir; 
        rend.SetPositions(my_rope_nodes);
    }
}

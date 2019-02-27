using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class position_generator : MonoBehaviour
{
    public GameObject move_position_prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void show()
    {
        GameObject position_show = Instantiate(move_position_prefab) as GameObject;
        position_show.transform.position = new Vector3(125, 0, 125);
    }
}

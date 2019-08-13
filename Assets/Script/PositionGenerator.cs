using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionGenerator : MonoBehaviour
{
    public GameObject move_position_prefab;
    private GameObject Plane;

    public void Move_Possible_Plane(int plane_position_x, int plane_position_z)
    {
        Plane = Instantiate(move_position_prefab) as GameObject;
        Plane.transform.position = new Vector3(125 * plane_position_x, 0, -125 * plane_position_z);
    }

    public void Destroy()
    {
        GameObject[] Tagobjs = GameObject.FindGameObjectsWithTag("TargetTag");
        foreach (GameObject Obj in Tagobjs)
            Destroy(Obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_pawn_generator : MonoBehaviour
{
    public GameObject white_pawn_prefab;
    private int body_num = 8;

    void Start()
    {
        for (int x = 0; x < body_num; x++)
        {
            GameObject x_position = Instantiate(white_pawn_prefab) as GameObject;
            x_position.transform.position = new Vector3(x * 125, 0, -125*6);
        }
    }
}

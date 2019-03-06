using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_pawn_generator : MonoBehaviour
{
    public GameObject black_pawn_prefab;
    private int body_num = 8;

    void Start()
    {
        for (int x = 0; x < body_num; x++)
        {
            GameObject x_position = Instantiate(black_pawn_prefab) as GameObject;
            x_position.transform.position = new Vector3(x * 125, 0, -125);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

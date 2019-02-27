using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class white_pawn : MonoBehaviour
{
    private int move_length = 125;

    void Start()
    {
    }

    void Update()
    {
    }
    

    public void special_move()
    {
        GetComponent<AudioSource>().Play();
        transform.Translate(0, 0, move_length * 2);
    }

    public void attack_move()
    {
        GetComponent<AudioSource>().Play();
        //右に攻撃
        transform.Translate(move_length, 0, move_length);
        //左に攻撃
        transform.Translate(-move_length, 0, move_length);
    }

    public void normal_move()
    {
        GetComponent<AudioSource>().Play();
        transform.Translate(0, 0, move_length);
    }
}

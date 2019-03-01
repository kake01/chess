using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class position_generator : MonoBehaviour
{
    public GameObject move_position_prefab;
    //public GameObject is_select_chessman;

    //                        position_show[i] = Instantiate(move_position_prefab) as GameObject;
    //    position_show[i].transform.position = new Vector3(125 * i, 0, -625);

    void Start()
    {
        //int temp = position_show.board_2D_array;
        //Debug.Log(temp);
    }

    void Update()
    {
    }

    //generatorが持っているのっはおかしい
    //pawnが移動出来る場所のshow
    public void show(int[,] board_2D_array, int select_x, int select_y/*盤面の情報 2次元 select_x select_y*/)
    {
        /*
          * もらっと配列から移動出来る場所
         //配列が必要な場合
        GameObject[] position_show = new GameObject[5];
        for (var i = 0; i < 3; i++)
        {
            position_show[i] = Instantiate(move_position_prefab) as GameObject;
            position_show[i].transform.position = new Vector3(125 * i, 0, -625);
            //Debug.Log(this.position_show);
        }*/
    }
}

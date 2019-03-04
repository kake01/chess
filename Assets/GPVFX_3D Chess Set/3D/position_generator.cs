using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class position_generator : MonoBehaviour
{
    public GameObject move_position_prefab;
    GameObject show;


    public void move_possible_plane(int show_position_x, int show_position_y)
    {
        show = Instantiate(move_position_prefab) as GameObject;
        show.transform.position = new Vector3(125 * show_position_x, 0, -125 * show_position_y);
    }

    public void destroy()
    {
        GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("TargetTag");
        Debug.Log(tagobjs.Length);
        foreach (GameObject obj in tagobjs)
        {
            Destroy(obj);
        }
        //        destroy();
        //        Debug.Log(show.Length);
    }

    /*
public void show(int[,] board_2D_array, int select_x, int select_y 盤面の情報 2次元 select_x select_y)
{
       もらっと配列から移動出来る場所
    //配列が必要な場合
    GameObject[] position_show = new GameObject[5];
    for (var i = 0; i< 3; i++)
    {
        position_show[i] = Instantiate(move_position_prefab) as GameObject;
        position_show[i].transform.position = new Vector3(125 * i, 0, -625);}
        }
            //public GameObject is_select_chessman;
    //position_show[i] = Instantiate(move_position_prefab) as GameObject;
    //position_show[i].transform.position = new Vector3(125 * i, 0, -625);

        */
}

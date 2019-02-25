using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clic_manager : MonoBehaviour
{
    public bool is_select_time = false;

    private int select_x, select_z;
    private int cell_length = 125;
    //    private bool turn = true;
    private int[,] board_2D_array =
    {
        {-1, -2, -3, -4, -5, -3, -2, -1},
        {-6, -6, -6, -6, -6, -6, -6, -6},
        { 0,  0,  0,  0,  0,  0,  0,  0},
        { 0,  0,  0,  0,  0,  0,  0,  0},
        { 0,  0,  0,  0,  0,  0,  0,  0},
        { 0,  0,  0,  0,  0,  0,  0,  0},
        { 6,  6,  6,  6,  6,  6,  6,  6},
        { 1,  2,  3,  4,  5,  3,  2,  1},
    };

    void Start()
    {
        Debug.Log(board_2D_array[0, 0]);
        Debug.Log(board_2D_array[0, 1]);
    }

    void Update()
    {
        //選択中以外でマウスが押されたら
        if (Input.GetMouseButtonDown(0) && is_select_time == false)
        {
            //カメラから光線を飛ばす準備
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                is_select_time = !is_select_time;
                hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;

                //getしたオブジェクトの座標を取得
                Vector3 worldDir = hit.transform.position;
                select_x = (int)worldDir.x / cell_length;
                select_z = (int)worldDir.z / cell_length;
                                Debug.Log("座標xは" + worldDir.x + "座標zは" + worldDir.z);
                              Debug.Log("座標select_xは" + select_x + "座標select_zは" + select_z);


                if (board_2D_array[select_x, select_z + 1] == 0)
                {
                    board_2D_array[select_x, select_z] = 0;
                    board_2D_array[select_x, select_z + 1] = 6;
                    select_z++;
                    hit.collider.gameObject.GetComponent<white_pawn>().normal_move();
                }
                //            hit.collider.gameObject.GetComponent<white_pawn>().normal_move();


                //hit.collider.gameObject.GetComponent<white_pawn>().special_move();
                is_select_time = !is_select_time;
                /* 追記する内容1
                 * 選択されたオブジェクトが移動出来るなら移動範囲を表示
                 * 
                 * 1,移動出来る場所をクリックしたら、
                 *      座標変更　&& もし敵がいたら、駒のデリート
                 *      is_select_time =! is_select_time;
                 * 2,選択外をクリックしたら
                 *      is_select_time =! is_select_time;
                 *  
                 *
                 */
                //  if (hit.collider.gameObject.CompareTag(chessman_tag))
            }
        }
    }
}

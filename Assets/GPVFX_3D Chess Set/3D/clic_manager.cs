using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clic_manager : MonoBehaviour
{
    public bool is_select_time = false;
    private int select_x, select_y;
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
        { 1,  2,  3,  4,  5,  3,  2,  1}
    };
    //    GameObject position;

    void Start()
    {
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
                select_y = -(int)worldDir.z / cell_length;
                select_x = (int)worldDir.x / cell_length;


                //ポーンの場合
                if (board_2D_array[select_y, select_x] == 6 && -1 < select_y - 1)
                {
                    if (board_2D_array[select_y - 1, select_x] == 0)
                    {
                        /* 選択されている状態の命令(関数で書く)
                          this.position = GameObject.Find("generator");
                          this.position.GetComponent<position_generator>().show();
                          this.position.GetComponent<position_generator>().destroy();
                        */

                        /*移動する場合の座標変更
                         * board_2D_array[select_y, select_x] = 0;
                         * board_2D_array[select_y - 1, select_x] = 6;
                         * hit.collider.gameObject.GetComponent<white_pawn>().normal_move();
                         * is_select_time = !is_select_time;
                         */
                        //カメラから光線を飛ばす準備
                        //Ray ray = new Ray();
                        //RaycastHit hit = new RaycastHit();
                        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    }
                }
            }
        }
    }

    void is_select_time_func()
    {
    }
}

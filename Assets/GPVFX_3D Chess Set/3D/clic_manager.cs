using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clic_manager : MonoBehaviour
{
    public bool is_select_time = false;
    private int select_x, select_y;
    private int cell_length = 125;
    //    private bool turn = true;
    //    private GameObject move_position_prefab;


    //クリックmanagerが盤面の情報を引数にして、駒のshowを呼び出す
    void Start()
    {

    }



    //駒が選択され

    void Update()
    {
        //選択中以外でマウスが押されたら
        if (Input.GetMouseButtonDown(0) && !is_select_time)
        {
            //カメラから光線を飛ばす準備
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                is_select_time = true;
                hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                //getしたオブジェクトの座標を取得
                Vector3 worldDir = hit.transform.position;
                select_y = -(int)worldDir.z / cell_length;
                select_x = (int)worldDir.x / cell_length;


                //ポーンの場合
                if (0 <= (select_y - 1))
                {
//                    get_board_2D_array();

                    switch (board_2D_array[select_y, select_x])
                    {
                        case NUM_PAWN:
                            //ｐａｗｎが移動出来る場所を表示するメソッドを呼ぶ
                            show(get_board_2D_array(), select_x, select_y/*盤面の情報 2次元 select_x select_y*/);
                            break;
                        case NUM_ROOK:
                            //ｐａｗｎが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case NUM_KNIGHT:
                            //ｐａｗｎが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case NUM_BISHOP:
                            //ｐａｗｎが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case NUM_QUEEN:
                            //ｐａｗｎが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        case NUM_KING:
                            //ｐａｗｎが移動出来る場所を表示するメソッドを呼ぶ
                            break;
                        default:
                            break;
                    }



                    //clickマネジメントが座標行進をボードに伝えるupデート



                    /*
                    void 
                    if (board_2D_array[select_y - 1, select_x] == 0)
                    {
                        //移動出来る場所の表示

                        GameObject position_show = Instantiate(move_position_prefab) as GameObject;
                        //                        position_show[i] = Instantiate(move_position_prefab) as GameObject;
                        position_show[i].transform.position = new Vector3(125 * i, 0, -625);

                        */
                    /*移動する場合の座標変更
                     * board_2D_array[select_y, select_x] = 0;
                     * board_2D_array[select_y - 1, select_x] = 6;
                     * hit.collider.gameObject.GetComponent<white_pawn>().normal_move();
                     * is_select_time = false;
                     */

                    //選択状態で移動出来る場所かの検知
                    //カメラから光線を飛ばす準備
                    //Ray ray = new Ray();
                    //RaycastHit hit = new RaycastHit();
                    //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                }
            }
        }
    }
}
/*
void is_select_time_func()
{
}
*/

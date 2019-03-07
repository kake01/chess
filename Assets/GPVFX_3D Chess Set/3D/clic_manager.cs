using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clic_manager : MonoBehaviour
{
    private RaycastHit first_hit;
    private const int cell_length = 125;
    private bool is_select_time = false;
    private int select_x, select_y, move_pos_x, move_pos_y;
    private int turn = 1;
    private Color color;
    private GameObject plane;
    public GameObject board;
    public int[,] board_state;

    void Start()
    {
        this.board = GameObject.Find("board");
        this.plane = GameObject.Find("generator");
        this.board_state = board.GetComponent<board>().get_board_2D_array();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            board_state = board.GetComponent<board>().get_board_2D_array();
            if (!is_select_time)
            {
                //カメラから光線を飛ばす準備
                Ray first_ray = new Ray();
                first_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                first_hit = new RaycastHit();

                //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
                if (Physics.Raycast(first_ray.origin, first_ray.direction, out first_hit, Mathf.Infinity))
                {
                    Vector3 worldDir = first_hit.transform.position;

                    //boardの二次元座標に変換
                    select_y = -(int)worldDir.z / cell_length;
                    select_x = (int)worldDir.x / cell_length;
                    //掴んだ駒の色を保持する
                    color = first_hit.collider.gameObject.GetComponent<Renderer>().material.color;
                    first_hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;

                    //click_managerが盤面の情報を引数にして、駒の移動範囲のshowを呼び出す
                    switch (board_state[select_y, select_x] * turn)
                    {
                        case 6:
                            //PAWNが移動出来る場所を表示するメソッドを呼ぶ
                            first_hit.collider.gameObject.GetComponent<pawn>().move_possible_show(board_state, select_x, select_y, turn);
                            break;
                        case 1:
                            first_hit.collider.gameObject.GetComponent<rook>().move_possible_show(board_state, select_x, select_y, turn);
                            break;
                        case 2:
                            first_hit.collider.gameObject.GetComponent<knight>().move_possible_show(board_state, select_x, select_y, turn);
                            break;
                        case 3:
                            first_hit.collider.gameObject.GetComponent<bishop>().move_possible_show(board_state, select_x, select_y, turn);
                            break;
                        case 4:
                            first_hit.collider.gameObject.GetComponent<queen>().move_possible_show(board_state, select_x, select_y, turn);
                            break;
                        case 5:
                            first_hit.collider.gameObject.GetComponent<king>().move_possible_show(board_state, select_x, select_y, turn);
                            break;
                        default:
                            break;
                    }
                    is_select_time = true;
                }
            }
            else
            {
                Ray second_ray = new Ray();
                RaycastHit second_hit = new RaycastHit();
                second_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //マウスクリックした場所にRayを飛ばし、オブジェクトがあればtrue
                if (Physics.Raycast(second_ray.origin, second_ray.direction, out second_hit, Mathf.Infinity))
                {
                    //当たったオブジェクトが生成した板だったら
                    if (second_hit.collider.tag == "TargetTag")
                    {
                        Vector3 plan = second_hit.transform.position;
                        move_pos_y = -(int)plan.z / cell_length;
                        move_pos_x = (int)plan.x / cell_length;
                        //click_managerが盤面の情報を引数にして、駒の移動範囲のshowを呼び出す
                        switch (board_state[select_y, select_x] * turn)
                        {
                            case 6:
                                first_hit.collider.gameObject.GetComponent<pawn>().chessman_move(move_pos_x, move_pos_y);
                                break;
                            case 1:
                                first_hit.collider.gameObject.GetComponent<rook>().chessman_move(move_pos_x, move_pos_y);
                                break;
                            case 2:
                                first_hit.collider.gameObject.GetComponent<knight>().chessman_move(move_pos_x, move_pos_y);
                                break;
                            case 3:
                                first_hit.collider.gameObject.GetComponent<bishop>().chessman_move(move_pos_x, move_pos_y);
                                break;
                            case 4:
                                first_hit.collider.gameObject.GetComponent<queen>().chessman_move(move_pos_x, move_pos_y);
                                break;
                            case 5:
                                first_hit.collider.gameObject.GetComponent<king>().chessman_move(move_pos_x, move_pos_y);
                                break;
                            default:
                                break;
                        }
                        //座標の更新
                        board_state[move_pos_y, move_pos_x] = board_state[select_y, select_x];
                        board_state[select_y, select_x] = 0;
                        turn *= -1;
                    }
                }
                plane.GetComponent<position_generator>().destroy();
                first_hit.collider.gameObject.GetComponent<Renderer>().material.color = color;
                is_select_time = false;
            }
        }
    }

    public int[,] board_state_update()
    {
        return board_state;
    }
}
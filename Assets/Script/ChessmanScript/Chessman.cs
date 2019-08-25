using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    protected PhotonView photonView;
    protected Color is_select_color;
    protected List<Vector2> exist_chessman;
    public int pos_x, pos_y;
    public int[,] conditions;
    private int[,] search_condition;
    private Vector2[] find_pos;
    public Vector2[] move_pos;

    //boardが持つべきもの
    protected GameObject Plane;

    //chessmanには不要なもの
    protected const int MOVEMENT = 1;
    protected const int VECTER_ZERO = 0;
    protected const int CELL_MIN = 0;
    protected const int CELL_MAX = 7;
    protected const int CELL_LENGTH = 125;
    protected const int NUM_NOT_CHESSMAN = 0;
    protected const int NO_CONDITION = 4;

    public Chessman()
    {

    }

    public virtual void Start()
    {
        this.Plane = GameObject.Find("Generator");
        this.exist_chessman = new List<Vector2>();
        this.photonView = GetComponent<PhotonView>();
        this.pos_y = -(int)transform.position.z / CELL_LENGTH;
        this.pos_x = (int)transform.position.x / CELL_LENGTH;
        this.find_pos = new Vector2[] {
            new Vector2(0, -1),//↑
            new Vector2(1, -1),//,↗
            new Vector2(1, 0),//→
            new Vector2(1, 1),//↘
            new Vector2(0, 1),//↓
            new Vector2(-1, 1),//↙
            new Vector2(-1, 0),//←
            new Vector2(-1, -1)//↖
        };
    }

    public virtual void ChessmanMove(int move_x, int move_y)
    {
        this.transform.position = new Vector3(CELL_LENGTH * move_x, 0, -CELL_LENGTH * move_y);
        this.pos_x = move_x;
        this.pos_y = move_y;
    }

    public virtual List<Vector2> FindChessman(int turn, int[,] board_state)
    {
        this.search_condition = new int[,]
        {
            //↑,↗,→,↘,↓,←,↙,↖,,↗
            {pos_y - 1, NO_CONDITION, NO_CONDITION, NO_CONDITION},//↑
            {pos_y - 1, pos_x + 1, NO_CONDITION, NO_CONDITION},//,↗
            {NO_CONDITION, pos_x + 1, NO_CONDITION, NO_CONDITION},//→
            {NO_CONDITION, pos_x + 1, pos_y + 1, NO_CONDITION},//↘
            {NO_CONDITION, NO_CONDITION, pos_y + 1, NO_CONDITION},//↓
            {NO_CONDITION, NO_CONDITION, pos_y + 1, pos_x - 1},//↙
            {NO_CONDITION, NO_CONDITION, NO_CONDITION, pos_x - 1},//←
            {pos_y - 1, NO_CONDITION, NO_CONDITION, pos_x - 1}//↖
        };

        exist_chessman.Clear();
        //移動範囲の数だけ
        for (int chess_num = 0; chess_num < find_pos.Length; chess_num++)
        {
            if (turn * board_state[pos_y, pos_x] > NUM_NOT_CHESSMAN)
            {
                if (CELL_MIN <= search_condition[chess_num, 0] && search_condition[chess_num, 1] <= CELL_MAX && search_condition[chess_num, 2] <= CELL_MAX && CELL_MIN <= search_condition[chess_num, 3])
                {
                    if (turn * board_state[pos_y + (int)find_pos[chess_num].y, pos_x + (int)find_pos[chess_num].x] < NUM_NOT_CHESSMAN)
                        exist_chessman.Add(new Vector2(pos_x + (int)find_pos[chess_num].x, pos_y + (int)find_pos[chess_num].y));
                }
            }
        }
        return exist_chessman;
    }

    public virtual void ChessmanDestroy(int destroy_pos_x, int destroy_pos_y)
    {
        if (this.pos_x == destroy_pos_x && this.pos_y == destroy_pos_y)
            PhotonNetwork.Destroy(this.gameObject);
    }
    public virtual void PlaneDestroy() => Plane.GetComponent<PositionGenerator>().Destroy();

    public virtual void ResetColor() => this.GetComponent<Renderer>().material.color = is_select_color;

    //描画関係
    public virtual void MovePossibleShow(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        is_select_color = GetComponent<Renderer>().material.color;
        this.GetComponent<Renderer>().material.color = Color.red;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    public virtual void NoneRenderer() => photonView.RPC("FalseRenderer", PhotonTargets.Others);
    public virtual void DisplayRenderer(Vector2 disp_pos) => photonView.RPC("TrueRender", PhotonTargets.Others, disp_pos);
    [PunRPC]
    public void FalseRenderer() => this.GetComponent<Renderer>().enabled = false;
    [PunRPC]
    public void TrueRender(Vector2 render_pos)
    {
        pos_y = -(int)this.transform.position.z / CELL_LENGTH;
        pos_x = (int)this.transform.position.x / CELL_LENGTH;
        if (pos_x == render_pos.x && this.pos_y == render_pos.y)
            this.GetComponent<Renderer>().enabled = true;
    }
}
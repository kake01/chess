using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    private int board_num;
    private const int NEXT_CELL = 1;
    private const int INIT_CELL_NUM = 1;
    private const int TRIAL_NUM = 8;

    public override void MovePossibleShow(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        base.MovePossibleShow(board_state, is_position_x, is_position_y, turn);

        for (int chess_num = 0; chess_num < TRIAL_NUM; chess_num++)
        {
            board_num = 1;

            while (CELL_MIN <= 8 - board_num)
            {
                this.conditions = new int[,]
                {
                    {is_position_y - board_num, NO_CONDITION, NO_CONDITION, NO_CONDITION},//↑
                    {is_position_y - board_num, is_position_x + board_num, NO_CONDITION, NO_CONDITION},//,↗
                    {NO_CONDITION, is_position_x + board_num, NO_CONDITION, NO_CONDITION},//→
                    {NO_CONDITION, is_position_x + board_num, is_position_y + board_num, NO_CONDITION},//↘
                    {NO_CONDITION, NO_CONDITION, is_position_y + board_num, NO_CONDITION},//↓
                    {NO_CONDITION, NO_CONDITION, is_position_y + board_num, is_position_x - board_num},//↙
                    {NO_CONDITION, NO_CONDITION, NO_CONDITION, is_position_x - board_num},//←
                    {is_position_y - board_num, NO_CONDITION, NO_CONDITION, is_position_x - board_num}//↖
                };
                this.move_pos = new Vector2[] 
                {
                    new Vector2(VECTER_ZERO, -board_num),//↑
                    new Vector2(board_num, -board_num),//,↗
                    new Vector2(board_num, VECTER_ZERO),//→
                    new Vector2(board_num, board_num),//↘
                    new Vector2(VECTER_ZERO, board_num),//↓
                    new Vector2(-board_num, board_num),//↙
                    new Vector2(-board_num, VECTER_ZERO),//←
                    new Vector2(-board_num, -board_num)//↖
                };

                //条件↑,→,↓,←の順番
                if (CELL_MIN <= conditions[chess_num, 0] && conditions[chess_num, 1] <= CELL_MAX && conditions[chess_num, 2] <= CELL_MAX && CELL_MIN <= conditions[chess_num, 3])
                {
                    //移動可能場所に板を設置
                    if (turn * board_state[is_position_y + (int)move_pos[chess_num].y, is_position_x + (int)move_pos[chess_num].x] <= NUM_NOT_CHESSMAN)
                        Plane.GetComponent<PositionGenerator>().Move_Possible_Plane(is_position_x + (int)move_pos[chess_num].x, is_position_y + (int)move_pos[chess_num].y);
                    //敵の駒があったら
                    if (turn * board_state[is_position_y + (int)move_pos[chess_num].y, is_position_x + (int)move_pos[chess_num].x] < NUM_NOT_CHESSMAN)
                        break;
                    //自分の駒があったら
                    if (turn * board_state[is_position_y + (int)move_pos[chess_num].y, is_position_x + (int)move_pos[chess_num].x] > NUM_NOT_CHESSMAN)
                        break;
                }
                board_num++;
            }
        }
    }
}
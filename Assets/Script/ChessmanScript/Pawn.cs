using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Chessman
{
    private bool is_first_move = true;
    private readonly int NORMAL_MOVEMENT = 1;
    private readonly int SPECIAL_MOVEMENT = 2;

    public override void ChessmanMove(int move_x, int move_z)
    {
        base.ChessmanMove(move_x, move_z);
        if (is_first_move)
            is_first_move = false;
    }

    public override void MovePossibleShow(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        base.MovePossibleShow(board_state, is_position_x, is_position_y, turn);

        //初動のみ有効
        if (is_first_move && board_state[is_position_y - SPECIAL_MOVEMENT * turn, is_position_x] == NUM_NOT_CHESSMAN && board_state[is_position_y - NORMAL_MOVEMENT * turn, is_position_x] == NUM_NOT_CHESSMAN)
            Plane.GetComponent<PositionGenerator>().Move_Possible_Plane(is_position_x, is_position_y - SPECIAL_MOVEMENT * turn);

        if ((CELL_MIN <= is_position_y - NORMAL_MOVEMENT) && (is_position_y + NORMAL_MOVEMENT <= CELL_MAX))
        {
            //前に駒がなかったら
            if (board_state[is_position_y - NORMAL_MOVEMENT * turn, is_position_x] == NUM_NOT_CHESSMAN)
            {
                Plane.GetComponent<PositionGenerator>().Move_Possible_Plane(is_position_x, is_position_y - NORMAL_MOVEMENT * turn);
            }
            //左に駒が合ったら
            if (CELL_MIN <= is_position_x - NORMAL_MOVEMENT && turn * board_state[is_position_y - NORMAL_MOVEMENT * turn, is_position_x - NORMAL_MOVEMENT] < NUM_NOT_CHESSMAN)
            {
                Plane.GetComponent<PositionGenerator>().Move_Possible_Plane(is_position_x - NORMAL_MOVEMENT, is_position_y - NORMAL_MOVEMENT * turn);
            }
            //右に駒が合った
            if (is_position_x + NORMAL_MOVEMENT <= CELL_MAX && turn * board_state[is_position_y - NORMAL_MOVEMENT * turn, is_position_x + NORMAL_MOVEMENT] < NUM_NOT_CHESSMAN)
            {
                Plane.GetComponent<PositionGenerator>().Move_Possible_Plane(is_position_x + NORMAL_MOVEMENT, is_position_y - NORMAL_MOVEMENT * turn);
            }
        }
    }
}

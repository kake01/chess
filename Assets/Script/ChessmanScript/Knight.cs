using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Chessman
{
    private const int VECTOR_1 = 1;
    private const int VECTOR_2 = 2;

    public override void Start()
    {
        base.Start();
        move_pos = new Vector2[]
        {
            //↑,→,↓,←
            new Vector2(VECTOR_1, -VECTOR_2),//↗
            new Vector2(-VECTOR_1, -VECTOR_2),//↸
            new Vector2(VECTOR_2, -VECTOR_1),//➚
            new Vector2(-VECTOR_2, -VECTOR_1),//↖
            new Vector2(-VECTOR_1, VECTOR_2),//↙
            new Vector2(-VECTOR_2, VECTOR_1),//⇙
            new Vector2(VECTOR_1, VECTOR_2),//➘
            new Vector2(VECTOR_2, VECTOR_1)//↘
        };
    }

    public override void MovePossibleShow(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        base.MovePossibleShow(board_state, is_position_x, is_position_y, turn);

        this.conditions = new int[,]
        {
            //↗,➚,➘,↘,↙,⇙,↸,↖
            {is_position_y - VECTOR_2, is_position_x + VECTOR_1, NO_CONDITION, NO_CONDITION},//↗
            {is_position_y - VECTOR_2, NO_CONDITION, NO_CONDITION, is_position_x - VECTOR_1},//↸
            {is_position_y - VECTOR_1, is_position_x + VECTOR_2, NO_CONDITION, NO_CONDITION},//➚
            {is_position_y - VECTOR_1, NO_CONDITION, NO_CONDITION, is_position_x - VECTOR_2},//↖
            {NO_CONDITION, NO_CONDITION, is_position_y + VECTOR_2, is_position_x - VECTOR_1},//↙
            {NO_CONDITION, NO_CONDITION, is_position_y + VECTOR_1, is_position_x - VECTOR_2},//⇙
            {NO_CONDITION, is_position_x + VECTOR_1, is_position_y + VECTOR_2, NO_CONDITION},//➘
            {NO_CONDITION, is_position_x + VECTOR_2, is_position_y + VECTOR_1, NO_CONDITION}//↘
        };

        //移動範囲の数だけ
        for (int chess_num = 0; chess_num < move_pos.Length; chess_num++)
        {
            //条件↑,→,↓,←の順番
            if (CELL_MIN <= conditions[chess_num, 0] && conditions[chess_num, 1] <= CELL_MAX && conditions[chess_num, 2] <= CELL_MAX && CELL_MIN <= conditions[chess_num, 3])
                if (turn * board_state[is_position_y + (int)move_pos[chess_num].y, is_position_x + (int)move_pos[chess_num].x] <= NUM_NOT_CHESSMAN)
                    Plane.GetComponent<PositionGenerator>().Move_Possible_Plane(is_position_x + (int)move_pos[chess_num].x, is_position_y + (int)move_pos[chess_num].y);
        }
    }
}
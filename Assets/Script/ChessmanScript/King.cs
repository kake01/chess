using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{
    public override void Start()
    {
        base.Start();
        //↑,↗,→,↘,↓,←,↙,↖,,↗
        this.move_pos = new Vector2[] {
            new Vector2(VECTER_ZERO, -MOVEMENT),//↑
            new Vector2(MOVEMENT, -MOVEMENT),//,↗
            new Vector2(MOVEMENT, VECTER_ZERO),//→
            new Vector2(MOVEMENT, MOVEMENT),//↘
            new Vector2(VECTER_ZERO, MOVEMENT),//↓
            new Vector2(-MOVEMENT, MOVEMENT),//↙
            new Vector2(-MOVEMENT, VECTER_ZERO),//←
            new Vector2(-MOVEMENT, -MOVEMENT)//↖
        };
    }

    public override void NoneRenderer()
    {
    }

    public override void MovePossibleShow(int[,] board_state, int is_position_x, int is_position_y, int turn)
    {
        base.MovePossibleShow(board_state, is_position_x, is_position_y, turn);

        //それぞれの移動先が可能かどうかの判定リスト
        this.conditions = new int[,]
        {
            //↑,↗,→,↘,↓,←,↙,↖,,↗
            {is_position_y - MOVEMENT, NO_CONDITION, NO_CONDITION, NO_CONDITION},//↑
            {is_position_y - MOVEMENT, is_position_x + MOVEMENT, NO_CONDITION, NO_CONDITION},//,↗
            {NO_CONDITION, is_position_x + MOVEMENT, NO_CONDITION, NO_CONDITION},//→
            {NO_CONDITION, is_position_x + MOVEMENT, is_position_y + MOVEMENT, NO_CONDITION},//↘
            {NO_CONDITION, NO_CONDITION, is_position_y + MOVEMENT, NO_CONDITION},//↓
            {NO_CONDITION, NO_CONDITION, is_position_y + MOVEMENT, is_position_x - MOVEMENT},//↙
            {NO_CONDITION, NO_CONDITION, NO_CONDITION, is_position_x - MOVEMENT},//←
            {is_position_y - MOVEMENT, NO_CONDITION, NO_CONDITION, is_position_x - MOVEMENT}//↖
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
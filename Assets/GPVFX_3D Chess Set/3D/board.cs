using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    public const int NUM_NOT_CHESSMAN = 0;
    public const int NUM_ROOK = 1;
    public const int NUM_KNIGHT = 2;
    public const int NUM_BISHOP = 3;
    public const int NUM_QUEEN = 4;
    public const int NUM_KING = 5;
    public const int NUM_PAWN = 6;
    public int[,] board_2D_array =
    {
        {-NUM_ROOK, -NUM_KNIGHT, -NUM_BISHOP, -NUM_QUEEN, -NUM_KING, -NUM_BISHOP, -NUM_KNIGHT, -NUM_ROOK},
        {-NUM_PAWN, -NUM_PAWN, -NUM_PAWN, -NUM_PAWN, -NUM_PAWN, -NUM_PAWN, -NUM_PAWN, -NUM_PAWN},
        { NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN},
        { NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN},
        { NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN},
        { NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN,  NUM_NOT_CHESSMAN},
        { NUM_PAWN,  NUM_PAWN,  NUM_PAWN,  NUM_PAWN,  NUM_PAWN,  NUM_PAWN,  NUM_PAWN,  NUM_PAWN},
        { NUM_ROOK,  NUM_KNIGHT,  NUM_BISHOP,  NUM_QUEEN,  NUM_KING,  NUM_BISHOP,  NUM_KNIGHT,  NUM_ROOK}
    };

    void Start()
    {
    }

    void Update()
    {
    }

    public int[,] get_board_2D_array()
    {
        return board_2D_array;
    }
}

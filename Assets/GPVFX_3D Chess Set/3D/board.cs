using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    private const int NUM_ROOK = 1;
    private const int NUM_KNIGHT = 2;
    private const int NUM_BISHOP = 3;
    private const int NUM_QUEEN = 4;
    private const int NUM_KING = 5;
    private const int NUM_PAWN = 6;
    private const int NUM_NOT_CHESSMAN = 0;
    public int[,] board_2D_array =
     {
     {-NUM_ROOK, -NUM_KNIGHT, -NUM_BISHOP, -4, -NUM_KING, -NUM_BISHOP, -NUM_KNIGHT, -NUM_ROOK},
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

    int[,] get_board_2D_array()
    {
        return board_2D_array;
    }
}

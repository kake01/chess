using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    private const int NUM_NOT_CHESSMAN = 0;
    private const int NUM_ROOK = 1;
    private const int NUM_KNIGHT = 2;
    private const int NUM_BISHOP = 3;
    private const int NUM_QUEEN = 4;
    private const int NUM_KING = 5;
    private const int NUM_PAWN = 6;
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
    public GameObject clic_manager;

    void Start()
    {
        this.clic_manager = GameObject.Find("clic_manager");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            board_2D_array = clic_manager.GetComponent<clic_manager>().board_state_update();
    }

    public int[,] get_board_2D_array()
    {
        return board_2D_array;
    }
}
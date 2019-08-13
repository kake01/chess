using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    private const int WHITE_KING = 2;
    private const int BLACK_KING = -2;
    private const int PLAYER_WHITE = 1;
    private const int PLAYER_BLACK = 2;
    private const int WHITE_CHESSMAN = 1;
    private const int BLACK_CHESSMAN = -1;
    private const int NUM_NOT_CHESSMAN = 0;
    private const int TURN_CHENGE = -1;
    private const int CELL_LENGTH = 125;
    private const int CHESSMAN_SIDE_NUM = 8;
    public int turn = 1;// 1が白で-1が黒のturn
    public static int player_id;
    protected int[,] board_2D_array =
    {
        {BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN,BLACK_CHESSMAN, BLACK_KING, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN},
        {BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN, BLACK_CHESSMAN},
        {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
        {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
        {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
        {NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN, NUM_NOT_CHESSMAN},
        {WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN},
        {WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_KING, WHITE_CHESSMAN, WHITE_CHESSMAN, WHITE_CHESSMAN}
    };
    private List<Vector2> display_chessman;
    protected GameObject particle;
    protected GameObject white_pawn, black_pawn;
    protected GameObject white_rook, black_rook;
    protected GameObject white_king, black_king;
    protected GameObject white_queen, black_queen;
    protected GameObject white_knight, black_knight;
    protected GameObject white_bishop, black_bishop;
    protected GameObject click_manager, timer;
    protected GameObject[] chessman_list;
    protected PhotonView photonView;
    protected AudioSource move_sound;
    protected static string winner = "あなたの勝ちです";
    protected static string loser = "あなたの負けです。";
    public static string result;

    public void Start()
    {
        this.click_manager = GameObject.Find("Click_Manager");
        this.timer = GameObject.Find("Text");
        this.particle = GameObject.Find("Particle");
        this.photonView = GetComponent<PhotonView>();
        this.move_sound = GetComponent<AudioSource>();
    }

    public void ChessmanMoveShow(RaycastHit first_hit, int pos_x, int pos_y)
    {
        if (board_2D_array[pos_y, pos_x] * turn > NUM_NOT_CHESSMAN)
            first_hit.collider.gameObject.GetComponent<Chessman>().MovePossibleShow(board_2D_array, pos_x, pos_y, turn);
    }

    public void ChessmanMove(RaycastHit first_hit, int move_pos_x, int move_pos_y, int select_x, int select_y)
    {
        //相手に伝える為の仮変数
        Vector2 before_chessman_cell = new Vector2(select_x, select_y);
        Vector2 after_chessman_cell = new Vector2(move_pos_x, move_pos_y);

        photonView.RPC("MoveSound", PhotonTargets.MasterClient);


        //相手の駒が合ったら削除する
        if (board_2D_array[move_pos_y, move_pos_x] != NUM_NOT_CHESSMAN)
        {
            //キングの場合はゲームオーバー
            if (board_2D_array[move_pos_y, move_pos_x] == WHITE_KING)
                photonView.RPC("Result", PhotonTargets.All, WHITE_KING);
            if (board_2D_array[move_pos_y, move_pos_x] == BLACK_KING)
                photonView.RPC("Result", PhotonTargets.All, BLACK_KING);

            photonView.RPC("ParticleOccurrence", PhotonTargets.All, after_chessman_cell);
            photonView.RPC("ChessmanDestroy", PhotonTargets.Others, after_chessman_cell);
        }

        //移動の描画
        first_hit.collider.gameObject.GetComponent<Chessman>().ChessmanMove(move_pos_x, move_pos_y);
        //boardの更新
        photonView.RPC("BoardUpdate", PhotonTargets.All, before_chessman_cell, after_chessman_cell);

        //自分のディスプレイの駒の描画処理
        DrawingUpdate();
        //turnの更新
        photonView.RPC("TurnUpdate", PhotonTargets.All);
        //相手のディスプレイの駒の描画処理
        photonView.RPC("DrawingUpdate", PhotonTargets.Others);

        //自分のタイマーを止め,相手のタイマーを開始させる
        timer.gameObject.GetComponent<Timer>().TimerUpdate();
    }
    [PunRPC]
    protected void MoveSound() => move_sound.Play();

    [PunRPC]
    public void DrawingUpdate()
    {
        //敵の全駒の描画削除
        photonView.RPC("HideChessman", PhotonTargets.Others);

        //自分の駒の数だけ
        foreach (GameObject Obj in this.chessman_list)
        {
            if (Obj != null)
            {
                //一つの駒に対する敵の駒の発見した駒の値が入る
                display_chessman = Obj.gameObject.GetComponent<Chessman>().FindChessman(turn, board_2D_array);
                //その場所に該当する駒の描画
                for (int i = 0; i < display_chessman.Count; i++)
                    photonView.RPC("ShowChessman", PhotonTargets.Others, display_chessman[i]);
            }
        }
    }
    //見える駒の描画を行う
    [PunRPC]
    public void ShowChessman(Vector2 disp_pos)
    {
        foreach (GameObject Obj in this.chessman_list)
            if (Obj != null)
                Obj.GetComponent<Chessman>().DisplayRenderer(disp_pos);
    }
    //相手の駒の描画を全て消す
    [PunRPC]
    public void HideChessman()
    {
        foreach (GameObject Obj in this.chessman_list)
            if (Obj != null)
                Obj.GetComponent<Chessman>().NoneRenderer();
    }
    [PunRPC]
    void BoardUpdate(Vector2 before_board_array, Vector2 after_board_array)
    {
        this.board_2D_array[(int)after_board_array.y, (int)after_board_array.x] = board_2D_array[(int)before_board_array.y, (int)before_board_array.x];
        this.board_2D_array[(int)before_board_array.y, (int)before_board_array.x] = NUM_NOT_CHESSMAN;
    }
    [PunRPC]
    public void ChessmanDestroy(Vector2 destroy_pos)
    {
        foreach (GameObject Obj in this.chessman_list)
            if (Obj != null)
                Obj.gameObject.GetComponent<Chessman>().ChessmanDestroy((int)destroy_pos.x, (int)destroy_pos.y);
    }
    [PunRPC]
    public void ParticleOccurrence(Vector2 occurrence_cell) => particle.GetComponent<Particle>().Occurrence(occurrence_cell);
    [PunRPC]
    public void TurnUpdate() => this.turn *= TURN_CHENGE;
    [PunRPC]
    public void Result(int king_name)
    {
        //黒kingが取られた場合
        if (king_name == BLACK_KING)
        {
            if (player_id == PLAYER_WHITE)
                result = winner;
            if (player_id == PLAYER_BLACK)
                result = loser;
        }
        //白kingが取られた場合
        if (king_name == WHITE_KING)
        {
            if (player_id == PLAYER_WHITE)
                result = loser;
            if (player_id == PLAYER_BLACK)
                result = winner;
        }

        SceneManager.LoadScene("CheckMateScene");
    }

    /* turnの変化をタイマーで使う部分 */
    public void TurnChange() => photonView.RPC("TurnUpdate", PhotonTargets.All);

    public void OnJoinedRoom()
    {
        player_id = PhotonNetwork.player.ID;
        //白駒の生成
        if (player_id == PLAYER_WHITE)
        {
            //改良出来るはず
            for (int init_x = 0; init_x < CHESSMAN_SIDE_NUM; init_x++)
            {
                Vector3 pos = new Vector3(init_x * CELL_LENGTH, 0, -CELL_LENGTH * 6);
                white_pawn = PhotonNetwork.Instantiate("white_pawn", pos, this.transform.rotation, 0);
                white_pawn.gameObject.GetComponent<Renderer>().enabled = true;
                white_pawn.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            for (int init_x = 0; init_x < CHESSMAN_SIDE_NUM; init_x++)
            {
                Vector3 pos = new Vector3(init_x * CELL_LENGTH, 0, -CELL_LENGTH * 7);
                if (init_x == 0 || init_x == 7)
                {
                    white_rook = PhotonNetwork.Instantiate("white_rook", pos, this.transform.rotation, 0);
                    white_rook.gameObject.GetComponent<Renderer>().enabled = true;
                    white_rook.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 1 || init_x == 6)
                {
                    white_knight = PhotonNetwork.Instantiate("white_knight", pos, this.transform.rotation, 0);
                    white_knight.gameObject.GetComponent<Renderer>().enabled = true;
                    white_knight.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 2 || init_x == 5)
                {
                    white_bishop = PhotonNetwork.Instantiate("white_bishop", pos, this.transform.rotation, 0);
                    white_bishop.gameObject.GetComponent<Renderer>().enabled = true;
                    white_bishop.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 3)
                {
                    white_queen = PhotonNetwork.Instantiate("white_queen", pos, this.transform.rotation, 0);
                    white_queen.gameObject.GetComponent<Renderer>().enabled = true;
                    white_queen.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 4)
                {
                    white_king = PhotonNetwork.Instantiate("white_king", pos, this.transform.rotation, 0);
                    white_king.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
        //黒駒の生成
        if (player_id == PLAYER_BLACK)
        {
            for (int init_x = 0; init_x < CHESSMAN_SIDE_NUM; init_x++)
            {
                Vector3 pos = new Vector3(init_x * CELL_LENGTH, 0, -CELL_LENGTH * 1);
                black_pawn = PhotonNetwork.Instantiate("black_pawn", pos, this.transform.rotation, 0);
                black_pawn.gameObject.GetComponent<Renderer>().enabled = true;
                black_pawn.gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            for (int init_x = 0; init_x < CHESSMAN_SIDE_NUM; init_x++)
            {
                Vector3 pos = new Vector3(init_x * CELL_LENGTH, 0, -CELL_LENGTH * 0);
                if (init_x == 0 || init_x == 7)
                {
                    black_rook = PhotonNetwork.Instantiate("black_rook", pos, this.transform.rotation, 0);
                    black_rook.gameObject.GetComponent<Renderer>().enabled = true;
                    black_rook.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 1 || init_x == 6)
                {
                    black_knight = PhotonNetwork.Instantiate("black_knight", pos, this.transform.rotation, 0);
                    black_knight.gameObject.GetComponent<Renderer>().enabled = true;
                    black_knight.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 2 || init_x == 5)
                {
                    black_bishop = PhotonNetwork.Instantiate("black_bishop", pos, this.transform.rotation, 0);
                    black_bishop.gameObject.GetComponent<Renderer>().enabled = true;
                    black_bishop.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 3)
                {
                    black_queen = PhotonNetwork.Instantiate("black_queen", pos, this.transform.rotation, 0);
                    black_queen.gameObject.GetComponent<Renderer>().enabled = true;
                    black_queen.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
                if (init_x == 4)
                {
                    black_king = PhotonNetwork.Instantiate("black_king", pos, this.transform.rotation, 0);
                    black_king.gameObject.GetComponent<BoxCollider>().enabled = true;
                }
            }
        }
        this.chessman_list = GameObject.FindGameObjectsWithTag("Chessman");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Application.Quit();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    private const int PLAYER_WHITE = 1;
    private const int PLAYER_BLACK = 2;
    private const int WHITE_CHESSMAN = 1;
    private const int BLACK_CHESSMAN = -1;
    private const int NUM_NOT_CHESSMAN = 0;
    private const int CELL_LENGTH = 125;
    private const int TURN_CHENGE = -1;
    public int player_id;
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
    protected PhotonView photonView;
    protected GameObject[] chessman_list = new GameObject[16];
    private List<Vector2> display_chessman;
    string[] chessman_type = { "rook", "knight", "bishop", "queen", "king", "bishop", "knight", "rook" };


    //boardが持ってるのはおかしい
    public int turn = 1;// 1が白で-1が黒のturn
    protected static string winner = "あなたの勝ちです";
    protected static string loser = "あなたの負けです。";
    protected GameObject particle;
    protected AudioSource move_sound;
    public static string result;
    protected GameObject timer;

    //不要になる可能性がある
    private const int WHITE_KING = 2;
    private const int BLACK_KING = -2;


    public void Start()
    {
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
        if (PLAYER_WHITE == player_id)
            Debug.Log(123);
        //            ChessmanCreate("white_");
        //黒駒の生成
        if (PLAYER_BLACK == player_id)
            Debug.Log(123);
        //          ChessmanCreate("black_");
    }



    private void CreateChessman(string color, string type, int chessman_num, int pos_x, int pos_y)
    {
        Vector3 pos = new Vector3(pos_x * CELL_LENGTH, 0, -CELL_LENGTH * pos_y);
        chessman_list[chessman_num] = PhotonNetwork.Instantiate(color + type, pos, this.transform.rotation, 0);
        chessman_list[chessman_num].gameObject.GetComponent<Renderer>().enabled = true;
        chessman_list[chessman_num].gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void RowCreateChessman(string chessman_color, int row)
    {
        for (int i = 0; i < 8; i++)
        {
            CreateChessman(chessman_color, "pawn", i, i, row);
            CreateChessman(chessman_color, chessman_type[i], 8 + i, i, row);
        }
    }

    /*
        private void ChessmanCreate(string chessman_color, int row)
        {



            for (int i = 0; i < 8; i++)
            {
                Vector3 pos = new Vector3(i * CELL_LENGTH, 0, -CELL_LENGTH * row);
                chessman_list[i] = PhotonNetwork.Instantiate(chessman_color + "pawn", pos, this.transform.rotation, 0);
                chessman_list[i].gameObject.GetComponent<Renderer>().enabled = true;
                chessman_list[i].gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            for (int i = 0; i < 8; i++)
            {
                Vector3 pos = new Vector3(i * CELL_LENGTH, 0, -CELL_LENGTH * row);
                chessman_list[8 + i] = PhotonNetwork.Instantiate(chessman_color + cheeman_type[i], pos, this.transform.rotation, 0);
                chessman_list[8 + i].gameObject.GetComponent<Renderer>().enabled = true;
                chessman_list[8 + i].gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }
        */
    //強制終了
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Application.Quit();
    }
}
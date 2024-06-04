using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public MapGenerator mapGenerator;//MapGeneratorの関数にアクセスできるようにする

    public GameObject successText, failText, toTitleButton;//, toTitleButtonFrame;//お宝探しの成功時，失敗時，タイトル遷移ボタンのテキストオブジェクト
    public TextMeshProUGUI operationInstruction;
    //Start is called before the first frame update
    void Start()
    {
        //最初はいずれもも見えない状態
        toTitleButton.SetActive(false);
        //toTitleButtonFrame.SetActive(false);
        successText.SetActive(false);
        failText.SetActive(false);
        operationInstruction.text = "←↓→キーで方向転換 ↑キーで前進\r\nスペースキーで発掘  hキーでヒント";
    }

    // Update is called once per frame
    void Update()
    { 
        //スペースキーが押された際に
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject player = GameObject.Find("Player(Clone)");
            if (!player.GetComponent<Player>().gameClear)//まだゲームが続いているならば
            {
                //お宝探しが成功したならば(プレーヤーの座標と宝の座標が同じならば)
                if (mapGenerator.GetSpaceType(player.GetComponent<Player>().playerPos) == MapGenerator.SpaceType.Treasure)
                {
                    successText.SetActive(true);//クリアメッセージを表示
                    operationInstruction.text = "スペースキーでもう一回お宝さがし";
                    toTitleButton.SetActive(true);//タイトル遷移ボタン表示
                    //toTitleButtonFrame.SetActive(true);//タイトル遷移ボタン表示
                }
                else//お宝探しが失敗したならば
                {
                    failText.SetActive(true);//失敗用のメッセージを表示
                }
            }
            else//ゲームがクリアされているならば
            {
                Start();//もう一回
            }

        }

        //Escキーでタイトルに戻る
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StartScene");//ゲーム画面へと遷移
        }

        //移動，または方向転換が起きたならば
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject player = GameObject.Find("Player(Clone)");
            if (!player.GetComponent<Player>().gameClear)//まだゲームが続いているならば
            {
                failText.SetActive(false);//失敗用のメッセージを消す
            }

        }
    }
}

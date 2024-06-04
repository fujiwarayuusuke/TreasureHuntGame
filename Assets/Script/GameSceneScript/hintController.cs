using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ヒントによってプレイヤーの位置をマップに表示させるか切り替える(レイヤーで調整)
public class hintContrller : MonoBehaviour
{
    GameObject player;//プレイヤーのゲームオブジェクト
    bool isHint;//最初はヒント無し
    // Start is called before the first frame update
    void Start()
    {
        isHint = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Hが押下されたならば
        if (Input.GetKeyDown(KeyCode.H) )
        {
            player = GameObject.Find("Player(Clone)");
            if (!player.GetComponent<Player>().gameClear && isHint)//ヒントを与えている状態ならば，プレイヤーの位置を隠す
            {
                player.GetComponent<Renderer>().sortingOrder = -2;
                isHint = !isHint;//フラグの切り替え
            }
            else if(!player.GetComponent<Player>().gameClear && !isHint)//ヒントを与えていない状態ならばプレイヤーの位置を示す
            {
                player.GetComponent<Renderer>().sortingOrder = 1;
                isHint = !isHint;//フラグの切り替え
            }
        }

        //スペースキーが押された際に
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject player = GameObject.Find("Player(Clone)");
            if (player.GetComponent<Player>().gameClear)//ゲームが終了しているならば
            {
                
                Start();//もう一回
                
            }
        }
    }
}

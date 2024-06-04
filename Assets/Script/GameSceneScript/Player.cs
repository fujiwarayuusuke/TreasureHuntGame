using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

//操作キャラのためのスクリプト
public class Player : MonoBehaviour
{
    public MapGenerator mapGenerator;

    //プレイヤーの座標
    public Vector2Int playerPos, nextPos;

    public bool gameClear;//ゲーム終了判定用のフラグ

    //向いている方向を表す列挙型
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }
    //これに向きを格納する変数
    public Direction direction = Direction.Up;

    //1歩進んだ際の座標のx,y座標の遷移
    //Up,Downについてはy座標が反対なので直観とは逆向きになっている
    int[,] step = {
      { 1, 0 },　//Rightの場合
      { -1, 0 },   //LeftTの場合
      { 0, -1 },   //Upの場合
      { 0, 1 }   //Downの場合
    };

    //各方向を向いた時の背景の描画の順番
    int[,,] location = {
            //左{x,y} 右{x,y} 中{x,y}
           {{0,1 },{0,-1 },{0,0 } },//Right 上，下，真ん中の順
           {{0,-1 },{0,1 },{0,0 } },//Left　下，上，真ん中の順
           {{1,0 } , {-1,0 }, {0,0 } },//Up　左，右，真ん中の順
           {{-1,0 },{1,0 },{0,0 } },//Down　右，左，真ん中の順
    };

    //向いている方向をマップ上に示すもの
    [SerializeField] Transform faceDirection;
    //向いている方向を表すオブジェクトの上下左右のポジション
    Vector3[] faceDirectionPosition = 
        new[] { new Vector3(1.5f, 0), new Vector3(-1.5f, 0f), new Vector3(0, -1.5f), new Vector3(0f, 1.5f) };

    AudioSource[] audioSources;//効果音用の配列

    // Start is called before the first frame update
    void Start()
    {
        //mapGeneratorはプレイヤーの生成時にMapGeneratorから登録
        gameClear = false;//ゲームのクリア状況を初期化
        direction = Direction.Down;//最初は下向き
        //viewFaceDirection();//向いている方向の表示
        mapGenerator.ResetView3D();//3D視点を一旦消去

        seePosition();//3D視点を構成

        //AudioSourceコンポーネントを取得し、変数に格納
        audioSources = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameClear)//ゲームがまだ続いている場合は
        {
            //十字キーの入力に従い方向を決め，移動用のmove関数をよぶ
            //上キーの時のみ動き他は方向転換
            if (Input.GetKeyDown(KeyCode.DownArrow))//反対向く
            {
                if (direction == Direction.Down || direction == Direction.Left)
                {
                    direction -= 1;
                }
                else
                {
                    direction += 1;
                }
                //viewFaceDirection();
                mapGenerator.ResetView3D();//3D視点をリセット
                seePosition();//3D視点を構築
                audioSources[2].PlayOneShot(audioSources[2].clip);//効果音を鳴らす
                //Debug.Log(faceDirection.position);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))//進む
            {
                move();
                mapGenerator.ResetView3D();
                seePosition();
                //Debug.Log(mapGenerator.GetSpaceType(playerPos));
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))//90度左向く
            {
                if (direction == Direction.Right)
                {
                    direction = Direction.Up;
                }
                else if (direction == Direction.Left)
                {
                    direction = Direction.Down;
                }
                else if (direction == Direction.Up)
                {
                    direction = Direction.Left;
                }
                else
                {
                    direction = Direction.Right;
                }
                //viewFaceDirection();
                mapGenerator.ResetView3D();//3D視点をリセット
                seePosition();//3D視点を構築
                audioSources[2].PlayOneShot(audioSources[2].clip);//効果音を鳴らす
                //Debug.Log(faceDirection.position);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))//90度右向く
            {
                if (direction == Direction.Right)
                {
                    direction = Direction.Down;
                }
                else if (direction == Direction.Left)
                {
                    direction = Direction.Up;
                }
                else if (direction == Direction.Up)
                {
                    direction = Direction.Right;
                }
                else
                {
                    direction = Direction.Left;
                }
                //viewFaceDirection();
                mapGenerator.ResetView3D();//3D視点をリセット
                seePosition();//3D視点を構築
                audioSources[2].PlayOneShot(audioSources[2].clip);//効果音を鳴らす
                //Debug.Log(faceDirection.position);
            }

            

            //今いるマスのお宝を調べ，宝が発見できたならば
            if (Input.GetKeyDown(KeyCode.Space)　&& mapGenerator.GetSpaceType(playerPos) == MapGenerator.SpaceType.Treasure)
            {
                gameClear = true;//ゲームクリア用のフラグをオンにする
                audioSources[3].PlayOneShot(audioSources[3].clip);//効果音を鳴らす
            }
            else if(Input.GetKeyDown(KeyCode.Space) && mapGenerator.GetSpaceType(playerPos) != MapGenerator.SpaceType.Treasure)//宝が発見できなければ
            {
                audioSources[4].PlayOneShot(audioSources[4].clip);//効果音を鳴らす
            }
        }
        else//ゲームクリアしていた場合は
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Start();//スペースキーでもう一回
            }
                
        }
        
    }

    //向いている方向を表すオブジェクトの位置を修正
    void viewFaceDirection()
    {
        faceDirection.localPosition = faceDirectionPosition[(int)direction];
        
    }

    //移動用の関数
    //playerPosの更新とともにプレイヤー画像の更新も行う
    private void move()
    {
        
        //仮に進むとして次の位置を計算
        //方向に合わせてstepから変位を取得し，移動のベクトルに対応するVector2Intを元の位置に加算
        nextPos = playerPos + new Vector2Int(step[(int)direction, 0], step[(int)direction, 1]);
        
        //もし進行方向が壁ではない場合
        if ( mapGenerator.GetSpaceType( nextPos ) != MapGenerator.SpaceType.Wall )
        {
            transform.localPosition = mapGenerator.realPos(nextPos);//プレイヤーの画像の位置を更新
            playerPos = nextPos;//プレーヤーの位置を更新
            audioSources[0].PlayOneShot(audioSources[0].clip);//前進したときの効果音
        }
        else
        {
            audioSources[1].PlayOneShot(audioSources[1].clip);//壁にぶつかった時の効果音
        }
    }

    // 現在の座標，向きから見える座標の描画をする関数
    void seePosition()
    {
        int depth = 3;//奥は3マス先まで
        int width = 2;//幅は自身の左右2マス
        for (int d = depth; d >= 0; d--)//奥の座標から取得
        {
            for (int w = 0; w <= width; w++)
            {
                int posX = playerPos.x + location[(int)direction, w, 0] + step[(int)direction, 0] * d;//プレイヤーから見て奥行d，左右正面いずれかのx座標
                int posY = playerPos.y + location[(int)direction, w, 1] + step[(int)direction, 1] * d;//プレイヤーから見て奥行d，左右正面いずれかのy座標
                //Debug.Log(new Vector2Int(mapX, mapY));
                //Debug.Log(mapGenerator.w) ;

                //posX,posYが迷路の中のマスならば
                if (posX >= 0 && posX < mapGenerator.row && posY >= 0 && posY < mapGenerator.col)
                {
                    //posX,posYに壁が存在するならば
                    if (mapGenerator.GetSpaceType(new Vector2Int(posX, posY)) == MapGenerator.SpaceType.Wall)
                    {
                        //MapGeneratorのView3Dに送るパラメタ，小さい番号の方が手前の画像のインデックス
                        int index = d * depth + width - w;
                        //追加 MapGeneratorのView3Dを呼び出し
                        mapGenerator.View3D(index);
                    }
                }
                
            }
        }
    }
}

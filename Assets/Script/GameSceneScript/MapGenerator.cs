using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップをMap，マスのことをSpaceと呼ぶことにする
public class MapGenerator : MonoBehaviour
{
    //マップをテキストで表現　テキスト中で0が床，１が壁，2がプレイヤーのマス，","が升目の区切りを表す
    [SerializeField] TextAsset mapText;
    //5 * 5のマップの一例としては以下
    // 1,1,1,1,1
    // 1,0,0,1,1
    // 1,0,1,1,1
    // 1,0,0,2,1
    // 1,1,1,1,1

    //オブジェクトの絵のプレファブ　配列の0が床，１が壁，2がプレイヤーのドット絵，3がお宝を表す
    [SerializeField] GameObject[] prefabs;

    //迷路の各プレファブの親要素
    [SerializeField] Transform map2D;

    //3D視点の壁画像のための配列,プレイヤーから見て手前の画像が先に格納されるようにする
    [SerializeField] WallArr[] wallArr;

    //マップの縦横の長さを表す
    public int row, col;

    //マップの大きさの乱数保持用の補助変数
    public int h, w;

    //中心座標の位置を表す
    Vector2 centerPos;

    //一マスごとの幅を表す
    float spaceSize;

    MazeGenerator maze; //MazeGenerator型の変数を定義
    int[,] mazeData; //迷路データ用にint型の二次元配列の変数を定義
    GameObject[] objects;//2Dマップ制作時のオブジェクトを保管しておく変数，オブジェクト破壊用

    //各マスを表す列挙型,テキストでは0,1,2文字列だが，プログラムで扱いやすいよう列挙型で表す
    public enum SpaceType
    {
        Floor, //0...Floor
        Wall,   //1...Wall
        Player,  //2...Player
        Treasure //3...Treasure
    }
    SpaceType[,] map; // 各マスの二次元配列がマップとなる

    //座標を入力として対応するmapの位置の列挙型の値を返す関数
    public SpaceType GetSpaceType(Vector2Int pos)
    {
        return map[pos.x, pos.y];
    }

    // Start is called before the first frame update
    void Start()
    { 
        h = StartDirector.height;
        w = StartDirector.width;
        
        maptextToSpceType();//mapを作成
       
        makeMap(); //マップを表示
        
        //マップの表示位置
        map2D.position = new Vector3(0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //スペースキー押下時にゲームクリア済みであればもう一回
        if (Input.GetKeyDown(KeyCode.Space))
        {
            

            GameObject player = GameObject.Find("Player(Clone)");
            if (player.GetComponent<Player>().gameClear)
            {
                //2Dマップを削除
                clear2DView();
                Start();
            }
        }
    }

    //mapを作成する関数
    private void maptextToSpceType()
    {
        //MazeGeneratorをインスタンス化
        maze = new MazeGenerator(h, w);
        //迷路データ用二次元配列を生成
        mazeData = new int[h, w];
        //迷路データ作成＆取得
        mazeData = maze.MazeGenerate();
        //マップの横の長さ取得
        row = mazeData.GetLength(1);
        //マップの縦の長さ取得
        col = mazeData.GetLength(0);
        //マップテーブル初期化
        map = new SpaceType[col, row];
        //ゲームオブジェクトの配列を初期化
        objects = new GameObject[row * col　+ 2];//各マスの床と壁，更にお宝とプレイヤーオブジェクトで+2

        //マップテーブル作成
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                //迷路データをSpaceTypeにキャストしてマップテーブルに格納
                map[i, j] = (SpaceType)mazeData[i , j];
            }
        }


        //テキストデータからマップを作る際は以下のコメントアウトを外す

        //var lineSeparator = new char[] { '\n', '\r' };//無視するべき改行コード
        //var spaceSeparator = new char[] { ',' };//マスの間の無視するべきコンマ

        ////テキストデータを各行ごとに分割
        //string[] mapLines = mapText.text.Split(lineSeparator, System.StringSplitOptions.RemoveEmptyEntries);

        ////行の数
        //int numRow = mapLines.Length;
        ////列の数
        //int numCol = mapLines[0].Split(new char[] { ',' }).Length;
        ////numRow * numColのマスを持つmapを初期化
        //map = new SpaceType[numCol, numRow];

        ////各行ごとに分割
        //for(int i = 0; i< numRow; i++)
        //{
        //    //1マスごとに分割
        //    string[] spaceValue = mapLines[i].Split(spaceSeparator);

        //    //各0,1,2をenum型の値に変換
        //    for (int j = 0; j < numCol; j++)
        //    {
        //        //配列mapのi行j列目への代入
        //        //spaceValueのj番目を整数にキャストし，更にSpaceType型にキャスト
        //        map[i, j] = (SpaceType) int.Parse(spaceValue[j]);
        //    }
        //}
    }

    private void makeMap()
    {
        //ドット絵のサイズを取得する
        spaceSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;

        //中心座標を取得
        calcuCenter();

        //mapの各行でループ
        for (int i = 0; i< map.GetLength(0); i++)
        {
            //mapの各列でループ
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //仮の座標
                Vector2Int tantativePos = new Vector2Int(i, j);

                //map[x,y]に当たる画像をprefabs内から持ってきてmap2Dの子要素としてインスタンス化
                GameObject mapObject = Instantiate(prefabs[(int)map[i, j]], map2D);
                //オブジェクトを配列に格納
                objects[j + i * map.GetLength(0)] = mapObject;
                //mapObjectの位置を適切に移動
                //realPos関数により画像の大きさを考慮した位置に表示される
                mapObject.transform.position = realPos(tantativePos);


                ////座標が1，1なら
                //if (i == 1 && j == 1)
                //{
                //    //プレイヤーを生成
                //    GameObject player = Instantiate(prefabs[2], map2D);
                //    //ポジション修正
                //    player.transform.position = realPos(tantativePos);
                //    player.GetComponent<Player>().playerPos = tantativePos;
                //    //プレイヤーのmapGeneratorを設定
                //    player.GetComponent<Player>().mapGenerator = this;
                //}

                //テキストデータからマップを作る際は以下のコメントアウトを外す
                ////プレーヤーのマスの場合は床も同時に表示
                //if (map[i, j] == SpaceType.Player)
                //{
                //    //床の画像をprefabs内から持ってきてインスタンス化
                //    GameObject floorObject = Instantiate(prefabs[(int) SpaceType.Floor], this.transform);
                //    //mapObjectの位置を適切に移動
                //    floorObject.transform.position = realPos(tantativePos);

                //    //プレイヤーの座標を代入
                //    mapObject.GetComponent<Player>().playerPos = tantativePos;
                //}
            }
        }


        //プレイヤーの初期位置の設定
        while (true)
        {
            //プレイヤーの初期位置は壁ではないところからランダムに決定
            Vector2Int randomPos = new Vector2Int(Random.Range(1, w), Random.Range(1, h));
            if(GetSpaceType(randomPos) == SpaceType.Floor)
            {
                //プレイヤーを生成
                GameObject player = Instantiate(prefabs[2], map2D);
                //ポジション修正
                player.transform.position = realPos(randomPos);
                player.GetComponent<Player>().playerPos = randomPos;
                //最初はプレイヤーの位置を隠しておく
                player.GetComponent<Renderer>().sortingOrder = -2;
                //プレイヤーのmapGeneratorを設定
                player.GetComponent<Player>().mapGenerator = this;
                //プレイヤーオブジェクトを格納
                objects[map.GetLength(1) * map.GetLength(0)] = player;
                break;
            }
        }

        //お宝の位置の設定
        while (true)
        {
            //お宝の初期位置は壁ではないところからランダムに決定
            Vector2Int randomPos = new Vector2Int(Random.Range(1, w), Random.Range(1, h));
            if (GetSpaceType(randomPos) == SpaceType.Floor)
            {
                //プレイヤーを生成
                GameObject treasure = Instantiate(prefabs[3], map2D);
                //ポジション修正
                treasure.transform.position = realPos(randomPos);
                //マップ情報を更新
                map[randomPos.x, randomPos.y] = SpaceType.Treasure;
                //お宝のオブジェクトを格納
                objects[map.GetLength(1) * map.GetLength(0) + 1] = treasure;
                break;
            }
        }



        //中心座標を計算する関数
        //各行，各列数の半分に画像のサイズをかける
        void calcuCenter()
        {
            //行が偶数の場合は画像の半分のサイズを引く調整が必要
            if (map.GetLength(0) % 2 == 0)
            {
                centerPos.x = map.GetLength(0) / 2 * spaceSize - (spaceSize / 2);
            }
            else//行が奇数の場合
            {
                centerPos.x = map.GetLength(0) / 2 * spaceSize;
            }

            //列も同様に計算する
            if (map.GetLength(1) % 2 == 0)
            {
                centerPos.y = map.GetLength(1) / 2 * spaceSize - (spaceSize / 2);
            }
            else
            {
                centerPos.y = map.GetLength(1) / 2 * spaceSize;
            }
        }
    }

    //仮の座標からプレファブの適切な位置を返す関数
    public Vector2 realPos(Vector2Int pos)
    {
        //仮の座標 * 画像の大きさ の計算で真の座標を取得
        //mapを中心に移すためにcenterPosの値で引き算
        return new Vector2(pos.x * spaceSize - centerPos.x, -(pos.y * spaceSize - centerPos.x));
    }

    //3D画面の壁を表示させる関数,
    //引数が小さいほど手前の画像を表示させる
    public void View3D(int index)
    {
        //Debug.Log(index);
        foreach (GameObject wallpaper in wallArr[index].wall)
        {
            wallpaper.SetActive(true);
            
        }
    }

    //移動時，方向転換時にそれまで表示していた壁画像を消すための関数
    public void ResetView3D()
    {
        foreach (WallArr walls in wallArr)
        {
            foreach (GameObject wallpaper in walls.wall)
            {
                wallpaper.SetActive(false);
            }
        }
    }

    //クリア後に2Dマップのオブジェクトを全削除
    private void clear2DView()
    {
        foreach(GameObject spaceTile in objects)
        {
            Destroy(spaceTile);
        }
    }
}

//壁画像用のクラス，GameObjectの配列なので複数の画像を一緒に纏めることができる
[System.Serializable]
public class WallArr
{
    public GameObject[] wall;
}

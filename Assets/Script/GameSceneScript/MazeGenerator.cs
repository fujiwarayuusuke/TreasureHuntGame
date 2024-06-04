using System;
using System.Collections;
using System.Collections.Generic;

//テキストデータで迷路を作る
//今回は穴掘り法を採用
public class MazeGenerator
{
    int[,] maze; //int型の二次元配列で迷路を表す
    int width; //横幅
    int height; //縦幅
    const int floor = 0; //0は通路を表す
    const int wall = 1; //1は壁を表す
    
    enum Direction　//穴を掘っていく方向
    {
        Right,//右
        Left, //左
        Up,   //上
        Down  //下
    }

    List<Cell> startCells = new List<Cell>();　//マス目用クラスのリスト

    //MazeGeneratorのコンストラクタ
    public MazeGenerator(int w, int h)
    {
        if ( w < 5 ||  h < 5) throw new ArgumentOutOfRangeException();// 幅5未満の小さすぎる迷路は作らない

        //閉じた領域とループ構造を無くすため，迷路の大きさは奇数にする
        if ( w % 2 == 0) w++; //横が偶数なら+1して奇数にする
        if ( h % 2 == 0) h++; //縦が偶数なら+1して奇数にする

        this.width = w; //渡ってきた横幅を横幅変数widthに代入
        this.height = h; //渡ってきた縦幅を縦幅変数heightに代入
        this.maze = new int[w, h]; //迷路用の二次元配列を作成
    }

    //迷路データを生成する関数
    public int[,] MazeGenerate()
    {
        //外周以外を壁で,内側を床で埋める
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    maze[x, y] = floor;//外側，最終的に壁にする
                }
                else
                {
                    maze[x, y] = wall;//内側
                }
            }
        }

        //座標1，1から掘る
        Dig(1, 1);

        //終わったら外周を壁で埋める
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    maze[x, y] = wall;
                }
            }
        }

        //迷路データを返す
        return maze;
    }

    //掘る道を決める関数
    void Dig(int x, int y)
    {
        //掘る方向の乱数のため、Randomの変数rndを作成
        Random rnd = new Random();

        //掘る作業
        while (true)
        {
            //掘れる方角の候補を保持するリスト
            List<Direction> direction = new List<Direction>();

            //現在のマスから2マス先まで壁ならば(掘っても迷路として機能するならば)掘れる方角をリストに格納
            //短絡評価で一つ目がfalseならば二つ目の判断でにマスがなくとも評価されない
            if (maze[x + 1, y] == wall && maze[x + 2, y] == wall)
                direction.Add(Direction.Right);
            if (maze[x - 1, y] == wall && maze[x - 2, y] == wall)
                direction.Add(Direction.Left);
            if (maze[x, y - 1] == wall && maze[x, y - 2] == wall)
                direction.Add(Direction.Up);
            if (maze[x, y + 1] == wall && maze[x, y + 2] == wall)
                direction.Add(Direction.Down);
            

            //この時点で掘れる方角がなければループを抜ける
            if (direction.Count == 0) break;

            //ループを抜けなければ道をセット
            SetPath(x, y);

            //掘る方角をランダムで取得
            int directionIndex = rnd.Next(direction.Count);

            //取得した方角によって掘り進める
            switch (direction[directionIndex])
            {
                case Direction.Right:
                    SetPath(++x, y);
                    SetPath(++x, y);
                    break;
                case Direction.Left:
                    SetPath(--x, y);
                    SetPath(--x, y);
                    break;
                case Direction.Up:
                    SetPath(x, --y);
                    SetPath(x, --y);
                    break;
                case Direction.Down:
                    SetPath(x, ++y);
                    SetPath(x, ++y);
                    break;
            }

            //次の起点を取得
            Cell cell = GetStartCell();

            //もし起点があればDig関数を自身で呼び出す
            if (cell != null)
            {
                Dig(cell.X, cell.Y);
            }

        }
    }

    //道を掘り次の起点候補を決める関数
    void SetPath(int x, int y)
    {
        //受け取った座標をfloor（道）にする
        maze[x, y] = floor;
        //座標が縦横共に奇数なら次の起点候補に追加する
        if (x % 2 == 1 && y % 2 == 1)
        {
            startCells.Add(new Cell() { X = x, Y = y });
        }
    }

    //起点位置をランダムで選択する関数
    Cell GetStartCell()
    {
        //もし格納された起点リストが0ならnullを返して終わり
        if (startCells.Count == 0) return null;

        //起点をランダムで選ぶためのRandom型変数
        Random rnd = new Random();
        //起点リストのインデックスをランダムに取得
        int idx = rnd.Next(startCells.Count);
        //ランダムで起点を取得しCell型の変数に格納
        Cell cell = startCells[idx];
        //取得した起点をリストから削除
        startCells.RemoveAt(idx);
        //新たな起点を返す
        return cell;
    }
}

//マス目用のクラス
public class Cell
{
    public int X, Y;
}
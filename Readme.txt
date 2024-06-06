自作ウィザードリィ風ゲームです，Unityを使いました
ゲームはコチラから遊べます
https://fujiwarayuusuke.github.io/TreasureHuntGame/

以下Asset内ファイル解説
Font 日本語用のフォント
Prefab 各オブジェクトのプレファブ
Scenes タイトル画面，操作説明画面，ゲーム画面のシーン
Script スクリプト
|- GameSceneScript ゲーム画面のスクリプト
|- InstructSceneScript 操作説明画面のスクリプト
|- Others その他ゲーム全体のスクリプト
|- StatSceneScript ゲーム開始画面のスクリプト

Sound　音声関係
|- BGM BGM
|- SE 効果音

Sprite ゲームで使った画像
|- 2D 画面右マップに用いた画像 
|- 3D 画面左プレイヤー視点に用いた画像
|- BackGround 背景

Stage ステージのテキスト表現，現在はステージをランダムで生成しているのでテキストファイルの意味はないが，テキスト経由で自分の作りたいステージが作れる

TextMesh Pro テキストを使うために必要な諸々，初期生成されたタイミングから特にいじっていない

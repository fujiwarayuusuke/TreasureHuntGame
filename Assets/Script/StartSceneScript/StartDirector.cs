using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//初期画面制御用のスクリプト
public class StartDirector : MonoBehaviour
{
    public static int height, width;

    private AudioSource audioSource;//ボタン押した時の効果音用

    const int easy = 7;//難易度ごとのマス数
    const int normal = 9;
    const int hard = 11;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //easy用ボタン
    public void Easy()
    {
        //Debug.Log("Start");
        height = easy;
        width = easy;
        audioSource.Play();//効果音
        Invoke(nameof(toGameScene), 0.2f);//効果音待ち
    }

    //normal用ボタン
    public void Normal()
    {
        //Debug.Log("Start");
        height = normal;
        width = normal;
        audioSource.Play();//効果音
        Invoke(nameof(toGameScene), 0.2f);//効果音待ち
    }

    //hard用ボタン
    public void Hard()
    {
        //Debug.Log("Start");
        height = hard;
        width = hard;
        audioSource.Play();//効果音
        Invoke(nameof(toGameScene), 0.2f);//効果音待ち
    }

    public void InstructButton()
    {
        //Debug.Log("Instruct");
        audioSource.Play();//効果音
        Invoke(nameof(toInstructionScene), 0.2f);//効果音待ち
    }

    void toGameScene()
    {
        SceneManager.LoadScene("GameScene");//ゲーム画面へと遷移
    }

    void toInstructionScene()
    {
        SceneManager.LoadScene("InstructScene");//操作説明画面へと遷移
    }
}

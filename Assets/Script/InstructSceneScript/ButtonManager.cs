using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private AudioSource audioSource;//ボタン押した時の効果音用
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //タイトルに戻る用ボタン
    public void TitleBackButton()
    {
        //Debug.Log("Start");
        audioSource.Play();//効果音

        Invoke(nameof(NextScene), 0.2f);
    }

    void NextScene()
    {
        SceneManager.LoadScene("StartScene");//ゲーム画面へと遷移
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonManager : MonoBehaviour
{
    private AudioSource audioSource;//�{�^�����������̌��ʉ��p
    
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //�^�C�g���ɖ߂�p�{�^��
    public void TitleBackButton()
    {
        //Debug.Log("Start");
        audioSource.Play();//���ʉ�
        Invoke(nameof(NextScene), 0.1f);
    }

    void NextScene()
    {
        SceneManager.LoadScene("StartScene");//�Q�[����ʂւƑJ��
    }

}

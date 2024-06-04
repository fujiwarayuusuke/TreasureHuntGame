using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private AudioSource audioSource;//�{�^�����������̌��ʉ��p
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    //�^�C�g���ɖ߂�p�{�^��
    public void TitleBackButton()
    {
        //Debug.Log("Start");
        audioSource.Play();//���ʉ�

        Invoke(nameof(NextScene), 0.2f);
    }

    void NextScene()
    {
        SceneManager.LoadScene("StartScene");//�Q�[����ʂւƑJ��
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//������ʐ���p�̃X�N���v�g
public class StartDirector : MonoBehaviour
{
    public static int height, width;

    private AudioSource audioSource;//�{�^�����������̌��ʉ��p

    const int easy = 7;//��Փx���Ƃ̃}�X��
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

    //easy�p�{�^��
    public void Easy()
    {
        //Debug.Log("Start");
        height = easy;
        width = easy;
        audioSource.Play();//���ʉ�
        Invoke(nameof(toGameScene), 0.2f);//���ʉ��҂�
    }

    //normal�p�{�^��
    public void Normal()
    {
        //Debug.Log("Start");
        height = normal;
        width = normal;
        audioSource.Play();//���ʉ�
        Invoke(nameof(toGameScene), 0.2f);//���ʉ��҂�
    }

    //hard�p�{�^��
    public void Hard()
    {
        //Debug.Log("Start");
        height = hard;
        width = hard;
        audioSource.Play();//���ʉ�
        Invoke(nameof(toGameScene), 0.2f);//���ʉ��҂�
    }

    public void InstructButton()
    {
        //Debug.Log("Instruct");
        audioSource.Play();//���ʉ�
        Invoke(nameof(toInstructionScene), 0.2f);//���ʉ��҂�
    }

    void toGameScene()
    {
        SceneManager.LoadScene("GameScene");//�Q�[����ʂւƑJ��
    }

    void toInstructionScene()
    {
        SceneManager.LoadScene("InstructScene");//���������ʂւƑJ��
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public MapGenerator mapGenerator;//MapGenerator�̊֐��ɃA�N�Z�X�ł���悤�ɂ���

    public GameObject successText, failText, toTitleButton, toTitleButtonFrame;//����T���̐������C���s���C�^�C�g���J�ڃ{�^���̃e�L�X�g�I�u�W�F�N�g
    public Text operationInstruction;
    //Start is called before the first frame update
    void Start()
    {
        //�ŏ��͂�������������Ȃ����
        toTitleButton.SetActive(false);
        toTitleButtonFrame.SetActive(false);
        successText.SetActive(false);
        failText.SetActive(false);
        operationInstruction.text = "�������L�[�ŕ����]���@���L�[�őO�i\r\n�X�y�[�X�L�[�Ŕ��@�@h�L�[�Ńq���g";
    }

    // Update is called once per frame
    void Update()
    {
        //�X�y�[�X�L�[�������ꂽ�ۂ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject player = GameObject.Find("Player(Clone)");
            if (!player.GetComponent<Player>().gameClear)//�܂��Q�[���������Ă���Ȃ��
            {
                //����T�������������Ȃ��(�v���[���[�̍��W�ƕ�̍��W�������Ȃ��)
                if (mapGenerator.GetSpaceType(player.GetComponent<Player>().playerPos) == MapGenerator.SpaceType.Treasure)
                {
                    successText.SetActive(true);//�N���A���b�Z�[�W��\��
                    operationInstruction.text = "�X�y�[�X�L�[�ł�����񂨕󂳂���";
                    toTitleButton.SetActive(true);//�^�C�g���J�ڃ{�^���\��
                    toTitleButtonFrame.SetActive(true);//�^�C�g���J�ڃ{�^���\��
                }
                else//����T�������s�����Ȃ��
                {
                    failText.SetActive(true);//���s�p�̃��b�Z�[�W��\��
                }
            }
            else//�Q�[�����N���A����Ă���Ȃ��
            {
                Start();//�������
            }

        }

        //�ړ��C�܂��͕����]�����N�����Ȃ��
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject player = GameObject.Find("Player(Clone)");
            if (!player.GetComponent<Player>().gameClear)//�܂��Q�[���������Ă���Ȃ��
            {
                failText.SetActive(false);//���s�p�̃��b�Z�[�W������
            }

        }
    }
}

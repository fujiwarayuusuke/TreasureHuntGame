using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�q���g�ɂ���ăv���C���[�̈ʒu���}�b�v�ɕ\�������邩�؂�ւ���(���C���[�Œ���)
public class hintContrller : MonoBehaviour
{
    GameObject player;//�v���C���[�̃Q�[���I�u�W�F�N�g
    bool isHint;//�ŏ��̓q���g����
    // Start is called before the first frame update
    void Start()
    {
        isHint = false;
    }

    // Update is called once per frame
    void Update()
    {
        //H���������ꂽ�Ȃ��
        if (Input.GetKeyDown(KeyCode.H) )
        {
            player = GameObject.Find("Player(Clone)");
            if (!player.GetComponent<Player>().gameClear && isHint)//�q���g��^���Ă����ԂȂ�΁C�v���C���[�̈ʒu���B��
            {
                player.GetComponent<Renderer>().sortingOrder = -2;
                isHint = !isHint;//�t���O�̐؂�ւ�
            }
            else if(!player.GetComponent<Player>().gameClear && !isHint)//�q���g��^���Ă��Ȃ���ԂȂ�΃v���C���[�̈ʒu������
            {
                player.GetComponent<Renderer>().sortingOrder = 1;
                isHint = !isHint;//�t���O�̐؂�ւ�
            }
        }

        //�X�y�[�X�L�[�������ꂽ�ۂ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject player = GameObject.Find("Player(Clone)");
            if (player.GetComponent<Player>().gameClear)//�Q�[�����I�����Ă���Ȃ��
            {
                
                Start();//�������
                
            }
        }
    }
}

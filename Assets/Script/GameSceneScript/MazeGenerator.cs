using System;
using System.Collections;
using System.Collections.Generic;

//�e�L�X�g�f�[�^�Ŗ��H�����
//����͌��@��@���̗p
public class MazeGenerator
{
    int[,] maze; //int�^�̓񎟌��z��Ŗ��H��\��
    int width; //����
    int height; //�c��
    const int floor = 0; //0�͒ʘH��\��
    const int wall = 1; //1�͕ǂ�\��
    
    enum Direction�@//�����@���Ă�������
    {
        Right,//�E
        Left, //��
        Up,   //��
        Down  //��
    }

    List<Cell> startCells = new List<Cell>();�@//�}�X�ڗp�N���X�̃��X�g

    //MazeGenerator�̃R���X�g���N�^
    public MazeGenerator(int w, int h)
    {
        if ( w < 5 ||  h < 5) throw new ArgumentOutOfRangeException();// ��5�����̏�����������H�͍��Ȃ�

        //�����̈�ƃ��[�v�\���𖳂������߁C���H�̑傫���͊�ɂ���
        if ( w % 2 == 0) w++; //���������Ȃ�+1���Ċ�ɂ���
        if ( h % 2 == 0) h++; //�c�������Ȃ�+1���Ċ�ɂ���

        this.width = w; //�n���Ă��������������ϐ�width�ɑ��
        this.height = h; //�n���Ă����c�����c���ϐ�height�ɑ��
        this.maze = new int[w, h]; //���H�p�̓񎟌��z����쐬
    }

    //���H�f�[�^�𐶐�����֐�
    public int[,] MazeGenerate()
    {
        //�O���ȊO��ǂ�,���������Ŗ��߂�
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    maze[x, y] = floor;//�O���C�ŏI�I�ɕǂɂ���
                }
                else
                {
                    maze[x, y] = wall;//����
                }
            }
        }

        //���W1�C1����@��
        Dig(1, 1);

        //�I�������O����ǂŖ��߂�
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

        //���H�f�[�^��Ԃ�
        return maze;
    }

    //�@�铹�����߂�֐�
    void Dig(int x, int y)
    {
        //�@������̗����̂��߁ARandom�̕ϐ�rnd���쐬
        Random rnd = new Random();

        //�@����
        while (true)
        {
            //�@�����p�̌���ێ����郊�X�g
            List<Direction> direction = new List<Direction>();

            //���݂̃}�X����2�}�X��܂ŕǂȂ��(�@���Ă����H�Ƃ��ċ@�\����Ȃ��)�@�����p�����X�g�Ɋi�[
            //�Z���]���ň�ڂ�false�Ȃ�Γ�ڂ̔��f�łɃ}�X���Ȃ��Ƃ��]������Ȃ�
            if (maze[x + 1, y] == wall && maze[x + 2, y] == wall)
                direction.Add(Direction.Right);
            if (maze[x - 1, y] == wall && maze[x - 2, y] == wall)
                direction.Add(Direction.Left);
            if (maze[x, y - 1] == wall && maze[x, y - 2] == wall)
                direction.Add(Direction.Up);
            if (maze[x, y + 1] == wall && maze[x, y + 2] == wall)
                direction.Add(Direction.Down);
            

            //���̎��_�Ō@�����p���Ȃ���΃��[�v�𔲂���
            if (direction.Count == 0) break;

            //���[�v�𔲂��Ȃ���Γ����Z�b�g
            SetPath(x, y);

            //�@����p�������_���Ŏ擾
            int directionIndex = rnd.Next(direction.Count);

            //�擾�������p�ɂ���Č@��i�߂�
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

            //���̋N�_���擾
            Cell cell = GetStartCell();

            //�����N�_�������Dig�֐������g�ŌĂяo��
            if (cell != null)
            {
                Dig(cell.X, cell.Y);
            }

        }
    }

    //�����@�莟�̋N�_�������߂�֐�
    void SetPath(int x, int y)
    {
        //�󂯎�������W��floor�i���j�ɂ���
        maze[x, y] = floor;
        //���W���c�����Ɋ�Ȃ玟�̋N�_���ɒǉ�����
        if (x % 2 == 1 && y % 2 == 1)
        {
            startCells.Add(new Cell() { X = x, Y = y });
        }
    }

    //�N�_�ʒu�������_���őI������֐�
    Cell GetStartCell()
    {
        //�����i�[���ꂽ�N�_���X�g��0�Ȃ�null��Ԃ��ďI���
        if (startCells.Count == 0) return null;

        //�N�_�������_���őI�Ԃ��߂�Random�^�ϐ�
        Random rnd = new Random();
        //�N�_���X�g�̃C���f�b�N�X�������_���Ɏ擾
        int idx = rnd.Next(startCells.Count);
        //�����_���ŋN�_���擾��Cell�^�̕ϐ��Ɋi�[
        Cell cell = startCells[idx];
        //�擾�����N�_�����X�g����폜
        startCells.RemoveAt(idx);
        //�V���ȋN�_��Ԃ�
        return cell;
    }
}

//�}�X�ڗp�̃N���X
public class Cell
{
    public int X, Y;
}
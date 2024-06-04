using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�}�b�v��Map�C�}�X�̂��Ƃ�Space�ƌĂԂ��Ƃɂ���
public class MapGenerator : MonoBehaviour
{
    //�}�b�v���e�L�X�g�ŕ\���@�e�L�X�g����0�����C�P���ǁC2���v���C���[�̃}�X�C","�����ڂ̋�؂��\��
    [SerializeField] TextAsset mapText;
    //5 * 5�̃}�b�v�̈��Ƃ��Ă͈ȉ�
    // 1,1,1,1,1
    // 1,0,0,1,1
    // 1,0,1,1,1
    // 1,0,0,2,1
    // 1,1,1,1,1

    //�I�u�W�F�N�g�̊G�̃v���t�@�u�@�z���0�����C�P���ǁC2���v���C���[�̃h�b�g�G�C3�������\��
    [SerializeField] GameObject[] prefabs;

    //���H�̊e�v���t�@�u�̐e�v�f
    [SerializeField] Transform map2D;

    //3D���_�̕ǉ摜�̂��߂̔z��,�v���C���[���猩�Ď�O�̉摜����Ɋi�[�����悤�ɂ���
    [SerializeField] WallArr[] wallArr;

    //�}�b�v�̏c���̒�����\��
    public int row, col;

    //�}�b�v�̑傫���̗����ێ��p�̕⏕�ϐ�
    public int h, w;

    //���S���W�̈ʒu��\��
    Vector2 centerPos;

    //��}�X���Ƃ̕���\��
    float spaceSize;

    MazeGenerator maze; //MazeGenerator�^�̕ϐ����`
    int[,] mazeData; //���H�f�[�^�p��int�^�̓񎟌��z��̕ϐ����`
    GameObject[] objects;//2D�}�b�v���쎞�̃I�u�W�F�N�g��ۊǂ��Ă����ϐ��C�I�u�W�F�N�g�j��p

    //�e�}�X��\���񋓌^,�e�L�X�g�ł�0,1,2�����񂾂��C�v���O�����ň����₷���悤�񋓌^�ŕ\��
    public enum SpaceType
    {
        Floor, //0...Floor
        Wall,   //1...Wall
        Player,  //2...Player
        Treasure //3...Treasure
    }
    SpaceType[,] map; // �e�}�X�̓񎟌��z�񂪃}�b�v�ƂȂ�

    //���W����͂Ƃ��đΉ�����map�̈ʒu�̗񋓌^�̒l��Ԃ��֐�
    public SpaceType GetSpaceType(Vector2Int pos)
    {
        return map[pos.x, pos.y];
    }

    // Start is called before the first frame update
    void Start()
    {
        h = StartDirector.height;
        w = StartDirector.width;
        
        maptextToSpceType();//map���쐬
       
        makeMap(); //�}�b�v��\��
        
        //�}�b�v�̕\���ʒu
        map2D.position = new Vector3(0, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //�X�y�[�X�L�[�������ɃQ�[���N���A�ς݂ł���΂������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            

            GameObject player = GameObject.Find("Player(Clone)");
            if (player.GetComponent<Player>().gameClear)
            {
                //2D�}�b�v���폜
                clear2DView();
                Start();
            }
        }
    }

    //map���쐬����֐�
    private void maptextToSpceType()
    {
        //MazeGenerator���C���X�^���X��
        maze = new MazeGenerator(h, w);
        //���H�f�[�^�p�񎟌��z��𐶐�
        mazeData = new int[h, w];
        //���H�f�[�^�쐬���擾
        mazeData = maze.MazeGenerate();
        //�}�b�v�̉��̒����擾
        row = mazeData.GetLength(1);
        //�}�b�v�̏c�̒����擾
        col = mazeData.GetLength(0);
        //�}�b�v�e�[�u��������
        map = new SpaceType[col, row];
        //�Q�[���I�u�W�F�N�g�̔z���������
        objects = new GameObject[row * col�@+ 2];//�e�}�X�̏��ƕǁC�X�ɂ���ƃv���C���[�I�u�W�F�N�g��+2

        //�}�b�v�e�[�u���쐬
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                //���H�f�[�^��SpaceType�ɃL���X�g���ă}�b�v�e�[�u���Ɋi�[
                map[i, j] = (SpaceType)mazeData[i , j];
            }
        }


        //�e�L�X�g�f�[�^����}�b�v�����ۂ͈ȉ��̃R�����g�A�E�g���O��

        //var lineSeparator = new char[] { '\n', '\r' };//��������ׂ����s�R�[�h
        //var spaceSeparator = new char[] { ',' };//�}�X�̊Ԃ̖�������ׂ��R���}

        ////�e�L�X�g�f�[�^���e�s���Ƃɕ���
        //string[] mapLines = mapText.text.Split(lineSeparator, System.StringSplitOptions.RemoveEmptyEntries);

        ////�s�̐�
        //int numRow = mapLines.Length;
        ////��̐�
        //int numCol = mapLines[0].Split(new char[] { ',' }).Length;
        ////numRow * numCol�̃}�X������map��������
        //map = new SpaceType[numCol, numRow];

        ////�e�s���Ƃɕ���
        //for(int i = 0; i< numRow; i++)
        //{
        //    //1�}�X���Ƃɕ���
        //    string[] spaceValue = mapLines[i].Split(spaceSeparator);

        //    //�e0,1,2��enum�^�̒l�ɕϊ�
        //    for (int j = 0; j < numCol; j++)
        //    {
        //        //�z��map��i�sj��ڂւ̑��
        //        //spaceValue��j�Ԗڂ𐮐��ɃL���X�g���C�X��SpaceType�^�ɃL���X�g
        //        map[i, j] = (SpaceType) int.Parse(spaceValue[j]);
        //    }
        //}
    }

    private void makeMap()
    {
        //�h�b�g�G�̃T�C�Y���擾����
        spaceSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;

        //���S���W���擾
        calcuCenter();

        //map�̊e�s�Ń��[�v
        for (int i = 0; i< map.GetLength(0); i++)
        {
            //map�̊e��Ń��[�v
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //���̍��W
                Vector2Int tantativePos = new Vector2Int(i, j);

                //map[x,y]�ɓ�����摜��prefabs�����玝���Ă���map2D�̎q�v�f�Ƃ��ăC���X�^���X��
                GameObject mapObject = Instantiate(prefabs[(int)map[i, j]], map2D);
                //�I�u�W�F�N�g��z��Ɋi�[
                objects[j + i * map.GetLength(0)] = mapObject;
                //mapObject�̈ʒu��K�؂Ɉړ�
                //realPos�֐��ɂ��摜�̑傫�����l�������ʒu�ɕ\�������
                mapObject.transform.position = realPos(tantativePos);


                ////���W��1�C1�Ȃ�
                //if (i == 1 && j == 1)
                //{
                //    //�v���C���[�𐶐�
                //    GameObject player = Instantiate(prefabs[2], map2D);
                //    //�|�W�V�����C��
                //    player.transform.position = realPos(tantativePos);
                //    player.GetComponent<Player>().playerPos = tantativePos;
                //    //�v���C���[��mapGenerator��ݒ�
                //    player.GetComponent<Player>().mapGenerator = this;
                //}

                //�e�L�X�g�f�[�^����}�b�v�����ۂ͈ȉ��̃R�����g�A�E�g���O��
                ////�v���[���[�̃}�X�̏ꍇ�͏��������ɕ\��
                //if (map[i, j] == SpaceType.Player)
                //{
                //    //���̉摜��prefabs�����玝���Ă��ăC���X�^���X��
                //    GameObject floorObject = Instantiate(prefabs[(int) SpaceType.Floor], this.transform);
                //    //mapObject�̈ʒu��K�؂Ɉړ�
                //    floorObject.transform.position = realPos(tantativePos);

                //    //�v���C���[�̍��W����
                //    mapObject.GetComponent<Player>().playerPos = tantativePos;
                //}
            }
        }


        //�v���C���[�̏����ʒu�̐ݒ�
        while (true)
        {
            //�v���C���[�̏����ʒu�͕ǂł͂Ȃ��Ƃ��납�烉���_���Ɍ���
            Vector2Int randomPos = new Vector2Int(Random.Range(1, w), Random.Range(1, h));
            if(GetSpaceType(randomPos) == SpaceType.Floor)
            {
                //�v���C���[�𐶐�
                GameObject player = Instantiate(prefabs[2], map2D);
                //�|�W�V�����C��
                player.transform.position = realPos(randomPos);
                player.GetComponent<Player>().playerPos = randomPos;
                //�ŏ��̓v���C���[�̈ʒu���B���Ă���
                player.GetComponent<Renderer>().sortingOrder = -2;
                //�v���C���[��mapGenerator��ݒ�
                player.GetComponent<Player>().mapGenerator = this;
                //�v���C���[�I�u�W�F�N�g���i�[
                objects[map.GetLength(1) * map.GetLength(0)] = player;
                break;
            }
        }

        //����̈ʒu�̐ݒ�
        while (true)
        {
            //����̏����ʒu�͕ǂł͂Ȃ��Ƃ��납�烉���_���Ɍ���
            Vector2Int randomPos = new Vector2Int(Random.Range(1, w), Random.Range(1, h));
            if (GetSpaceType(randomPos) == SpaceType.Floor)
            {
                //�v���C���[�𐶐�
                GameObject treasure = Instantiate(prefabs[3], map2D);
                //�|�W�V�����C��
                treasure.transform.position = realPos(randomPos);
                //�}�b�v�����X�V
                map[randomPos.x, randomPos.y] = SpaceType.Treasure;
                //����̃I�u�W�F�N�g���i�[
                objects[map.GetLength(1) * map.GetLength(0) + 1] = treasure;
                break;
            }
        }



        //���S���W���v�Z����֐�
        //�e�s�C�e�񐔂̔����ɉ摜�̃T�C�Y��������
        void calcuCenter()
        {
            //�s�������̏ꍇ�͉摜�̔����̃T�C�Y�������������K�v
            if (map.GetLength(0) % 2 == 0)
            {
                centerPos.x = map.GetLength(0) / 2 * spaceSize - (spaceSize / 2);
            }
            else//�s����̏ꍇ
            {
                centerPos.x = map.GetLength(0) / 2 * spaceSize;
            }

            //������l�Ɍv�Z����
            if (map.GetLength(1) % 2 == 0)
            {
                centerPos.y = map.GetLength(1) / 2 * spaceSize - (spaceSize / 2);
            }
            else
            {
                centerPos.y = map.GetLength(1) / 2 * spaceSize;
            }
        }
    }

    //���̍��W����v���t�@�u�̓K�؂Ȉʒu��Ԃ��֐�
    public Vector2 realPos(Vector2Int pos)
    {
        //���̍��W * �摜�̑傫�� �̌v�Z�Ő^�̍��W���擾
        //map�𒆐S�Ɉڂ����߂�centerPos�̒l�ň����Z
        return new Vector2(pos.x * spaceSize - centerPos.x, -(pos.y * spaceSize - centerPos.x));
    }

    //3D��ʂ̕ǂ�\��������֐�,
    //�������������قǎ�O�̉摜��\��������
    public void View3D(int index)
    {
        //Debug.Log(index);
        foreach (GameObject wallpaper in wallArr[index].wall)
        {
            wallpaper.SetActive(true);
            
        }
    }

    //�ړ����C�����]�����ɂ���܂ŕ\�����Ă����ǉ摜���������߂̊֐�
    public void ResetView3D()
    {
        foreach (WallArr walls in wallArr)
        {
            foreach (GameObject wallpaper in walls.wall)
            {
                wallpaper.SetActive(false);
            }
        }
    }

    //�N���A���2D�}�b�v�̃I�u�W�F�N�g��S�폜
    private void clear2DView()
    {
        foreach(GameObject spaceTile in objects)
        {
            Destroy(spaceTile);
        }
    }
}

//�ǉ摜�p�̃N���X�CGameObject�̔z��Ȃ̂ŕ����̉摜���ꏏ�ɓZ�߂邱�Ƃ��ł���
[System.Serializable]
public class WallArr
{
    public GameObject[] wall;
}

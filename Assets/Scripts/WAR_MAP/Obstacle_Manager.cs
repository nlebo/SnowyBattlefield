using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Manager : MonoBehaviour {

    #region VARIABLE
    struct xy
    {
        public int x;
        public int y;
    }

    public GameObject[] Obstacles;
    List<xy> XY = new List<xy>();
    Tile_Manager Tile;
    int SpawnX, SpawnY, ESpawnX, ESpawnY;


    int Debris_MAX=50,Debris_MIN = 30;
    int HalfCover_MAX = 30, HalfCover_MIN = 20;
    int HighCover_MAX = 20, HighCover_MIN = 15;
    int Straw_MAX = 20, Straw_MIN = 10;
    int Hut_MAX = 5, Hut_MIN = 0;

    Rect rect;

    #endregion

    // Use this for initialization
    void Start () {
        Tile = GetComponent<Tile_Manager>();
        RectFill();
        MakeDebris();
        MakeHalfCover();
        MakeHighCover();
        MakeStraw();
        MakeHut();
		GetComponent<SpawnPocket>().SpawnPlayer();
        Board_Manager.m_Board_Manager.Loading++;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void RectFill()
    {
        SpawnPocket _Spawn = GetComponent<SpawnPocket>();
        SpawnX = _Spawn.x;
        SpawnY = _Spawn.y;
        ESpawnX = _Spawn.ex;
        ESpawnY = _Spawn.ey;

        if (SpawnX > ESpawnX)
        {
            rect.xMin = ESpawnX;
            rect.xMax = SpawnX;
        }
        else
        {
            rect.xMin = SpawnX;
            rect.xMax = ESpawnX;
        }

        if (SpawnY > ESpawnY)
        {
            rect.yMin = ESpawnY;
            rect.yMax = SpawnY;
        }
        else
        {
            rect.yMin = SpawnY;
            rect.yMax = ESpawnY;
        }
    }

    
    //---------- Make Covers --------------------

    bool MakeDebris()
    {
        

        int count = 0;
        int Debris_Count = Random.Range(Debris_MIN, Debris_MAX);
        int PX = 0, PY = 0;

        #region RectBox Insert to List
        if (rect.xMax - rect.xMin < 15)
            PX = 15 - (int)(rect.xMax - rect.xMin);

        if (rect.yMax - rect.yMin < 15)
            PY = 15 - (int)(rect.yMax - rect.yMin);

        PX = PX / 2;
        PY = PY / 2;


        for (int i = (int)rect.xMin - PX; i < (int)rect.xMax + PX; i++)
        {
            for (int j = (int)rect.yMin - PY; j < (int)rect.yMax + PY; j++)
            {
                xy _XY = new xy();
                _XY.x = i;
                _XY.y = j;

                XY.Add(_XY);
            }
        }
        #endregion


        for (int i =0; i< Debris_Count * 0.6;i++)
        {

            if (XY.Count == 0)
                break;
            int where = Random.Range(0, XY.Count);

            if (Tile.TileMap[XY[where].x][XY[where].y] == 0)
            {
                int What = Random.Range(1, 4);
                int _Check = 0;
                int _Count = 0;

                #region Select_Type
                while (true)
                {
                    switch (What)
                    {
                        case 1:
                            //Tile.MY_Tile[XY[where].x][XY[where].y].GetComponent<SpriteRenderer>().color = Color.green;
                            //Instantiate(Obstacles[0], Tile.MY_Tile[XY[where].x][XY[where].y].transform);
                            Tile.TileMap[XY[where].x][XY[where].y] = Tile_Manager.Cover_Kind.Debris;
                            count++;
                            _Check = 1;
                            break;
                        case 2:
                            if (CheckDebris_TYPE2(XY[where]))
                            {
                                count += 2;
                                _Check = 1;
                            }
                            break;
                        case 3:
                            if (CheckDebris_TYPE3(XY[where]))
                            {
                                count += 4;
                                _Check = 1;
                            }
                            break;


                    }

                    if (_Check == 1)
                    {
                        XY.Remove(XY[where]);
                        break;
                    }

                    What++;
                    if (What > 3) What = 1;

                    _Count++;
                    if (_Count >= 3)
                    {
                        i--;
                        break;
                    }
                }
                #endregion

            }
            else i--;

            
            

            
        }

        while (count < Debris_Count)
        {
            int x = Random.Range(0, Tile.X-1);
            int y = Random.Range(0, Tile.Y-1);

            if (Tile.TileMap[x][y] != 0)
                continue;

            Tile.TileMap[x][y] = Tile_Manager.Cover_Kind.Debris;
            //Instantiate(Obstacles[0], Tile.MY_Tile[x][y].transform);
            //Tile.MY_Tile[x][y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0,0,0,-0.5f);

            count++;
        }

        

        return true;
    }
    bool MakeHalfCover()
    {

        int count = 0;
        int HalfCover_Count = Random.Range(HalfCover_MIN, HalfCover_MAX);

        for (int i = 0; i < HalfCover_Count * 0.6; i++)
        {
            if (XY.Count == 0)
                break;

            int where = Random.Range(0, XY.Count);

            if (Tile.TileMap[XY[where].x][XY[where].y] == 0)
            {
                int What = Random.Range(1, 5);
                int _Check = 0;
                int _Count = 0;

                #region Select_Type
                while (true)
                {
                    switch (What)
                    {
                        case 1:
                            // Tile.MY_Tile[XY[where].x][XY[where].y].GetComponent<SpriteRenderer>().color = Color.yellow;
                            //Instantiate(Obstacles[1], Tile.MY_Tile[XY[where].x][XY[where].y].transform);
                            Tile.TileMap[XY[where].x][XY[where].y] = Tile_Manager.Cover_Kind.HalfCover;
                            count++;
                            _Check = 1;
                            break;
                        case 2:
                            if (CheckHalfCover_TYPE2(XY[where]))
                            {
                                count += 2;
                                _Check = 1;
                            }
                            break;
                        case 3:
                            if (CheckHalfCover_TYPE3(XY[where]))
                            {
                                count += 3;
                                _Check = 1;
                            }
                            break;
                        case 4:
                            if (CheckHalfCover_TYPE4(XY[where]))
                            {
                                count += 6;
                                _Check = 1;
                            }
                            break;

                    }

                    if (_Check == 1)
                    {
                        XY.Remove(XY[where]);
                        break;
                    }

                    What++;
                    if (What > 4) What = 1;

                    _Count++;
                    if (_Count >= 4)
                    {
                        i--;
                        break;
                    }
                }
                #endregion
            }
            else i--;

            if (where < XY.Count)
                XY.Remove(XY[where]);


        }

        while (count < HalfCover_Count)
        {
            int x = Random.Range(0, Tile.X - 1);
            int y = Random.Range(0, Tile.Y - 1);

            if (Tile.TileMap[x][y] != 0)
                continue;

            Tile.TileMap[x][y] = Tile_Manager.Cover_Kind.HalfCover;
            //Instantiate(Obstacles[1], Tile.MY_Tile[x][y].transform);
            //Tile.MY_Tile[x][y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0, 0, -0.5f);

            count++;
        }
        return true;
    }
    bool MakeHighCover()
    {

        int count = 0;
        int HighCover_Count = Random.Range(HighCover_MIN, HighCover_MAX);

        for (int i = 0; i < HighCover_Count * 0.6; i++)
        {

            if (XY.Count == 0)
                break;

            int where = Random.Range(0, XY.Count);

            if (Tile.TileMap[XY[where].x][XY[where].y] == 0)
            {
                //Tile.MY_Tile[XY[where].x][XY[where].y].GetComponent<SpriteRenderer>().color = Color.white;
                Tile.TileMap[XY[where].x][XY[where].y] = Tile_Manager.Cover_Kind.HighCover;
                count++;
            }
            else i--;

            XY.Remove(XY[where]);


        }

        while (count < HighCover_Count)
        {
            int x = Random.Range(0, Tile.X - 1);
            int y = Random.Range(0, Tile.Y - 1);

            if (Tile.TileMap[x][y] != 0)
                continue;

            Tile.TileMap[x][y] = Tile_Manager.Cover_Kind.HighCover;
            //Tile.MY_Tile[x][y].GetComponent<SpriteRenderer>().color = Color.white + new Color(0, 0, 0, -0.5f);

            count++;
        }
        return true;
    }

    bool CheckDebris_TYPE2(xy _XY)
    {
        int where = Random.Range(1,5);
        int check = 0;
        int count = 0;
        while (true)
        {
            switch (where)
            {
                #region left
                case 1: // left
                    if (_XY.y - 1 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 2 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0 
                        || Tile.TileMap[_XY.x - 2][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x][_XY.y + 1 ] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                        || Tile.TileMap[_XY.x - 1][_XY.y + 1 ] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 1] != 0)
                        break;
                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f,0,0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;
                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;

                    check = 1;
                    break;
                #endregion

                #region right
                case 2: // right
                    if (_XY.y - 1 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 2 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0 
                        || Tile.TileMap[_XY.x + 2][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x][_XY.y + 1] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x + 1][_XY.y - 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;
                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region up
                case 3: // up
                    if (_XY.y - 1 < 0 || _XY.y + 2 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x][_XY.y + 1] != 0 
                        || Tile.TileMap[_XY.x][_XY.y + 2] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y + 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;
                    //Tile.MY_Tile[_XY.x][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y + 1].transform);
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.Debris;

                    Tile.TileMap[_XY.x][_XY.y + 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;

                    check = 1;
                    break;
                #endregion

                #region down
                case 4:
                    if (_XY.y - 2 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x][_XY.y - 1] != 0 
                        || Tile.TileMap[_XY.x][_XY.y - 2] != 0 || Tile.TileMap[_XY.x][_XY.y + 1] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y - 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;
                    //Tile.MY_Tile[_XY.x][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y - 1].transform);
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.Debris;

                    Tile.TileMap[_XY.x][_XY.y - 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;

                    check = 1;
                    break;
                    #endregion

            }
            if (check == 1)
                break;

            where++;
            if (where > 4) where = 1;
            count++;

            if (count >= 4)
                return false;
        }
        return true;
    }
    bool CheckDebris_TYPE3(xy _XY)
    {
        int where = Random.Range(1, 5);
        int check = 0;
        int count = 0;
        while (true)
        {
            switch (where)
            {
                #region left_right_up
                case 1: // left_right_up
                    if (_XY.y - 1 < 0 || _XY.y + 2 >= Tile.Y || _XY.x - 3 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 2][_XY.y] != 0 || Tile.TileMap[_XY.x - 3][_XY.y] != 0 
                        || Tile.TileMap[_XY.x][_XY.y + 1] != 0 || Tile.TileMap[_XY.x][_XY.y + 2] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x - 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x - 2][_XY.y].transform);
                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y + 1].transform);
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.Debris;
                    //--------------------------------------------------------------
                    Tile.TileMap[_XY.x - 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region right_right_up
                case 2: // right_right_up
                    if (_XY.y - 1 < 0 || _XY.y + 2 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 3 >= Tile.X) break;


                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 2][_XY.y] != 0
                         || Tile.TileMap[_XY.x + 3][_XY.y] != 0 
                         || Tile.TileMap[_XY.x][_XY.y + 1] != 0 || Tile.TileMap[_XY.x][_XY.y + 2] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                         || Tile.TileMap[_XY.x - 1][_XY.y] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x + 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x + 2][_XY.y].transform);
                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y + 1].transform);
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.Debris;
                    //--------------------------------------------------------------
                    Tile.TileMap[_XY.x + 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region left_right_down
                case 3: // left_right_down
                    if (_XY.y - 2 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 3 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 2][_XY.y] != 0
                         || Tile.TileMap[_XY.x - 3][_XY.y] != 0
                         || Tile.TileMap[_XY.x][_XY.y - 1] != 0 || Tile.TileMap[_XY.x][_XY.y - 2] != 0 || Tile.TileMap[_XY.x][_XY.y + 1] != 0
                         || Tile.TileMap[_XY.x + 1][_XY.y] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x - 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x - 2][_XY.y].transform);
                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y - 1].transform);
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.Debris;
                    //--------------------------------------------------------------
                    Tile.TileMap[_XY.x - 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region right_right_down
                case 4: // right_right_down
                    if (_XY.y - 2 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 3 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 2][_XY.y] != 0
                         || Tile.TileMap[_XY.x + 3][_XY.y] != 0
                         || Tile.TileMap[_XY.x][_XY.y - 1] != 0 || Tile.TileMap[_XY.x][_XY.y - 2] != 0 || Tile.TileMap[_XY.x][_XY.y + 1] != 0
                         || Tile.TileMap[_XY.x - 1][_XY.y] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x + 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x + 2][_XY.y].transform);
                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.Debris;

                    //Tile.MY_Tile[_XY.x][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.green + new Color(0.4f, 0, 0.4f);
                    //Instantiate(Obstacles[0], Tile.MY_Tile[_XY.x][_XY.y - 1].transform);
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.Debris;
                    //--------------------------------------------------------------
                    Tile.TileMap[_XY.x + 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                    #endregion

            }
            if (check == 1)
                break;

            where++;
            if (where > 4) where = 1;
            count++;

            if (count >= 4)
                return false;
        }
        return true;
    }

    bool CheckHalfCover_TYPE2(xy _XY)
    {
        int where = Random.Range(1, 5);
        int check = 0;
        int count = 0;
        while (true)
        {
            switch (where)
            {
                #region left
                case 1: // left
                    if (_XY.y - 1 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 2 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x - 2][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x][_XY.y + 1] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                        || Tile.TileMap[_XY.x - 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 1] != 0)
                        break;
                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;

                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;

                    check = 1;
                    break;
                #endregion

                #region right
                case 2: // right
                    if (_XY.y - 1 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 2 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0
                       || Tile.TileMap[_XY.x + 2][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y] != 0
                       || Tile.TileMap[_XY.x][_XY.y + 1] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                       || Tile.TileMap[_XY.x + 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x + 1][_XY.y - 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);

                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;

                    check = 1;
                    break;
                #endregion

                #region up
                case 3: // up

                    if (_XY.y - 1 < 0 || _XY.y + 2 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x][_XY.y + 1] != 0
                        || Tile.TileMap[_XY.x][_XY.y + 2] != 0 || Tile.TileMap[_XY.x][_XY.y - 1] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y + 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    //Tile.MY_Tile[_XY.x][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y + 1].transform);

                    Tile.TileMap[_XY.x][_XY.y + 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;

                    check = 1;
                    break;
                #endregion

                #region down
                case 4:
                    if (_XY.y - 2 < 0 || _XY.y + 1 >= Tile.Y || _XY.x - 1 < 0 || _XY.x + 1 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x][_XY.y - 1] != 0
                         || Tile.TileMap[_XY.x][_XY.y - 2] != 0 || Tile.TileMap[_XY.x][_XY.y + 1] != 0
                         || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y] != 0
                         || Tile.TileMap[_XY.x + 1][_XY.y - 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);
                    //Tile.MY_Tile[_XY.x][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0);
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y - 1].transform);

                    Tile.TileMap[_XY.x][_XY.y - 2] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                    #endregion

            }
            if (check == 1)
                break;

            where++;
            if (where > 4) where = 1;
            count++;

            if (count >= 4)
                return false;
        }
        return true;
    }
    bool CheckHalfCover_TYPE3(xy _XY)
    {
        int where = Random.Range(1, 5);
        int check = 0;
        int count = 0;
        while (true)
        {
            switch (where)
            {
                #region left
                case 1: // left
                    if (_XY.x - 3 < 0 || _XY.x + 1 >= Tile.X) break;
                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 2][_XY.y] != 0 || Tile.TileMap[_XY.x - 3][_XY.y] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x - 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 2][_XY.y].transform);


                    Tile.TileMap[_XY.x - 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region right
                case 2: // right
                    if (_XY.x - 1 < 0 || _XY.x + 3 >= Tile.X) break;

                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 2][_XY.y] != 0 || Tile.TileMap[_XY.x + 3][_XY.y] != 0
                        || Tile.TileMap[_XY.x - 1][_XY.y] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x + 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 2][_XY.y].transform);

                    Tile.TileMap[_XY.x + 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region up
                case 3: // up
                    if (_XY.y - 1 < 0 || _XY.y + 3 >= Tile.Y) break;

                    if (Tile.TileMap[_XY.x][_XY.y + 1] != 0 || Tile.TileMap[_XY.x][_XY.y + 2] != 0 || Tile.TileMap[_XY.x][_XY.y + 3] != 0
                        || Tile.TileMap[_XY.x][_XY.y - 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y + 1].transform);

                    //Tile.MY_Tile[_XY.x][_XY.y + 2].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y + 2] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y + 2].transform);



                    Tile.TileMap[_XY.x][_XY.y + 3] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region down
                case 4:
                    if (_XY.y - 3 < 0 || _XY.y + 1 >= Tile.Y) break;

                    if (Tile.TileMap[_XY.x][_XY.y - 1] != 0 || Tile.TileMap[_XY.x][_XY.y - 2] != 0 || Tile.TileMap[_XY.x][_XY.y - 3] != 0
                        || Tile.TileMap[_XY.x][_XY.y + 1] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y - 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y - 1].transform);

                    //Tile.MY_Tile[_XY.x][_XY.y - 2].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0, 0.4f, 0.4f);
                    Tile.TileMap[_XY.x][_XY.y - 2] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y - 2].transform);



                    Tile.TileMap[_XY.x][_XY.y - 3] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x][_XY.y + 1] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                    #endregion

            }
            if (check == 1)
                break;

            where++;
            if (where > 4) where = 1;
            count++;

            if (count >= 4)
                return false;
        }
        return true;
    }
    bool CheckHalfCover_TYPE4(xy _XY)
    {
        int where = Random.Range(1, 5);
        int check = 0;
        int count = 0;
        while (true)
        {
            switch (where)
            {
                #region left_up
                case 1: // left_up
                    if (_XY.x - 3 < 0 || _XY.x + 1 >= Tile.X || _XY.y + 4 >= Tile.Y) break;
                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 2][_XY.y] != 0
                        || Tile.TileMap[_XY.x - 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y + 2] != 0 || Tile.TileMap[_XY.x - 1][_XY.y + 3] != 0
                        || Tile.TileMap[_XY.x - 3][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y + 4] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x - 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 2][_XY.y].transform);

                    //----------------------------------------------------------------

                    //Tile.MY_Tile[_XY.x - 1][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y + 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y + 1].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y + 2].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y + 2] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y + 2].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y + 3].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y + 3] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y + 3].transform);

                    //-------------------------------------------------------------
                    Tile.TileMap[_XY.x - 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y + 4] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region right_up
                case 2: // right_up

                    if (_XY.x - 1 < 0 || _XY.x + 3 >= Tile.X || _XY.y + 4 >= Tile.Y) break;

                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 2][_XY.y] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y + 1] != 0 || Tile.TileMap[_XY.x + 1][_XY.y + 2] != 0 || Tile.TileMap[_XY.x + 1][_XY.y + 3] != 0
                        || Tile.TileMap[_XY.x + 3][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y + 4] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x + 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 2][_XY.y].transform);

                    //----------------------------------------------------------------

                    //Tile.MY_Tile[_XY.x + 1][_XY.y + 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y + 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y + 1].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y + 2].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y + 2] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y + 2].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y + 3].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y + 3] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y + 3].transform);

                    //-------------------------------------------------------------
                    Tile.TileMap[_XY.x + 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y + 4] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region left_down
                case 3: // left_down
                    if (_XY.x - 3 < 0 || _XY.x + 1 >= Tile.X || _XY.y - 4 < 0) break;

                    if (Tile.TileMap[_XY.x - 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 2][_XY.y] != 0
                        || Tile.TileMap[_XY.x - 1][_XY.y - 1] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 2] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 3] != 0
                        || Tile.TileMap[_XY.x - 3][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x - 1][_XY.y - 4] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x - 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 2][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 2][_XY.y].transform);

                    //----------------------------------------------------------------

                    //Tile.MY_Tile[_XY.x - 1][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y - 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y - 1].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y - 2].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y - 2] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y - 2].transform);

                    //Tile.MY_Tile[_XY.x - 1][_XY.y - 3].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x - 1][_XY.y - 3] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x - 1][_XY.y - 3].transform);

                    //-------------------------------------------------------------
                    Tile.TileMap[_XY.x - 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y - 4] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                #endregion

                #region right_down
                case 4://right_down
                    if (_XY.x - 1 < 0 || _XY.x + 3 >= Tile.X || _XY.y - 4 < 0) break;

                    if (Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 2][_XY.y] != 0
                        || Tile.TileMap[_XY.x + 1][_XY.y - 1] != 0 || Tile.TileMap[_XY.x + 1][_XY.y - 2] != 0 || Tile.TileMap[_XY.x + 1][_XY.y - 3] != 0
                        || Tile.TileMap[_XY.x + 3][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y] != 0 || Tile.TileMap[_XY.x + 1][_XY.y - 4] != 0)
                        break;

                    //Tile.MY_Tile[_XY.x][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y].transform);

                    //Tile.MY_Tile[_XY.x + 2][_XY.y].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 2][_XY.y] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 2][_XY.y].transform);

                    //----------------------------------------------------------------

                    //Tile.MY_Tile[_XY.x + 1][_XY.y - 1].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y - 1] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y - 1].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y - 2].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y - 2] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y - 2].transform);

                    //Tile.MY_Tile[_XY.x + 1][_XY.y - 3].GetComponent<SpriteRenderer>().color = Color.yellow + new Color(0.4f, 0.4f, 0.6f);
                    Tile.TileMap[_XY.x + 1][_XY.y - 3] = Tile_Manager.Cover_Kind.HalfCover;
                    //Instantiate(Obstacles[1], Tile.MY_Tile[_XY.x + 1][_XY.y - 3].transform);

                    //-------------------------------------------------------------
                    Tile.TileMap[_XY.x + 3][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x - 1][_XY.y] = Tile_Manager.Cover_Kind.CanNot;
                    Tile.TileMap[_XY.x + 1][_XY.y - 4] = Tile_Manager.Cover_Kind.CanNot;
                    check = 1;
                    break;
                    #endregion

            }
            if (check == 1)
                break;

            where++;
            if (where > 4) where = 1;
            count++;

            if (count >= 4)
                return false;
        }
        return true;
    }

    //---------- Make Decorate -------------------

    bool MakeStraw()
    {

        int count = 0;
        int Straw_Count = Random.Range(Straw_MIN, Straw_MAX);

        while (count < Straw_Count)
        {
            int x = Random.Range(0, Tile.X - 1);
            int y = Random.Range(0, Tile.Y - 1);

            if (Tile.TileMap[x][y] != 0)
                continue;

            Tile.TileMap[x][y] = Tile_Manager.Cover_Kind.Others;
            Tile.MY_Tile[x][y].GetComponent<SpriteRenderer>().color = Color.cyan;

            count++;
        }
        return true;
    }
    bool MakeHut()
    {

        int count = 0;
        int BreakCount = 0;
        int direction = 0;
        int Hut_Count = Random.Range(Hut_MIN, Hut_MAX);

        while (count < Hut_Count)
        {
            int check = 0;
            int x = Random.Range(0, Tile.X - 1);
            int y = Random.Range(0, Tile.Y - 1);
            direction = 0;
            BreakCount++;

            if (Tile.TileMap[x][y] != 0)
                continue;

            #region Check Side Direction
            //------ left ---------//
            for (int i = x - 4; i <= x; i++)
            {
                if(i < 0 || i >= Tile.X || check == 1)
                {
                    check = 1;
                    break;
                }
                for(int j = y - 6;j<=y;j++)
                {
                    if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                    {
                        check = 1;
                        break;
                    }
                }
            }
            if (check == 1)
            {
                check = 0;
                for (int i = x - 4; i <= x; i++)
                {
                    
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y + 6; j >= y; j--)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 1;
            }

            //------- right -----------//
            if (check == 1)
            {
                check = 0;
                for (int i = x + 4; i >= x; i--)
                {
                    
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y - 6; j <= y; j++)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 2;
            }

            if (check == 1)
            {
                check = 0;
                for (int i = x + 4; i >= x; i--)
                {
                    
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y + 6; j >= y; j--)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 3;
            }
            #endregion

            #region Check UpDown Direction
            //------ left ---------//
            if (check == 1)
            {
                check = 0;
                for (int i = x - 6; i <= x; i++)
                {
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y - 4; j <= y; j++)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 4;
            }
            if (check == 1)
            {
                check = 0;
                for (int i = x - 6; i <= x; i++)
                {
                    
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y + 4; j >= y; j--)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 5;
            }

            //------- right -----------//
            if (check == 1)
            {
                check = 0;
                for (int i = x + 6; i >= x; i--)
                {
                    
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y - 4; j <= y; j++)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 6;
            }

            if (check == 1)
            {
                check = 0;
                for (int i = x + 6; i >= x; i--)
                {
                    
                    if (i < 0 || i >= Tile.X || check == 1)
                    {
                        check = 1;
                        break;
                    }
                    for (int j = y + 4; j >= y; j--)
                    {
                        if (j >= Tile.Y || j < 0 || Tile.TileMap[i][j] != 0)
                        {
                            check = 1;
                            break;
                        }
                    }
                }
                direction = 7;
            }
            #endregion 

            if (BreakCount >= 20) return false;
            if (check == 1) continue;
            BreakCount=0;
            Tile.TileMap[x][y] = Tile_Manager.Cover_Kind.Others;
            Tile.MY_Tile[x][y].GetComponent<SpriteRenderer>().color = Color.magenta;

            count++;
        }
        return true;
    }
}

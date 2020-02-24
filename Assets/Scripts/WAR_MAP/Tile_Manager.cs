using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Manager : MonoBehaviour {

    #region VARIABLE

    public static Tile_Manager m_Tile_Manager;
    public enum Cover_Kind { Default=0, HighCover,HalfCover,Debris,Others,CanNot,Skimisher,Slit,Standing};
    public List<List<Cover_Kind>> TileMap;

    public Cover_Kind Cover;

    [SerializeField]
    public List<List<Tile>> MY_Tile;
    public GameObject Tile;
    

    public Vector2[] SpawnPoint;
    public int X;
    public int Y;

    int MAX;
    #endregion

    // Use this for initialization
    void Start () {
        SpawnPoint = new Vector2[2];
        MAX = 40;
        CreateTileMap();
        Board_Manager.m_Board_Manager.Loading++;
        m_Tile_Manager = this;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateTileMap()
    {
        TileMap = new List<List<Cover_Kind>>();
        MY_Tile = new List<List<Tile>>();

        X = Random.Range(MAX - 20, MAX + 20);
        Y = MAX + (MAX - X);

        for (int i = 0; i < X; i++)
        {
            List<Cover_Kind> TileMap_y = new List<Cover_Kind>();
            List<Tile> MY_Tile_y = new List<Tile>();
            for (int y = 0; y < Y; y++)
            {
                TileMap_y.Add(Cover_Kind.Default);
                MY_Tile_y.Add(Instantiate(Tile, transform.position + new Vector3(i, y, 0), transform.rotation, transform).GetComponent<Tile>());
                MY_Tile_y[y].name = (i + "," + y);
                MY_Tile_y[y].Init(i, y);
            }
            TileMap.Add(TileMap_y);
            MY_Tile.Add(MY_Tile_y);
        }


        transform.Rotate(new Vector3(-45, 0, -45));
        
    }
}

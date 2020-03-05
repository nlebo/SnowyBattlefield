using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPocket : MonoBehaviour
{

	#region VARIABLE
	enum directions { North = 0, South, East, West };
	Unit_Manager.Class[] _Class = { Unit_Manager.Class.Infantry, Unit_Manager.Class.Gunner, Unit_Manager.Class.Second_Gunner, Unit_Manager.Class.Ammuniation_Soldier};

	directions _Directions;
	Tile_Manager _Tile;

	public int x, y, ex, ey;
	int radius = 6;

	public static int MaxEnemyCount = 5;
	public static int MaxPlayerCount = 5;

	public GameObject Player;
	public GameObject Enemy;

	#endregion
	// Use this for initialization
	void Start()
	{
		_Tile = GetComponent<Tile_Manager>();

		switch (Random.Range(0, 3))
		{
			case 0:
				if (_Tile.Y > _Tile.X)
					_Directions = directions.East;
				else
					_Directions = directions.North;
				break;
			case 1:
				if (_Tile.Y > _Tile.X)
					_Directions = directions.West;
				else
					_Directions = directions.South;
				break;
			case 2:
				if (_Tile.X > _Tile.Y)
					_Directions = directions.North;
				else
					_Directions = directions.East;
				break;
			case 3:
				if (_Tile.X > _Tile.Y)
					_Directions = directions.South;
				else
					_Directions = directions.West;
				break;
		}


		SpawnSpot();
		Board_Manager.m_Board_Manager.Loading++;

	}

	public void SpawnSpot()
	{
		switch (_Directions)
		{
			case directions.North:
				x = _Tile.X - radius;
				y = Random.Range(radius - 1, _Tile.Y - radius + 1);
				break;
			case directions.South:
				x = radius - 1;
				y = Random.Range(radius - 1, _Tile.Y - radius + 1);
				break;
			case directions.East:
				x = Random.Range(radius - 1, _Tile.X - radius + 1);
				y = radius - 1;
				break;
			case directions.West:
				x = Random.Range(radius - 1, _Tile.X - radius + 1);
				y = _Tile.Y - radius;
				break;
		}
		_Tile.MY_Tile[x][y].name = _Tile.MY_Tile[x][y].name + " Spawn";
		_Tile.MY_Tile[x][y].GetComponent<SpriteRenderer>().color = Color.blue;
		Camera.main.transform.position = new Vector3(_Tile.MY_Tile[x][y].transform.position.x, _Tile.MY_Tile[x][y].transform.position.y, -10);
		_Tile.SpawnPoint[0].x = x;
		_Tile.SpawnPoint[0].y = y;

		ex = _Tile.X - x - 1;
		ey = _Tile.Y - y - 1;
		_Tile.MY_Tile[ex][ey].name = _Tile.MY_Tile[ex][ey].name + " ESpawn";
		_Tile.MY_Tile[ex][ey].GetComponent<SpriteRenderer>().color = Color.red;
		_Tile.SpawnPoint[1].x = ex;
		_Tile.SpawnPoint[1].y = ey;
	}
	public void SpawnPlayer()
	{
		int count = 0;
		while (count < MaxEnemyCount)
		{
			int _x = Random.Range(ex - radius - 1, ex + radius - 1);
			int _y = Random.Range(ey - radius - 1, ey + radius - 1);

			if (_x < 0) _x = 0;
			else if (_x >= _Tile.X) _x = _Tile.X - 1;

			if (_y < 0) _y = 0;
			else if (_y >= _Tile.Y) _y = _Tile.Y - 1;

			if (_Tile.TileMap[_x][_y] == Tile_Manager.Cover_Kind.Default || _Tile.TileMap[_x][_y] == Tile_Manager.Cover_Kind.CanNot)
			{
				if (_Tile.MY_Tile[_x][_y].transform.childCount > 0) continue;
				GameObject P;
				P = Instantiate(Enemy, _Tile.MY_Tile[_x][_y].transform);
				P.GetComponent<Unit_Manager>().Char_Num = count + MaxPlayerCount;
				P.GetComponent<Unit_Manager>()._Class = _Class[count % 4];
				Tactical_Head.AddEnemy(P.GetComponent<Enemy_Manager>());
				count++;
			}
		}

		count = 0;

		while (count < MaxPlayerCount)
		{
			int _x = Random.Range(x - radius - 1, x + radius - 1);
			int _y = Random.Range(y - radius - 1, y + radius - 1);

			if (_x < 0) _x = 0;
			else if (_x >= _Tile.X) _x = _Tile.X - 1;

			if (_y < 0) _y = 0;
			else if (_y >= _Tile.Y) _y = _Tile.Y - 1;

			if (_Tile.TileMap[_x][_y] == Tile_Manager.Cover_Kind.Default || _Tile.TileMap[_x][_y] == Tile_Manager.Cover_Kind.CanNot)
			{
				if (_Tile.MY_Tile[_x][_y].transform.childCount > 0) continue;

				Unit_Manager P;
				P = Instantiate(Player, _Tile.MY_Tile[_x][_y].transform).GetComponent<Unit_Manager>();
				P.Char_Num = count;
				P._Class = _Class[count % 4];
				Tactical_Head.AddPlayer(P);
				if (_Directions == directions.North) P.dir = Unit_Manager.Direction.left;
				else if (_Directions == directions.South) P.dir = Unit_Manager.Direction.right;
				else if (_Directions == directions.East) P.dir = Unit_Manager.Direction.up;
				else P.dir = Unit_Manager.Direction.down;
				count++;
			}
		}
	}
}

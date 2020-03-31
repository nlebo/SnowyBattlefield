using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heavy_MachinGun_Manager : Weapon_Manager {

	public bool Action = false;
	public bool Btn1 = false;
	public bool Btn2 = false;
	public bool Btn3 = false;
	public bool Btn4 = false;

	protected int MaxReload = 5;
	Input_Manager _Input;

	[SerializeField]private LayerMask layerMask;

    //public Vector3 Evolve;
    private Mesh mesh;

	private float ShotRange;
    private Vector3 origin;
    private float fov;
	private float AddAngle;
	private bool _MakeAngle = false;

	private List<Unit_Manager> CheckPerson;

	// Use this for initialization
	protected void Start()
	{
		Btn_Name[0] = "SET UP [4]";
		Btn_Name[1] = "CHANGE POS [5]";
		Btn_Name[2] = "SUPPRESION FIRE [4]";
		Btn_Name[3] = "FIRE [3]";
		Btn_Name[4] = "RELOAD [4]";

		_Kind = Kind.HEAVY_MACHINE_GUN;
		_Title = Title.Main;

		_Input = Camera.main.GetComponent<Input_Manager>();
		_UI = GameObject.Find("Canvas").GetComponent<UI_MANAGER>();
		CheckPerson = new List<Unit_Manager>();
		
		
	}

	// Update is called once per frame
	void Update() {
		if (!transform.parent.CompareTag("Player")) return;

		if(Btn1 || Btn2)
		{
			MakeAngle();
		}
		if (Btn3 || Btn4)
		{
			Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[0]);
			Vector2 pos = _Input.pos;
			RaycastHit2D hit = _Input.hit_Player;

            if (hit.transform != null && hit.transform.tag == "Enemy")
            {
                CheckShot();
                int i = 0;
                for (i = 0; i < CheckPerson.Count; i++)
                {
                    if (CheckPerson[i].transform == hit.transform)
                        break;
                }

                if (i != CheckPerson.Count)
                {
                    Unit_Manager _Unit = hit.transform.GetComponent<Unit_Manager>();

                    if (!_UI.HitRate.gameObject.activeInHierarchy)
                    {
                        _UI.HitRate.gameObject.SetActive(true);
                        _UI.HitRateText.text = _Unit.GetHitRate(this).ToString() + "%";
                    }
                }
            }
            else
            {
                if (_UI.HitRate.gameObject.activeInHierarchy) _UI.HitRate.gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (Btn3 || Btn4)
            {
                Vector2 pos = _Input.pos;
                RaycastHit2D hit = _Input.hit_Player;

                if (hit.transform != null && hit.transform.tag == "Enemy")
                {
                    Unit_Manager _Unit = hit.transform.GetComponent<Unit_Manager>();
                    Debug.Log(_Unit);
                    if (Btn3)
                    {
                        StartCoroutine(_BTN3(_Unit));
                        Unit.Now_Action_Point -= 4;
                    }
                    else
                    {
                        StartCoroutine(_BTN4(_Unit, Bullet));
                        Unit.Now_Action_Point -= 3;
                    }

                    Unit.DrawActionPoint();
                    Cursor_Manager.m_Cursor_Manager.SetCursor(_UI.Cursors[1]);
                    if (_UI.HitRate.gameObject.activeInHierarchy) _UI.HitRate.gameObject.SetActive(false);

                }
            }
            if (Btn1 || Btn2)
            {
				Unit.Now_Action_Point = Btn1 ? Unit.Now_Action_Point - 4 : Unit.Now_Action_Point;
				


                Btn1 = false;
				Btn2 = false;
                _MakeAngle = true;
				Action=false;
            }
        }
	}


	public override void Select()
	{
		base.Select();
		for (int i = 0; i < _UI.Weapon_Button.Count; i++)
		{
			if (Btn_Name[i] == null) continue;
			_UI.Weapon_Button[i].gameObject.SetActive(true);
			_UI.Weapon_Button[i].transform.GetChild(0).GetComponent<Text>().text = Btn_Name[i];

		}

		_UI.Weapon_Button[0].onClick.AddListener(BTN1);
		_UI.Weapon_Button[1].onClick.AddListener(BTN2);
		_UI.Weapon_Button[2].onClick.AddListener(BTN3);
		_UI.Weapon_Button[3].onClick.AddListener(BTN4);
		_UI.Weapon_Button[4].onClick.AddListener(BTN5);
	}

    public override void BTN1()
    {
        if (Unit.Now_Action_Point < 4 || Action || _MakeAngle) return;
		
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 40f;
		ShotRange = 20;
		Action = true;
		Btn1 = true;

	}
	public override void BTN2()
	{
		if (Unit.Now_Action_Point < 5 || Action || !_MakeAngle) return;

		Unit.Now_Action_Point -=5;
		_MakeAngle = false;
		Action = true;
		Btn2 = true;

	}
	public override void BTN3()
	{
		if (Unit.Now_Action_Point < 4 || Action || Bullet < 50) return;

		Action = true;
		Btn3 = true;
		Aim_Bonus = -40;

	}
	public override void BTN4()
	{
		if (Unit.Now_Action_Point < 3 || Action || Bullet <= 0) return;

		Action = true;
		Btn4 = true;
		Aim_Bonus = -20;
	}
	public override void BTN5()
	{
		if (Unit.Now_Action_Point < 4 || Action || Bullet == MaxBullet || MaxReload <= 0) return;
		Tile_Manager _Tile = GameObject.Find("MapTiles").GetComponent<Tile_Manager>();

		#region Unit_Ammuniation_Check
		if (_Tile.MY_Tile[Unit.x - 1][Unit.y].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.x - 1][Unit.y].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}
			
		}
		if (_Tile.MY_Tile[Unit.x + 1][Unit.y].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.x + 1][Unit.y].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		if (_Tile.MY_Tile[Unit.x][Unit.y - 1].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.x][Unit.y - 1].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		if (_Tile.MY_Tile[Unit.x][Unit.y + 1].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.x][Unit.y + 1].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		#endregion
		#region Partner_Ammuniation_Check
		if (_Tile.MY_Tile[Unit.Partner.x - 1][Unit.Partner.y].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.Partner.x - 1][Unit.Partner.y].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Unit.Partner.Now_Action_Point = Unit.Now_Action_Point;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		if (_Tile.MY_Tile[Unit.Partner.x + 1][Unit.Partner.y].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.Partner.x + 1][Unit.Partner.y].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Unit.Partner.Now_Action_Point = Unit.Now_Action_Point;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		if (_Tile.MY_Tile[Unit.Partner.x][Unit.Partner.y - 1].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.Partner.x][Unit.Partner.y - 1].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Unit.Partner.Now_Action_Point = Unit.Now_Action_Point;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		if (_Tile.MY_Tile[Unit.Partner.x][Unit.Partner.y + 1].transform.childCount > 0)
		{
			Transform Ammuniation_Soldier = _Tile.MY_Tile[Unit.Partner.x][Unit.Partner.y + 1].transform.Find("Player(Clone)");
			if (Ammuniation_Soldier != null && Ammuniation_Soldier.GetComponent<Unit_Manager>()._Class == Unit_Manager.Class.Ammuniation_Soldier)
			{
				Unit.Now_Action_Point -= 4;
				Unit.Partner.Now_Action_Point = Unit.Now_Action_Point;
				Bullet = MaxBullet;
				Unit.DrawActionPoint();
				_UI._Clip.text = Bullet.ToString();
				return;
			}

		}
		#endregion

		


	}

	public override void Reload()
	{
		base.Reload();
		BTN5();
	}
	public override void Unselect()
	{
		base.Unselect();
		Btn1 = false;
		Btn2 = false;
		Action = false;
	}


    void MakeAngle()
    {
        transform.position = Vector3.zero;
        origin = Unit.transform.position;
        int rayCount = 50;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        // float angle3dIncrease = 2.8f / rayCount;
        // float angle3d = -1.4f;
        float viewDistance = ShotRange;
        Vector3[] Vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[Vertices.Length];
        int[] triangles = new int[rayCount * 3];

		Vector2 To = Input_Manager.m_InputManager.pos;
		Vector2 From = Unit.transform.position;
	
	
        //AddAngle = Quaternion.FromToRotation(Vector3.up, To - From).eulerAngles.z;
		Vector3 v = To - From;
    	AddAngle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		AddAngle += fov / 2;
        Vertices[0] = origin;

        int VertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

			vertex = origin + GetVectorFromAngle(angle + AddAngle) * viewDistance;
            Vertices[VertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = VertexIndex - 1;
                triangles[triangleIndex + 2] = VertexIndex;

                triangleIndex += 3;
            }

            VertexIndex++;
            angle -= angleIncrease;
            
        }

        mesh.vertices = Vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
	}
	
	void CheckShot()
	{
		int rayCount = 50;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        float viewDistance = ShotRange;
		CheckPerson.Clear();
		for(int i =0; i<= rayCount ; i++)
		{
			RaycastHit[] Hits;
			Hits = Physics.RaycastAll(origin,GetVectorFromAngle(angle + AddAngle),viewDistance,layerMask);

			for(int j =0; j < Hits.Length;j++)
			{
				if(Hits[j].collider.tag == "Enemy")
				{
					CheckPerson.Add(Hits[j].collider.GetComponent<Unit_Manager>());
				}
			}
		}
	}
	IEnumerator _BTN3(Unit_Manager _Unit)
	{
		for (int i = 0; i < 50; i++)
		{
			if (_Unit != null)
			{
				_Unit.Hit(this);
				Bullet--;
				_UI._Clip.text = Bullet.ToString();
			}
			yield return new WaitForSeconds(0.1f);
		}
		Aim_Bonus = 0;
		Action = false;
		Btn3 = false;
	}
	IEnumerator _BTN4(Unit_Manager _Unit, int _Bullet)
	{
		for (int i = 0; i < _Bullet; i++)
		{
			if (i >= 20) break;

			if (_Unit != null)
			{
				_Unit.Hit(this);
				Bullet--;
				_UI._Clip.text = Bullet.ToString();
			}
			yield return new WaitForSeconds(0.1f);
		}
		Aim_Bonus = 0;
		Action = false;
		Btn4 = false;
		yield return null;
	}

	Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad),Mathf.Sin(angleRad));
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
}

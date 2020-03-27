using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Input_Manager : MonoBehaviour {

    public static Input_Manager m_InputManager;
    public Vector2 pos;
    public RaycastHit2D hit,hit_Player,hit_Tile;
    public Player_Manager Selected = null;

    public UnityAction EndTurn;
    public UnityAction Tile_View;

    public static UnityAction Highlighted;

	public UnityAction EnemyView;

    public ItemUI_Manager DragItem;

    public float y = 0;

    int layerMask = 1 << 8;
    int layerMask_Tile = 1 << 8;

    // Use this for initialization
    void Start () {
        m_InputManager = this;
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        Plane plane = new Plane(Vector3.forward, transform.position + (Quaternion.AngleAxis(45,Vector3.right) * new Vector3(0,y,0)));
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter = 0;
        if(plane.Raycast(ray, out enter))
        {
            pos = new Vector2(ray.GetPoint(enter).x,ray.GetPoint(enter).y);
        }
        hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
        hit_Player = Physics2D.Raycast(pos, Vector2.zero, 0f, layerMask);
        hit_Tile = Physics2D.Raycast(pos, Vector2.zero, 0f, layerMask_Tile);

        if (Time.frameCount % 30 == 0)
        {
            System.GC.Collect();
        }
    }

    public void OnClick_EndTurn()
    {
        EndTurn();
        Board_Manager.m_Board_Manager.EndTurn();
    }
}

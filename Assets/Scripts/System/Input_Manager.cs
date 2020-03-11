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

        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

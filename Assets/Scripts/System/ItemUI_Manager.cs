using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI_Manager : ButtonAction
{
    // Start is called before the first frame update
    public int x =0 ,y=0;
    public int sx=1,sy=1;
    public Item_Manager _This;
    public bool Up=false;

    public override void InvenDown()
    {
        base.InvenDown();
        
        Up = false;
    }

    public override void InvenUp()
    {
        base.InvenUp();
        
        Up=true;
    }

    private void OnTriggerStay2D(Collider2D other) {


        if(other.gameObject.tag == "grid" && Up)
        {
            Debug.Log(other);
            GridsInfo _grid = other.GetComponent<GridsInfo>();

            _grid.Inven.Insert_Item(_This,_grid.x,_grid.y,sx,sy);
            Input_Manager.m_InputManager.DragItem = null;
            Up=false;
            // Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.gameObject.tag == "grid" && Up)
        {
            Debug.Log(other);
            GridsInfo _grid = other.GetComponent<GridsInfo>();

            _grid.Inven.Insert_Item(_This,_grid.x,_grid.y,sx,sy);
            Input_Manager.m_InputManager.DragItem = null;
            Up=false;
            // Destroy(gameObject);
        }
    }
    
}

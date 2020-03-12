using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI_Manager : ButtonAction
{
    // Start is called before the first frame update
    public int x =0 ,y=0;
    public int sx=1,sy=1;
    public Item_Manager _This;
    public bool Up=false;

    protected override void Start()
    {
        base.Start();
        _This = new Item_Manager();

        _This.UI_Image = new List<Sprite>();
        _This.UI_Image.Add(GetComponent<Image>().sprite);
        if(transform.childCount > 0)
        for(int i =0;i<transform.childCount;i++)
        {
            _This.UI_Image.Add(transform.GetChild(i).GetComponent<Image>().sprite);
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.R) && Down)
        {
            _This._Rot = !_This._Rot;
            
            transform.rotation = transform.rotation == Quaternion.Euler(0,0,0) ? Quaternion.Euler(0,0,-90) : Quaternion.Euler(0,0,0);

            int st = sx;
            sx = sy;
            sy = st;
        }
    }
    public override void InvenDown()
    {
        base.InvenDown();
        
        Up = false;
        Down = true;
    }

    public override void InvenDrag()
    {
        base.InvenDrag();
    }

    public override void InvenUp()
    {
        base.InvenUp();
        
        Up=true;
        Down = false;
    }

    private void OnTriggerStay2D(Collider2D other) {


        if(other.gameObject.tag == "grid" && Up)
        {
            Debug.Log(other);
            GridsInfo _grid = other.GetComponent<GridsInfo>();

            _grid.Inven.Insert_Item(_This,_grid.num,sx,sy);
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

            _grid.Inven.Insert_Item(_This,_grid.num,sx,sy);
            Input_Manager.m_InputManager.DragItem = null;
            Up=false;
            // Destroy(gameObject);
        }
    }
    
}

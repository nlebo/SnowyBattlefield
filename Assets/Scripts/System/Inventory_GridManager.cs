using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_GridManager : MonoBehaviour
{
    public int GX, GY;
    public int CX, CY;
    public int[,] Grids;
    int ItemNum = 1;

    [SerializeField]
    List<ItemUI_Manager> Items;

    private void Start()
    {
        Grids = new int[GX, GY];
        Grids.Initialize();
        Items = new List<ItemUI_Manager>();
    }

    public bool Insert_Item(ItemUI_Manager Item, int num, int sx,int sy)
    {
        int x = num == 0 ? 0 : num % GX;
        int y = num == 0 ? 0 : num / GX;

        return Insert_Item(Item,x,y,sx,sy);
    }
    public bool Insert_Item(ItemUI_Manager Item, int x, int y, int sx, int sy)
    {
        if (!Check_CanInsert(x, y, sx, sy)) return false;

        int count = 0;
        for (int _x = x; _x < x + sx; _x++)
        {
            for (int _y = y; _y < y + sy; _y++)
            {
                Grids[_x, _y] = ItemNum;

                if (count == 0)
                {
                    Item.transform.SetParent(transform.GetChild(_x + (_y * GX)));
                    Item.transform.localPosition = Vector3.zero;
                }
                else
                {
                    Transform child = Item.transform.GetChild(count - 1);
                    child.SetParent(transform.GetChild(_x + (_y * GX)));
                    child.localPosition = Vector3.zero;
                    child.rotation = Item.transform.rotation;
                }

                //transform.GetChild(_x + (_y * GX)).GetComponent<Image>().sprite = Item._This.UI_Image[count];
                count++;
                //transform.GetChild(_x + (_y * GX)).rotation = Item._This._Rot ?  Quaternion.Euler(0,0,-90) : Quaternion.Euler(0,0,0);
            }
        }
         
        

        Items.Add(Item);
        ItemNum++;
        return true;
    }

    public bool Insert_Item(ItemUI_Manager Item, int sx, int sy)
    {
        int _x = 0, _y = 0;
        for (_x = 0; _x < GX; _x++)
        {
            for (_y = 0; _y < GY; _y++)
            {
                if (Check_CanInsert(_x, _y, sx, sy)) break;
            }
        }

        if (_x == GX && _y == GY) return false;

        return Insert_Item(Item,_x,_y,sx,sy);
    }

    public bool Check_CanInsert(int x, int y, int sx, int sy)
    {
        if (x + sx > GX || y + sy > GY) return false;

        for (int _x = x; _x < x + sx; _x++)
        {
            for (int _y = y; _y < y + sy; _y++)
            {
                if (Grids[_x, _y] != 0) return false;
            }
        }

        return true;
    }

    public bool Remove_Item(int _x, int _y)
    {
        int Item = Grids[_x, _y];

        if (Item == 0) return false;

        Items.RemoveAt(Item - 1);
        ItemNum--;

        int count = 0;
        Transform _T;
        _T = null;
        for (int x = 0; x < GX; x++)
        {
            for (int y = 0; y < GY; y++)
            {
                if (Grids[x, y] == Item)
                {
                    if (count == 0)
                    {
                         _T = transform.GetChild(x + (y * GX)).GetChild(0);
                         _T.GetComponent<Image>().raycastTarget = true;
                         _T.SetParent(transform.parent);
                    }
                    else
                    {
                        transform.GetChild(x + (y * GX)).GetChild(0).SetParent(_T);
                    }
                    count++;
                }
                Grids[x, y] = Grids[x, y] == Item ? 0 : Grids[x, y] > Item ? Grids[x, y] - 1 : Grids[x, y];
            }
        }
        ItemUI_Manager I = _T.GetComponent<ItemUI_Manager>();
        I.Down = true;
        I.Up = false;
        I.MousePos = I.UICamera.ScreenToWorldPoint(Input.mousePosition);
        I.BasePos = I._Transform.position;


        return true;
    }

    public void Remove_Item(int num)
    {
         int x = num == 0 ? 0 : num % GX;
        int y = num == 0 ? 0 : num / GX;

        Remove_Item(x,y);
    }

    public bool Remove_Item(ItemUI_Manager _Item)
    {
        int Item;
        if (!Items.Contains(_Item)) return false;

        Item = Items.FindIndex(Items => Items == _Item);

        
        for (int x = 0; x < GX; x++)
        {
            for (int y = 0; y < GY; y++)
            {
                
                Grids[x, y] = Grids[x, y] == Item ? 0 : Grids[x, y] > Item ? Grids[x, y] - 1 : Grids[x, y];
            }
        }

        return true;
    }

}

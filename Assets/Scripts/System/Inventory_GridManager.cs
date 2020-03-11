using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory_GridManager : MonoBehaviour
{
    public int GX, GY;
    public int CX, CY;
    public int[,] Grids;
    int ItemNum = 0;
    List<Item_Manager> Items;

    private void Start()
    {
        Grids = new int[GX, GY];
        Grids.Initialize();
        Items = new List<Item_Manager>();
    }

    public bool Insert_Item(Item_Manager Item, int x, int y, int sx, int sy)
    {
        if (!Check_CanInsert(x, y, sx, sy)) return false;

        for (int _x = x; _x < x + sx; _x++)
        {
            for (int _y = y; _y < y + sy; _y++)
            {
                Grids[_x, _y] = ItemNum;
            }
        }

        Items.Add(Item);
        ItemNum++;
        return true;
    }

    public bool Insert_Item(Item_Manager Item, int sx, int sy)
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

        int SX = _x + sx;
        int SY = _y + sy;
        for (; _x < SX; _x++)
        {
            for (; _y < SY; _y++)
            {
                Grids[_x, _y] = ItemNum;
            }
        }

        Items.Add(Item);
        ItemNum++;
        return true;
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
        Items.RemoveAt(Item);

        for (int x = 0; x < GX; x++)
        {
            for (int y = 0; y < GY; y++)
            {
                Grids[x, y] = Grids[x, y] == Item ? 0 : Grids[x, y] > Item ? Grids[x, y] - 1 : Grids[x, y];
            }
        }

        return true;
    }
    public bool Remove_Item(Item_Manager _Item)
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

    public void SaveX(string x)
    {
        CX = int.Parse(x);
    
    }
    public void SaveY(string y)
    {
        CY = int.Parse(y);
    
    }
    public void ItemDown()
    {
        ItemUI_Manager item = Input_Manager.m_InputManager.DragItem;
        
        Insert_Item(item._This,CX,CY,item.sx,item.sy);
    }

}

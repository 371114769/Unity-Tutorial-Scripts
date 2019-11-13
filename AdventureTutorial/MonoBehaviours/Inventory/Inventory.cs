using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public const int numItemSlots = 4;

    public Image[] itemImages = new Image[numItemSlots];
    //注意这里的Type是Item，也就是那个Script
    public Item[] items = new Item[numItemSlots];

    public void AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = itemToAdd;
                //image type和sprite type不一样，所以这里才会有itemImages[i].sprite
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return;
            }
        }
    }

    public void RemoveItem(Item itemToremove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToremove)
            {
                items[i] = null;
                //image type和sprite type不一样，所以这里才会有itemImages[i].sprite
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                return;
            }
        }
    }
}

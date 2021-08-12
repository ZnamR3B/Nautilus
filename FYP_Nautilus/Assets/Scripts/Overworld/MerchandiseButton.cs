using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MerchandiseButton : MonoBehaviour
{
    public Shop shopScript;

    public int index;
    public int price;
    public int count;

    public TextMeshProUGUI quantityText;

    public void initButton(int i, int p, Shop script)
    {
        shopScript = script;
        price = p;
        index = i;
        count = 0;
    }
    public void addQuantity()
    {
        if(count < 99)
        {
            count++;
            shopScript.updateTotalPrice(price);
            shopScript.quantity[index]++;
        }
        else
        {
            count = 0;
            shopScript.updateTotalPrice(-price * 99);
            shopScript.quantity[index] = 0;
        }
        quantityText.text = count.ToString();
    }

    public void reduceQuantity()
    {
        if(count > 0)
        {
            count--;
            shopScript.updateTotalPrice(-price);
            shopScript.quantity[index]--;
        }
        else
        {
            shopScript.updateTotalPrice(price * 99);
            shopScript.quantity[index] = 99;
            count = 99;
        }
        quantityText.text = count.ToString();
    }

}

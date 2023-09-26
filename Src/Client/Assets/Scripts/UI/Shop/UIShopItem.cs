using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour,ISelectHandler
{

    public Image icon;
    public Text title;
    public Text price;
    public Text limitClass;
    public Text count;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    private bool selected;
    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectedBg : normalBg;
        }
    }
    public int ShopItemID { get; set; }
    private UIShop shop;

    private ItemDefine item;
    private ShopItemDefine ShopItem { get; set; }

    // Use this for initialization
    void Start () {
		
	}

    public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
    {

        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;

        if (DataManager.Instance.Items.ContainsKey(this.ShopItem.ItemID))
        {
            this.item = DataManager.Instance.Items[this.ShopItem.ItemID];
        }
        else
        {
            Debug.LogError("Item ID not found in the dictionary: " + this.ShopItem.ItemID);
            return;
        }

        this.title.text = this.item.Name;
        Debug.Log("ShopItem.Count: " + ShopItem.Count); // Debug log to verify the value
        this.count.text = ShopItem.Count.ToString();
        this.price.text = ShopItem.Price.ToString();
        this.limitClass.text = this.item.LimitClass.ToString();

        this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);
        if (this.icon.overrideSprite == null)
        {
            // 解析图集路径和精灵名字
            string[] parts = item.Icon.Split('/');
            if (parts.Length == 4)
            {
                string atlasPath = parts[0] + "/" + parts[1];
                string spriteName = parts[3];
                this.icon.overrideSprite = Resloader.LoadSpriteFromAtlas(atlasPath, spriteName);
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        this.shop.SelectShopItem(this);  
    }
}

using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIWindow
{
    public Text title;
    public Text money;

    public GameObject shopItem;
    public GameObject shopEquipItem;
    public GameObject shopBag;
    ShopDefine shop;
    public Transform[] itemRoot;

    void Start() {
        StartCoroutine(InitItems());
    }

    IEnumerator InitItems()
    {
        int count = 0;
        int page = 0;
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status <= 0) continue;

            GameObject prefabToInstantiate = (shop.ID == 1) ? shopItem : shopEquipItem;

            GameObject go = Instantiate(prefabToInstantiate, itemRoot[page]);
            UIShopItem ui = go.GetComponent<UIShopItem>();
            ui.SetShopItem(kv.Key, kv.Value, this);
            count++;
            if (count >= 10)
            {
                count = 0;
                page++;
                itemRoot[page].gameObject.SetActive(true);
            }
        }
        yield return null;
    }

    public void Setshop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    private UIShopItem selectedItem;

    public void SelectShopItem(UIShopItem item)
    {
        if (selectedItem != null)
            selectedItem.Selected = false;
        selectedItem = item;
    }

    public void OnClickBuy()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要购买的道具", "购买提示");
            return;
        }
        if (!ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID))
        {
            MessageBox.Show("购买失败", "购买提示");
            return;
        }
        if (ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID))
        {
            Debug.Log("购买成功!");
        }
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
        // 更新背包UI
        UIBag uiBag = UIManager.Instance.GetWindow<UIBag>();
        if (uiBag != null)
        {
            uiBag.OnReset();
        }
    }
}

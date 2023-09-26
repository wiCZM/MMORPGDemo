using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UICharEquip : UIWindow
{
    public Text title;
    public Text money;

    public GameObject itemPrefab;
    public GameObject itemEquipedPrefab;

    public Transform itemListRoot;

    public List<Transform> slots;


    void Start()
    {
        RefreshUI();
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }

    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipList();
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacter.Gold.ToString();
    }


    /// <summary>
    /// 初始化所有装备列表
    /// </summary>
    /// <returns></returns>


    private void InitAllEquipItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == Item_Type.Equip)
            {
                if (EquipManager.Instance.Contains(kv.Key))
                    continue;
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                if (go == null)
                {
                    Debug.LogError("Failed to instantiate prefab");
                    continue;
                }
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                if (ui == null)
                {
                    Debug.LogError("UIEquipItem component not found on the instantiated object");
                    continue;
                }
                ui.SetEquipItem(kv.Value.Define.Icon, kv.Key, kv.Value, this, false);
            }
        }
    }

    void ClearAllEquipList()
    {
        foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }

    void ClearEquipList()
    {
        foreach (var item in slots)
        {
            if (item.childCount > 0)
                Destroy(item.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// 初始化已经装备的列表
    /// </summary>
    /// <returns></returns>

    void InitEquipedItems()
    {
        for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            var item = EquipManager.Instance.Equips[i];
            {
                if (item != null)
                {
                    GameObject go = Instantiate(itemEquipedPrefab, slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem("",i, item, this, true);
                }
            }

        }
    }

    public void DeEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }
}


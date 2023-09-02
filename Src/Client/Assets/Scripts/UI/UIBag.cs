using Assets.Scripts.Models;
using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : MonoBehaviour {

    public Text money;

    public Transform[] pags;

    public GameObject bagItem;

    List<Image> slots;

    //public Item bagItem;
    // Use this for initialization
    void Start() {
        if (slots == null)
        {
            for (int page = 0; page < this.pags.Length; page++)
            {
                slots.AddRange(this.pags[page].GetComponentsInChildren<Image>(true));
            }
        }
        StartCoroutine(IntBags());
    }

    IEnumerator IntBags()
    {
        for (int i = 0; i < BagManager.Instance.Items.Length; i++)
        {
            var item = BagManager.Instance.Items[i];
            if (item.ItemId > 0)
            {
                GameObject go = Instantiate(bagItem, slots[i].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].Define;
                ui.SetMainIcon(def.Icon, item.Count.ToString());
            }
        }
        for (int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
        {
            slots[i].color = Color.gray;
        }
        yield return null;
    }

    public void SetTitle(string title)
    {
        this.money.text = User.Instance.CurrentCharacter.Id.ToString();
    }

    public void OnReset()
    {
        BagManager.Instance.Reset();
    }

}

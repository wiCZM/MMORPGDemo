using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour,IPointerClickHandler
{
    public Image icon;
    public Text title;
    public Text level;
    public Text limitClass;
    public Text limitCategory;

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

    public int index { get; set; }
    private UICharEquip owner;
    private Item item;

    void Start()
    {

    }

    bool isEquiped = false;

    internal void SetEquipItem(string iconName,int idx, Item item, UICharEquip owner, bool equiped)
    {
        this.owner = owner;
        this.index = idx;
        this.item = item;
        this.isEquiped = equiped;
        this.icon.overrideSprite = Resloader.Load<Sprite>(iconName);
        if (this.icon.overrideSprite == null)
        {
            // 这里需要知道图集的路径和精灵的名字
            // iconName 包含的信息，用 "/" 分隔
            string[] parts = iconName.Split('/');
            if (parts.Length == 4)
            {
                string atlasPath = parts[0] + "/" + parts[1];
                string spriteName = parts[3];
                this.icon.overrideSprite = Resloader.LoadSpriteFromAtlas(atlasPath, spriteName);
            }
        }

        if (this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.limitClass != null) this.limitClass.text = item.Define.LimitClass.ToString();
        if (this.limitCategory != null) this.limitCategory.text = item.Define.Category;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
        if (this.icon.overrideSprite == null)
        {
            // 这里需要知道图集的路径和精灵的名字
            // iconName 包含的信息，用 "/" 分隔
            string[] parts = this.item.Define.Icon.Split('/');
            if (parts.Length == 4)
            {
                string atlasPath = parts[0] + "/" + parts[1];
                string spriteName = parts[3];
                this.icon.overrideSprite = Resloader.LoadSpriteFromAtlas(atlasPath, spriteName);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.selected)
            {
                DoEquip();
                this.Selected = false;
            }
            else
                this.Selected = true;
        }
    }

    public void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("需要装备[{0}]吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            var oldEquip = EquipManager.Instance.GetEquip(item.EquipInfo.Slot);
            if (oldEquip != null)
            {
                var newmsg = MessageBox.Show(string.Format("要替换掉[{0}]吗?", oldEquip.Define.Name), "确认", MessageBoxType.Confirm);
                msg.OnYes = () =>
                {
                    this.owner.DeEquip(this.item);
                };
            }
            else
                this.owner.DeEquip(this.item);
        };
    }

    public void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("要取下装备[{0}]吗?", this.item.Define.Name), "确认", MessageBoxType.Confirm);
        msg.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }
}

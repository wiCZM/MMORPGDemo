using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFriendItem : ListView.ListViewItem
{
    public Text nickname;
    public Text @class;
    public Text level;
    public Text status;

    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    public NFriendInfo Info;
    void Start()
    {

    }

    bool isEquiped = false;

    public void SetFriendInfo(NFriendInfo item)
    {
        this.Info = item;
        if (this.nickname != null) this.nickname.text = this.Info.friendInfo.Name;
        if (this.@class != null) this.@class.text = this.Info.friendInfo.Class.ToString();
        if (this.level != null) this.level.text = this.Info.friendInfo.Level.ToString();
        if (this.status != null) this.status.text = this.Info.Status == 1 ? "在线" : "离线";
    }
}

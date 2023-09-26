using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem :ListView.ListViewItem
{
    public Text nickname;
    public Image classicon;
    public Image leadericon;

    public Image background;

    public override void onSelected(bool selected)
    {
        this.background.enabled = selected ? true : false;
    }

    public int idx;
    public NCharacterInfo Info;

    void Start()
    {
        this.background.enabled = false;
    }

    public void SetMemberInfo(int idx,NCharacterInfo item,bool isLeader)
    {
        this.idx = idx;
        this.Info = item;

        if (this.nickname != null) this.nickname.text = this.Info.Level.ToString().PadRight(4) + this.Info.Name;
        if (this.classicon != null) this.classicon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1];
        if (this.leadericon != null) this.leadericon.gameObject.SetActive (isLeader);
    }
}

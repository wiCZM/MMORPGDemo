using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : ListView.ListViewItem
{
    public Text title;
    public Image background;
    public Sprite normalBg;
    public Sprite selectedBg;

    public float lastClickTime = 0f;
    private const float doubleClickDelay = 0.5f;
    private bool isSelected = false;


    public override void onSelected(bool selected)
    {
        float timeSinceLastClick = Time.time - lastClickTime;

        if (selected && timeSinceLastClick <= doubleClickDelay)
        {
            this.background.overrideSprite = normalBg;
            isSelected = false;
        }
        else
        {
            this.background.overrideSprite = selected ? selectedBg : normalBg;
            isSelected = selected;
        }

        lastClickTime = Time.time;
    }

    public Quest quest;

    void Start () {
		
	}

    bool isEquiped = false;

    public void SetQuestInfo(Quest item)
    {
        this.quest = item;
        if (this.title != null) this.title.text = this.quest.Define.Name;
    }
}

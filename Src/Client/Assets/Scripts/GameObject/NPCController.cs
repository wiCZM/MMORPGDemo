﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;
using Managers;
using System;
using Models;

public class NPCController : MonoBehaviour {
    public int npcId;

    SkinnedMeshRenderer renderer;
    Animator anim;
    Color orignColor;

    private bool inTnteractive = false;

    NpcDefine npc;

    NpcQuestStatus questStatus;

	void Start () {
        renderer = this.GetComponentInChildren<SkinnedMeshRenderer>();
        anim = this.gameObject.GetComponent<Animator>();
        orignColor = renderer.sharedMaterial.color;
        npc = NPCManager.Instance.GetNpcDefine(this.npcId);
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
	}

    void OnQuestStatusChanged(Quest arg0)
    {
        this.RefreshNpcStatus();
    }

    private void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.npcId);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inTnteractive)
                yield return new WaitForSeconds(2f);
            else
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            this.Ralax();
        }
    }

    void Update()
    {

    }

    private void Ralax()
    {
        anim.SetTrigger("Relax");
    }

    void Interactive()
    {
        if (!inTnteractive)
        {
            inTnteractive = true;
            StartCoroutine(DoTnteraction());
        }
    }

    IEnumerator DoTnteraction()
    {
        yield return FaceToPlayer();
        if (NPCManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inTnteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        Interactive();
        ShopManager.Instance.OnOpenShop(npc);
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }


    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (renderer.sharedMaterial.color != Color.white)
                renderer.sharedMaterial.color = Color.white;
        }
        else
        {
            if (renderer.sharedMaterial.color != orignColor)
                renderer.sharedMaterial.color = orignColor;
        }
    }
}

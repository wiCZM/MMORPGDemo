﻿using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoBehaviour
{

    public Collider minimapBoundingBox;
    public Image minimap;
    public Image arrow;
    public Text mapName;

    private Transform playerTransform;

    void Start()
    {
        Debug.Log("UIMinimap Start: minimapBoundingBox is " + (minimapBoundingBox == null ? "null" : "not null"));
        MinimapManager.Instance.minimap = this;
        this.UpdateMap();
    }

    public void UpdateMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;
        this.minimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;
        if (minimapBoundingBox == null)
        {
            Debug.LogWarning("UpdateMap.minimapBoundingBox is null or destroyed!");
            return;
        }
        this.playerTransform = null;
    }

    void Update()
    {
        if (playerTransform == null)
            playerTransform = MinimapManager.Instance.PlayerTransform;
        if (minimapBoundingBox == null || playerTransform == null) return;
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        if (minimapBoundingBox == null)
        {
            Debug.LogWarning("Update.minimapBoundingBox is null or destroyed!");
            return;
        }

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }
}
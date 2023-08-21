using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoBehaviour {

    public Collider minimapBoundingBox;
    public Image minimap;
    public Image arrow;
    public Text mapName;

    private Transform playerTransform;
    // Use this for initialization
    //void Start()
    //{
    //    this.InitMap();
    //}
    IEnumerator Start()
    {
        while (User.Instance.CurrentCharacterObject == null)
        {
            yield return null; // 等待下一帧
        }
        this.InitMap();
    }


    void InitMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        if (this.minimap.overrideSprite == null)
            this.minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();

        this.minimap.SetNativeSize();
        //this.minimap.transform.localPosition = Vector3.zero;
        this.playerTransform = User.Instance.CurrentCharacterObject.transform;
    }

    //// Update is called once per frame
    void Update()
    {
        //if (this.playerTransform == null)
        //{
        //    if (User.Instance.CurrentCharacterObject != null)
        //    {
        //        this.playerTransform = User.Instance.CurrentCharacterObject.transform;
        //    }
        //    else
        //    {
        //        return; // 如果 playerTransform 仍然是 null，则直接返回，跳过此次 Update。
        //    }
        //}
        if (minimapBoundingBox == null || playerTransform == null) return;
        float realWidth = minimapBoundingBox.bounds.size.x;
        float realHeight = minimapBoundingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoundingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoundingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        this.minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        this.minimap.rectTransform.localPosition = Vector2.zero;
        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }
}

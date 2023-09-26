using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour {

    public Image mainImage;
    public Image secondImage;
    public Text mainText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMainIcon(string iconName, string text)
    {
        this.mainImage.overrideSprite = Resloader.Load<Sprite>(iconName);
        if (this.mainImage.overrideSprite == null)
        {
            // 这里需要知道图集的路径和精灵的名字
            // iconName 包含的信息，用 "/" 分隔
            string[] parts = iconName.Split('/');
            if (parts.Length == 4)
            {
                string atlasPath = parts[0] + "/" + parts[1];
                string spriteName = parts[3];
                this.mainImage.overrideSprite = Resloader.LoadSpriteFromAtlas(atlasPath, spriteName);
            }
        }
        this.mainText.text = text;
    }
}

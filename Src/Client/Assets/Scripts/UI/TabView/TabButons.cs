using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButons : MonoBehaviour {


    public Sprite activeImage;
    public Sprite normalImage;

    public TabView tabView;

    public int tabIndex = 0;
    public bool selected = false;

    private Image tabImage;


	// Use this for initialization
	void Start () {
        tabImage = this.GetComponent<Image>();
        normalImage = tabImage.sprite;

        this.GetComponent<Button>().onClick.AddListener(OnClick);
	}

    public void Select(bool select)
    {
        tabImage.sprite = select ? activeImage : normalImage;
    }

    public void OnClick()
    {
        this.tabView.SelectTab(this.tabIndex);
    }
}

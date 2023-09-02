using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text title;
	// Use this for initialization
	void Start () {
		
	}
    public void SetTitle(string title)
    {
        this.title.text = title;
    }


	// Update is called once per frame
	void Update () {
		
	}
}

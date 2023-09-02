using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabView : MonoBehaviour {
    public TabBuuton[] tabButtons;
    public GameObject[] tabPasges;

    public int Index;
    IEnumerator Start () {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();
        SelectTab(0);
	}

    public void SelectTab(int index)
    {
        if (this.Index != index)
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].Select(i == index);
                tabPasges[i].SetActive(i == index);
            }
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}

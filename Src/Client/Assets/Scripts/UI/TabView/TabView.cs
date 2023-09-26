using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabView : MonoBehaviour {
    public TabButons[] tabButtons;
    public GameObject[] tabPages;

    public event Action<int> OnTabSelect;

    public int Index = -1;
    IEnumerator Start ()
    {
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
                tabPages[i].SetActive(i == index);
            }
        }
    }
}

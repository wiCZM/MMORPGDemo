using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldElement : MonoBehaviour {

    public Transform owner;

    public float height = 1.5f;

    void Start () {
		
	}

    void Update()
    {
        if (owner != null)
        {
            this.transform.position = owner.position + Vector3.up * height;
        }
        if(Camera.main != null)
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }
}

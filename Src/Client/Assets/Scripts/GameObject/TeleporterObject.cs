using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour {
    public int ID;
    Mesh mesh = null;


	// Use this for initialization
	void Start () {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh, this.transform.position + Vector3.up * this.transform.localScale.y * 0.5f, this.transform.rotation, this.transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif

    void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerController = other.GetComponent<PlayerInputController>();
        if (playerController != null && playerController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td == null)
            {
                Debug.LogErrorFormat("TeleporterObject:Character [{0}] Enter Teleporter [{1}], But ReleporterDefine not existed", playerController.character.Info.Name, this.ID);
                return;
            }
            Debug.LogErrorFormat("TeleporterObject:Character [{0}] Enter Teleporter [{1} : {2}]", playerController.character.Info.Name, td.ID,td.Name);
            if (td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                    MapService.Instance.SendMapTeleport(this.ID);
                else
                    Debug.LogErrorFormat("Teleporter ID:{0} LinkTD {1} error!", td.ID, td.LinkTo);
            }
        }
    }
}

using Models;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap minimap;

        private Collider minimapBoundingBox;
        public Collider MinimapBoundingBox
        {
            get { Debug.Log("Getting MinimapBoundingBox value: " + (minimapBoundingBox == null ? "null" : "not null"));
                return minimapBoundingBox; }
        }

        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                    return null;
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }
        void Start()
        {
            Debug.Log("MinimapManager Start: minimapBoundingBox is " + (minimapBoundingBox == null ? "null" : "not null"));
        }


        public void UpdateMinimap(Collider minimapBoundingBox)
        {
            Debug.Log("MinimapBoundingBox value: " + (minimapBoundingBox == null ? "null" : "not null"));

            this.minimapBoundingBox = minimapBoundingBox;
            if (this.minimap != null)
                this.minimap.UpdateMap();
            if (minimapBoundingBox == null)
            {
                Debug.LogWarning("UpdateMinimap.minimapBoundingBox is null or destroyed!");
                return;
            }
            Debug.Log("MinimapBoundingBox value: " + (minimapBoundingBox == null ? "null" : "not null"));

        }
    }
}

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
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }
    }
}

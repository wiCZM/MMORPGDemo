using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Models;

namespace GameServer.Managers
{
    /// <summary>
    /// 刷怪管理器
    /// </summary>
    class SpawnManager
    {
        private List<Spawner> Rules = new List<Spawner>();

        private Map Map;

        public void Init(Map map)
        {
            this.Map = map;
            Debug.WriteLine("SpawnManager.Init: Initializing with map ID " + map.Define.ID);

            if (DataManager.Instance.SpawnRules.ContainsKey(map.Define.ID))
            {
                foreach (var define in DataManager.Instance.SpawnRules[map.Define.ID].Values)
                {
                    this.Rules.Add(new Spawner(define,this.Map));
                }
            }
            Debug.WriteLine("SpawnManager.Init: Initialization completed with " + this.Rules.Count + " rules loaded");
        }
        public void Update()
        {
            Debug.WriteLine("SpawnManager.Update: Updating...");

            if (Rules.Count == 0)
            {
                Debug.WriteLine("SpawnManager.Update: No rules to process, returning");
                return;
            }

            for (int i = 0; i < this.Rules.Count; i++)
            {
                this.Rules[i].Update();
            }

            Debug.WriteLine("SpawnManager.Update: Update completed");
        }
    }
}

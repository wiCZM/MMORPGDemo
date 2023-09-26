﻿using System;
using Common;
using Common.Data;
using GameServer.Models;

namespace GameServer.Managers
{
    class Spawner
    {
        public SpawnRuleDefine Define { get; set; }

        private Map Map;
        /// <summary>
        /// 刷新时间
        /// </summary>
        private float spawnTime = 0;

        /// <summary>
        /// 消失时间
        /// </summary>
        private float unspawnTime = 0;

        private bool spawned = false;

        private SpawnPointDefine spawnPoint = null;


        public Spawner(SpawnRuleDefine define, Map map)
        {
            this.Define = define;
            this.Map = map;

            if (DataManager.Instance.SpawnPoints.ContainsKey(this.Map.ID))
            {
                if (DataManager.Instance.SpawnPoints[this.Map.ID].ContainsKey(this.Define.SpawnPoint))
                {
                    spawnPoint = DataManager.Instance.SpawnPoints[this.Map.ID][this.Define.SpawnPoint];
                }
                else
                {
                    Log.ErrorFormat("SpawnRule[{0}] SpawnPoint[{1} not existed", this.Define.ID, this.Define.SpawnPoint);
                }
            }
        }

        public void Update()
        {
            if (this.CanSpawn())
            {
                this.Spawn();
            }
        }

        bool CanSpawn()
        {
            if (this.spawned)
                return false;
            if (this.unspawnTime + this.Define.SpawnPeriod > Time.time)
                return false;

            return true;
        }

        public void Spawn()
        {
            this.spawned = true;
            Log.InfoFormat("Map[{0}]Spawn[{1}:Mon:{2}],Lv:{3} At Point : [4]", this.Define.MapID, this.Define.ID, this.Define.SpawnMonID, this.Define.SpawnLevel, this.Define.SpawnPoint);
            this.Map.MonsterManager.Create(this.Define.SpawnMonID, this.Define.SpawnLevel, this.spawnPoint.Position, this.spawnPoint.Direction);
        }
    }
}
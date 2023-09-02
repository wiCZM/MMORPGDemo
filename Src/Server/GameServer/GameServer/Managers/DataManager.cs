using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using Common;
using Common.Data;
using Newtonsoft.Json;
namespace GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        internal string DataPath;
        internal Dictionary<int, MapDefine> Maps = null;
        internal Dictionary<int, CharacterDefine> Characters = null;
        internal Dictionary<int, TeleporterDefine> Teleporters = null;
        public Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
        public Dictionary<int, Dictionary<int,SpawnRuleDefine>> SpawnRules = null;
        public Dictionary<int, NpcDefine> Npcs = null;
        public Dictionary<int, ItemDefine> Items = null;

        public DataManager()
        {
            this.DataPath = "Data/";
            Log.Info("DataManager > DataManager()");
        }

        internal void Load()
        {
            string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
            this.Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
            this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);
        }
    }
}
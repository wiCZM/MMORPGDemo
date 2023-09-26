using System;
using Network;
using UnityEngine;

using Common.Data;
using SkillBridge.Message;
using Models;
using Managers;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {

        public int CurrentMapId = 0;
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEnitySync);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);
            foreach (var cha in response.Characters)
            {
                if ((User.Instance.CurrentCharacter == null || cha.Type == CharacterType.Player && User.Instance.CurrentCharacter.Id == cha.Id ))//需要修改成判断当前player,简化
                {//当前角色切换地图
                    User.Instance.CurrentCharacter = cha;
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId != response.mapId)
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:CharID:{0}", response.entityId);
            if (response.entityId != User.Instance.CurrentCharacter.EntityId)
                CharacterManager.Instance.RemoveCharacter(response.entityId);
            else
                CharacterManager.Instance.Clear();
        }



        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }

        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity)
        {
            //Debug.LogFormat("MapEntitySyncResponse: ID:{0} POS:{1} DIR:{2} SPD:{3}", entity.Id, entity.Direction, entity.String(), entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEnitySync(object sender, MapEntitySyncResponse response)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse: Entitys:{0}",response.entitySyncs.Count);
            sb.AppendLine();
            foreach (var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat("    [{0}]evt:{1} entity:{2}",entity.Id,entity.Event,entity.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }

        internal void SendMapTeleport(int teleporterID)
        {
            Debug.LogFormat("MapTeleportRequest: teleporterID:{0}", teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }
    }
}

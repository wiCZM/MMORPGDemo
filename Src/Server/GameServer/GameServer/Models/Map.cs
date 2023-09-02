using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        internal Map(MapDefine define)
        {
            this.Define = define;
        }

        internal void Update()
        {
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        //internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        //{
        //    Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

        //    character.Info.mapId = this.ID;

        //    NetMessage message = new NetMessage();
        //    message.Response = new NetMessageResponse();

        //    message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
        //    message.Response.mapCharacterEnter.mapId = this.Define.ID;
        //    message.Response.mapCharacterEnter.Characters.Add(character.Info);

        //    foreach (var kv in this.MapCharacters)
        //    {
        //        message.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
        //        this.SendCharacterEnterMap(kv.Value.connection, character.Info);
        //    }

        //    this.MapCharacters[character.Id] = new MapCharacter(conn, character);

        //    byte[] data = PackageHandler.PackMessage(message);
        //    conn.SendData(data, 0, data.Length);
        //}


        //

        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;

            // 发送新角色信息给地图上的所有其他角色
            NetMessage newCharacterMessage = new NetMessage();
            newCharacterMessage.Response = new NetMessageResponse();
            newCharacterMessage.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            newCharacterMessage.Response.mapCharacterEnter.mapId = this.Define.ID;
            newCharacterMessage.Response.mapCharacterEnter.Characters.Add(character.Info);
            byte[] newCharacterData = PackageHandler.PackMessage(newCharacterMessage);

            foreach (var kv in this.MapCharacters)
            {
                kv.Value.connection.SendData(newCharacterData, 0, newCharacterData.Length);
            }

            // 为新进入的角色创建完整的消息
            NetMessage fullMessage = new NetMessage();
            fullMessage.Response = new NetMessageResponse();
            fullMessage.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            fullMessage.Response.mapCharacterEnter.mapId = this.Define.ID;
            fullMessage.Response.mapCharacterEnter.Characters.Add(character.Info);
            foreach (var kv in this.MapCharacters)
            {
                fullMessage.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
            }
            byte[] fullData = PackageHandler.PackMessage(fullMessage);
            conn.SendData(fullData, 0, fullData.Length);

            this.MapCharacters[character.Id] = new MapCharacter(conn, character);
        }

        //角色离开
        internal void CharacterLeave(Character cha)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, cha.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, cha);
            }
            this.MapCharacters.Remove(cha.Id);
        }

        void SendCharacterEnterMap(NetConnection<NetSession> conn, NCharacterInfo character)
        {
            if (conn == null || character == null)
            {
                // Log an error or throw an exception based on your design.
                throw new ArgumentNullException("Connection or Character info cannot be null.");
            }

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            message.Response.mapCharacterEnter.mapId = this.Define.ID;
            message.Response.mapCharacterEnter.Characters.Add(character);

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }

        private void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character character)
        {
            if (conn == null || character == null)
            {
                // Log an error or throw an exception based on your design.
                throw new ArgumentNullException("Connection or Character info cannot be null.");
            }

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();

            message.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            message.Response.mapCharacterLeave.characterId = character.Id;

            byte[] data = PackageHandler.PackMessage(message);
            conn.SendData(data, 0, data.Length);
        }


        internal void UpdateEntity(NEntitySync entity)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.entityId == entity.Id)
                {
                    kv.Value.character.Position = entity.Entity.Position;
                    kv.Value.character.Direction = entity.Entity.Direction;
                    kv.Value.character.Speed = entity.Entity.Speed;
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(kv.Value.connection,entity);
                }
            }
        }
    }
}

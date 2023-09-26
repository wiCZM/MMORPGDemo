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
using System.Diagnostics;

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

        /// <summary>
        /// 地图中的角色,以CharacterID为Key
        /// </summary>
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();

        /// <summary>
        /// 刷怪管理器
        /// </summary>
        /// <param name="define"></param>
        SpawnManager SpawnManager = new SpawnManager();

        public MonsterManager MonsterManager = new MonsterManager();

        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
            Console.WriteLine("Map created: " + define.ID);
        }

        internal void Update()
        {
            SpawnManager.Update();
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;

            this.MapCharacters[character.Id] = new MapCharacter(conn, character);

            // 发送新角色信息给地图上的所有其他角色
            conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;

            foreach (var kv in this.MapCharacters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character != character)
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
            }

            foreach (var kv in this.MonsterManager.Monsters)
            {
                conn.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            }
            conn.SendResponse();
       }

        private void AddCharacterEnterMap(NetConnection<NetSession> connection, NCharacterInfo info)
        {
            // 初始化新的响应，用于告知新角色的进入
            connection.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            connection.Session.Response.mapCharacterEnter.mapId = this.ID;
            // 将新进入的角色信息添加到响应中
            connection.Session.Response.mapCharacterEnter.Characters.Add(info);
            connection.SendResponse();
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
            if (conn.Session.Response.mapCharacterEnter == null)
            {
                conn.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                conn.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }
            conn.Session.Response.mapCharacterEnter.Characters.Add(character);
            conn.SendResponse();
        }

        private void SendCharacterLeaveMap(NetConnection<NetSession> conn, Character character)
        {
            Log.InfoFormat("SendCharacterLeaveMap To:{0}:{1} : Map :{2} Character : {3}:{4}", conn.Session.Character.Id, conn.Session.Character.Info.Name,this.Define.ID,character.Id,character.Info.Name);
            conn.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            conn.Session.Response.mapCharacterLeave.entityId = character.entityId;
            conn.SendResponse();
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

        /// <summary>
        /// 怪物进入地图
        /// </summary>
        /// <param name="monster"></param>
        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} monsterId:{1}", this.Define.ID, monster.Id);

            foreach (var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }
        }
    }
}

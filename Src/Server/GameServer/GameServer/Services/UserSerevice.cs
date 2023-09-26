﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;

namespace GameServer.Services
{
    class UserService : Singleton<UserService>
    {

        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(this.OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(this.OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(this.OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(this.OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(this.OnGameLeave);
        }

        public void Init()
        {

        }

        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat("UserLoginRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户不存在";
            }
            else if (user.Password != request.Passward)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                sender.Session.User = user;
                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";
                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();

                if (user.Player != null)
                {

                    sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;
                    foreach (var c in user.Player.Characters)
                    {
                        NCharacterInfo info = new NCharacterInfo();
                        info.Id = c.ID;
                        info.Name = c.Name;
                        info.Type = CharacterType.Player;
                        info.Class = (CharacterClass)c.Class;
                        info.ConfigId = c.ID;
                        sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);
                    }
                }
                else
                {
                    sender.Session.Response.userLogin.Errormsg = "Player data is unavailable.";
                }
            }
            sender.SendResponse();
        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            Log.InfoFormat("UserRegisterRequest: User:{0}  Pass:{1}", request.User, request.Passward);
            sender.Session.Response.userRegister = new UserRegisterResponse();
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user != null)
            {
                sender.Session.Response.userRegister.Result = Result.Failed;
                sender.Session.Response.userRegister.Errormsg = "用户已存在.";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                sender.Session.Response.userRegister.Result = Result.Success;
                sender.Session.Response.userRegister.Errormsg = "None";
            }
            sender.SendResponse();
        }

        private void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat("UserCreateCharacterRequest: Name:{0}  Class:{1}", request.Name, request.Class);

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                Level = 1,
                MapID = 1,
                MapPosX = 5000, //初始出生位置X
                MapPosY = 4000, //初始出生位置Y
                MapPosZ = 820,
                Gold = 5000, //初始10万金币
                Equips = new byte[28]
            };
            var bag = new TCharacterBag();
            bag.Owner = character;
            bag.Items = new byte[0];
            bag.Unlocked = 20;
            character.Bag = DBService.Instance.Entities.CharacterBags.Add(bag);
            character = DBService.Instance.Entities.Characters.Add(character);
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1,
                ItemCount = 20,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 2,
                ItemCount = 20,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1031,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1032,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1033,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1034,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1035,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1036,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1037,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1038,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1039,
                ItemCount = 1,
            });
            character.Items.Add(new TCharacterItem()
            {
                Owner = character,
                ItemID = 1040,
                ItemCount = 1,
            });
            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();

            sender.Session.Response.createChar = new UserCreateCharacterResponse();
            sender.Session.Response.createChar.Result = Result.Success;
            sender.Session.Response.createChar.Errormsg = "None";

            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass)c.Class;
                    info.ConfigId = c.ID;
                sender.Session.Response.createChar.Characters.Add(info);
            }
            sender.SendResponse();
        }

        void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbchar = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat("UserGameEnterRequest: characterID:{0}:{1} Map:{2}", dbchar.ID, dbchar.Name, dbchar.MapID);
            Character character = CharacterManager.Instance.AddCharacter(dbchar);
            SessionManager.Instance.AddSession(character.Id, sender);
            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";

            //进入成功,发送初始角色信息
            sender.Session.Response.gameEnter.Character = character.Info;

            sender.SendResponse();
            sender.Session.Character = character;
            sender.Session.PostResponser = character;
            MapManager.Instance[dbchar.MapID].CharacterEnter(sender, character);
        }

        void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("UserGameLeaveRequest: characterID: {0} Map:{1}", character.Info.Name, character.Info.mapId);
            SessionManager.Instance.RemoveSession(character.Id);
            CharacterLeve(character);
            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            sender.Session.Response.gameLeave.Result = Result.Success;
            sender.Session.Response.gameLeave.Errormsg = "None";
            sender.SendResponse();
        }

        public void CharacterLeve(Character character)
        {
            Log.InfoFormat("CharacterLeve: characterID: {0}:{1}", character.Id, character.Info.Name);
            CharacterManager.Instance.RemoveCharacter(character.Id);
            character.Clear();
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }
}

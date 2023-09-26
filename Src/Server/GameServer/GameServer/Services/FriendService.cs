using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class FriendService : Singleton<FriendService>
    {
        public FriendService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FriendRemoveRequest>(this.OnFriendRemove);
        }

        public void Init()
        {

        }

        ///<summary>
        ///收到加好友请求
        ///</summary>>
        ///<param name="sender"></param>>
        ///<param name="request"></param>>

        void OnFriendAddRequest(NetConnection<NetSession> sender, FriendAddRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddRequest: :FromId : {0} FromName :{1} ToID :{2} ToName : {3}", request.FromId, request.FromName, request.ToId, request.ToName);

            if (request.ToId == 0)
            {//如果没有传入ID,则使用名称查找
                foreach (var cha in CharacterManager.Instance.Characters)
                {
                    if (cha.Value.Data.Name == request.ToName)
                    {
                        request.ToId = cha.Key;
                        break;
                    }
                }
            }
            NetConnection<NetSession> friend = null;
            if (request.ToId > 0)
            {
                if (character.FriendManager.GetFriendInfo(request.ToId) != null)
                {
                    sender.Session.Response.friendAddRes = new FriendAddResponse();
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "已经是好友了";
                    sender.SendResponse();
                    return;
                }
                friend = SessionManager.Instance.GetSession(request.ToId);
            }
            if (friend == null)
            {
                sender.Session.Response.friendAddRes = new FriendAddResponse();
                sender.Session.Response.friendAddRes.Result = Result.Failed;
                sender.Session.Response.friendAddRes.Errormsg = "好友不存在或者不在线";
                sender.SendResponse();
                return;
            }

            Log.InfoFormat("ForwardRequest: : FromId:{0} FromName : {1} ToId :{2} ToName : {3}", request.FromId, request.FromName, request.ToId, request.ToName);
            friend.Session.Response.friendAddReq = request;
            friend.SendResponse();
        }

        /// <summary>
        /// 收到加好友响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>

        private void OnFriendAddResponse(NetConnection<NetSession> sender, FriendAddResponse response)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendAddResponse: :character : {0} Result :{1} FromId :{2} ToId : {3}", character.Id, response.Result, response.Request.FromId, response.Request.ToId);
            sender.Session.Response.friendAddRes = response;
            if (response.Result == Result.Success)
            {//接受了好友请求
                var requester = SessionManager.Instance.GetSession(response.Request.FromId);
                if (requester == null)
                {
                    sender.Session.Response.friendAddRes.Result = Result.Failed;
                    sender.Session.Response.friendAddRes.Errormsg = "请求者已下线";
                }
                else
                {
                    //互相加好友
                    character.FriendManager.AddFriend(requester.Session.Character);
                    requester.Session.Character.FriendManager.AddFriend(character);
                    DBService.Instance.Save();
                    requester.Session.Response.friendAddRes = response;
                    requester.Session.Response.friendAddRes.Result = Result.Success;
                    requester.Session.Response.friendAddRes.Errormsg = "添加好友成功";
                    requester.SendResponse();

                }
            }
            sender.SendResponse();
        }

        void OnFriendRemove(NetConnection<NetSession> sender, FriendRemoveRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnFriendRemove: :character : {0} FriendReletionID :{1}", character.Id, request.Id);
            sender.Session.Response.friendRemove = new FriendRemoveResponse();
            sender.Session.Response.friendRemove.Id = request.Id;

            ///删除自己的好友
            if (character.FriendManager.RemoveFriendByID(request.Id))
            {
                sender.Session.Response.friendRemove.Result = Result.Success;
                //删除别人好友中的自己
                var friend = SessionManager.Instance.GetSession(request.friendId);
                if (friend != null)
                {//好友在线
                    friend.Session.Character.FriendManager.RemoveFriendByIDFriendId(character.Id);
                }
                else
                {//不在线
                    this.RemoveFriend(request.friendId, character.Id);
                }
            }
            else
                sender.Session.Response.friendRemove.Result = Result.Failed;

            DBService.Instance.Save();

            sender.SendResponse();
        }

        void RemoveFriend(int charId, int friendId)
        {
            var removeItem = DBService.Instance.Entities.CharacterFriends.FirstOrDefault(v => v.CharacterID == charId && v.FriendID == friendId);
            if (removeItem != null)
            {
                DBService.Instance.Entities.CharacterFriends.Remove(removeItem);
            }
        }
    }
}

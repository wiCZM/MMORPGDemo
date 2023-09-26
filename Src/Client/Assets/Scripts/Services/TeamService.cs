using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    class TeamService : Singleton<TeamService>, IDisposable
    {
        public void Init()
        {

        }

        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteReq = new TeamInviteRequest();
            message.Request.teamInviteReq.FromId = User.Instance.CurrentCharacter.Id;
            message.Request.teamInviteReq.FromName = User.Instance.CurrentCharacter.Name;
            message.Request.teamInviteReq.ToId = friendId;
            message.Request.teamInviteReq.ToName = friendName;
            NetClient.Instance.SendMessage(message);
        }

        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteRes = new TeamInviteResponse();
            message.Request.teamInviteRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.teamInviteRes.Errormsg = accept ? "组队成功" : "对方拒绝了组队请求";
            message.Request.teamInviteRes.Request = request;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到添加组队请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInviteRequest(object sender, TeamInviteRequest request)
        {
            Debug.Log("OnTeamInviteRequest");
            var config = MessageBox.Show(string.Format("{0} 邀请你加入队伍", request.FromName), "组队请求", MessageBoxType.Confirm, "接受", "拒绝");
            config.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, request);
            };
            config.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, request);
            };
        }

        /// <summary>
        /// 收到组队邀请响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInviteResponse(object sender, TeamInviteResponse message)
        {
            Debug.Log("OnTeamInviteResponse");
            if (message.Result == Result.Success)
                MessageBox.Show(message.Request.ToName + "加入您的队伍", "邀请组队成功");
            else
                MessageBox.Show(message.Errormsg, "邀请组队失败");
        }

        private void OnTeamInfo(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnTeamInfo");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }

        public void SendTeamLeaveRequest(int id)
        {
            Debug.Log("SendTeamLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamLeave = new TeamLeaveRequest();
            message.Request.teamLeave.TeamId = User.Instance.TeamInfo.Id;
            message.Request.teamLeave.characterId = User.Instance.CurrentCharacter.Id;
            NetClient.Instance.SendMessage(message);
        }

        private void OnTeamLeave(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);
                MessageBox.Show("退出成功", "退出队伍");
            }
            else
                MessageBox.Show("退出失败", "退出队伍", MessageBoxType.Error);
        }
    }
}

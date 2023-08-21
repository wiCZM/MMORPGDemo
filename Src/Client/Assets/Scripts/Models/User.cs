using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    public class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }


        public MapDefine CurrentMapData { get; set; }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

        public GameObject CurrentCharacterObject { get; set; }

    }
}

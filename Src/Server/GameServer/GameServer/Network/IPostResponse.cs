using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Network
{
    public interface IPostResponser
    {
        void PostProcess(NetMessageResponse message);
    }
}
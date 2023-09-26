using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class StatusManager
    {
        Character Owner;

        private List<NStatus> Status { get; set; }


        public bool HasStatus
        { get
            { return this.Status.Count > 0; }
        }
        private Character character;

        public StatusManager(Character owner)
        {
            this.Owner = owner;
            this.Status = new List<NStatus>();
        }

        public void AddStatus(StatusType type, int id, int value, StatusAction action)
        {
            this.Status.Add(new NStatus()
            {
                Type = type,
                Id = id,
                Value = value,
                Action = action
            });
        }

        internal void AddGoldChange(int goldDelta)
        {
            if (goldDelta > 0)
            {
                Console.WriteLine($"[StatusManager] Adding Gold: {goldDelta}");

                this.AddStatus(StatusType.Money, 0, goldDelta, StatusAction.Add);
            }
            if (goldDelta < 0)
            {
                Console.WriteLine($"[StatusManager] Removing Gold: {-goldDelta}");

                this.AddStatus(StatusType.Money, 0, -goldDelta, StatusAction.Delete);
            }
        }


        public void AddGoldChange(int id, int count, StatusAction action)
        {
            Console.WriteLine($"[StatusManager] Item Change. ID: {id}, Count: {count}, Action: {action}");

            this.AddStatus(StatusType.Item, id, count, action);
        }

        public void ApplyResponse(NetMessageResponse message)
        {
            if (message.statusNotify == null)
                message.statusNotify = new StatusNotify();
            foreach (var status in this.Status)
            {
                Console.WriteLine($"[StatusManager] Applying Status. Type: {status.Type}, ID: {status.Id}, Value: {status.Value}, Action: {status.Action}");
                message.statusNotify.Status.Add(status);
            }
            this.Status.Clear();

            Console.WriteLine($"[StatusManager] Cleared Status.");
        }

        internal void PostProcess(NetMessageResponse message)
        {
            if (message.statusNotify != null)
            {
                ApplyResponse(message);
                Console.WriteLine($"[StatusManager] PostProcess > StatusdManager : characterID:{this.Owner.Id}:{this.Owner.Info.Name}");
            }
        }
    }
}

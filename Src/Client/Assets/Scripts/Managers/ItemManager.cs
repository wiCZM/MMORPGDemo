using Assets.Scripts.Models;
using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class ItemManager : Singleton<ItemManager>
    {

        public Dictionary<int, Item> Items = new Dictionary<int, Item>();
        internal void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);

                Debug.LogFormat("ItemManager: Init[{ 0}]",item);
            }
        }

        public ItemDefine GetItem(int itemId)
        {


            return null; 
        }

        public bool UseItem(int itemId)
        {
            return false;
        }


        public bool UseItem(ItemDefine itemId)
        {
            return false;
        }



    }
}

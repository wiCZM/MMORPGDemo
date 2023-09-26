using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class BagManager: Singleton<BagManager>
    {

        public int Unlocked;

        public BagItem[] Items;

        NBagInfo Info;

        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            Debug.Log("ItemManager.Instance: " + (ItemManager.Instance != null));
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                Info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        public void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (i >= this.Items.Length) break;  // 添加这一行来避免数组越界异常

                if (kv.Value.Count <= kv.Value.Define.Stacklimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                    Debug.LogFormat("Reset: Setting ItemId {0} with count {1} at index {2}" ,kv.Key, kv.Value.Count, i);
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.Stacklimit)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.Stacklimit;
                        i++;
                        count -= kv.Value.Define.Stacklimit;

                        if (i >= this.Items.Length) break;  // 添加这一行来避免数组越界异常
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                    Debug.LogFormat("Reset: Setting ItemId {0} with count {1} at index {2}", kv.Key, kv.Value.Count, i);
                }
                i++;
            }
        }

        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    item[i] = *item;
                }
            }
        }

        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }

        public void AddItem(int itemId, int count)
        {
            ushort addCount = (ushort)count;
            for (int i = 0; i < Items.Length; i++)
            {
                if (this.Items[i].ItemId == itemId)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].Stacklimit - this.Items[i].Count);
                    if (canAdd >= addCount)
                    {
                        this.Items[i].Count += addCount;
                        addCount = 0;
                        break;
                    }
                    else
                    {
                        this.Items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                    Debug.LogFormat("AddItem: Adding {0} of item {1} to slot {2}. Remaining to add: {3}", canAdd, itemId, i, addCount);
                }
            }
            if (addCount > 0)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (this.Items[i].ItemId == 0)
                    {
                        this.Items[i].ItemId = (ushort)itemId;
                        this.Items[i].Count = addCount;
                        break;
                    }
                    Debug.LogFormat("AddItem: Adding remaining {0} of item {1} to a new slot {2}", addCount, itemId, i);
                }
            }
        }

        public void RemoveItem(int itemId, int count)
        {

        }
    }
}

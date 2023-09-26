using Common.Data;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop, OnOpenShop);
        }

        public bool OnOpenShop(NpcDefine npc)
        {
            
            if (npc != null)
            {
                this.ShowShop(npc.Param);
                return true;
            }
            else
            {
                Debug.LogError("OnOpenShop failed: npc is null!");
                return false;
            }
        }

        public void ShowShop(int shopId)
        {
            ShopDefine shop;
            if (DataManager.Instance.Shops.TryGetValue(shopId, out shop))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop != null)
                {
                    uiShop.Setshop(shop);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("Failed to show shop UI for shop ID {0}", shopId));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Shop with ID {0} not found", shopId));
            }
        }

        internal bool BuyItem(int shopId, int shopItemId)
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemId);
            return true;
        }
    }
}

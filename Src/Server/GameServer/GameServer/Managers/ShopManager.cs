using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        public Result BuyItem(NetConnection<NetSession> sender, int shopId, int shopItemId,int count = 1)
        {
            if (!DataManager.Instance.Shops.ContainsKey(shopId))
                return Result.Failed;

            ShopItemDefine shopItem;
            if (DataManager.Instance.ShopItems[shopId].TryGetValue(shopItemId, out shopItem))
            {
                Log.InfoFormat("BuyItem: :character:{0}:Item:{1} Count:{2} Price:{3}", sender.Session.Character.Id, shopItem.ItemID, count, shopItem.Price);
                if (sender.Session.Character.Gold >= shopItem.Price * count)
                {
                    sender.Session.Character.ItemManager.AddItem(shopItem.ItemID, count);
                    sender.Session.Character.Gold -= shopItem.Price * count;
                    DBService.Instance.Save();
                    return Result.Success;
                } 
            }
            return Result.Failed;
        }
    }
}

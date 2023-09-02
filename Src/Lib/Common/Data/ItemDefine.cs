using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;

namespace Common.Data
{
    public enum ItemFunction
    {
        RecoverHP,
        RecoverMP,
        AddBuff,
        AddExp,
        AddMoney,
        AddItem,
        AddSkillPoint,
    }
    public class ItemDefine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Item_Type Type { get; set; }
        public string Category { get; set; }
        public bool CanUse { get; set; }
        public float UseCD { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int Stacklimit { get; set; } //堆叠限制 
        public string Icon { get; set; }
        public ItemFunction Function { get; set; } //道具功能
        public int Param { get; set; }
        public List<int> Params { get; set; }
    }
}

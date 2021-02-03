using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOI_QUO.Models
{
    public class ItemNo
    {
        public int Ida { get; set; }
        public string ItemNoa { get; set; }
        public string ModelNoa { get; set; }
        public decimal RevisionNoa { get; set; }
        public string ItemShortNamea { get; set; }
        public string ItemNamea { get; set; }
        public string ItemSpeca { get; set; }
        public string OriginalLengtha { get; set; }
        public string ItemGroupa { get; set; }
        public string MakerNamea { get; set; }
        public int EnumItemTypeIda { get; set; }
        public int MasterUnitIda { get; set; }
        public int MasterUnitAltItemUnitCodeIda { get; set; }
        public int EnumIssueTypeIda { get; set; }
        public int EnumStockControlTypeIda { get; set; }
    }
}
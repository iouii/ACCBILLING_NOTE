using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BOI_QUO.Models
{
    public class QuotationShow
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
        public int IdM { get; set; }
        public string BusinessPartnerCodeM { get; set; }
        public string BusinessPartnerNameM { get; set; }
        public string BusinessPartnerShortNameM { get; set; }
        public int EnumBusinessPartnerTypeIdM { get; set; }

        public string itemNo { get; set; }
        public string supllierCode { get; set; }
        public string dateReceive { get; set; }
        public string quotationNo { get; set; }
        public string quotationFile { get; set; }
        public string dateEffective { get; set; }
        public string dateExpriation { get; set; }
        public string dateModify { get; set; }
        public string userCode { get; set; }
        public string quotationType { get; set; }
        public string quotationRemark { get; set; }
        public string quotationRemark1 { get; set; }
        public string quotationRemark2 { get; set; }

    }
}
// Copyright (C) 2015 Sergey Shabalin

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Текущие позиции по клиентским счетам
    /// </summary>
    public class AccountBalance
    {
        /// <summary>
        /// firmid  STRING  Идентификатор фирмы  
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// sec_code  STRING  Код бумаги  
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }
        
        /// <summary>
        /// trdaccid  STRING  Торговый счет
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

        /// <summary>
        /// depaccid  STRING  Счет депо  
        /// </summary>
        [JsonProperty("depaccid")]
        public string DepAccId { get; set; }

        /// <summary>
        /// openbal  NUMBER  Входящий остаток  
        /// </summary>
        [JsonProperty("openbal")]
        public decimal OpenBal { get; set; }

        /// <summary>
        /// currentpos  NUMBER  Текущий остаток 
        /// </summary>
        [JsonProperty("currentpos")]
        public decimal CurrentPos { get; set; }

        /// <summary>
        /// plannedpossell  NUMBER  Плановая продажа  
        /// </summary>
        [JsonProperty("plannedpossell")]
        public decimal PlannedPosSell { get; set; }

        /// <summary>
        /// plannedposbuy   NUMBER  Плановая покупка  
        /// </summary>
        [JsonProperty("plannedposbuy")]
        public decimal PlannedPosBuy { get; set; }

        /// <summary>
        /// planbal  NUMBER  Контрольный остаток простого клиринга, равен входящему 
        /// остатку минус плановая позиция на продажу, включенная в простой клиринг    
        /// </summary>
        [JsonProperty("planbal")]
        public decimal PlanBal { get; set; }

        /// <summary>
        /// usqtyb  NUMBER  Куплено  
        /// </summary>
        [JsonProperty("usqtyb")]
        public decimal UsQtyBuy { get; set; }

        /// <summary>
        /// usqtys  NUMBER  Продано  
        /// </summary>
        [JsonProperty("usqtys")]
        public decimal UsQtySell { get; set; }

        /// <summary>
        /// planned  NUMBER  Плановый остаток, равен текущему остатку минус плановая 
        /// позиция на продажу  
        /// </summary>
        [JsonProperty("planned")]
        public decimal Planned { get; set; }

        /// <summary>
        /// settlebal  NUMBER  Плановая позиция после проведения расчетов  
        /// </summary>
        [JsonProperty("settlebal")]
        public decimal SettleBal { get; set; }

        /// <summary>
        /// bank_acc_id  STRING  Идентификатор расчетного счета/кода в клиринговой 
        /// организации  
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccId { get; set; }

        /// <summary>
        /// firmuse  NUMBER  Признак счета обеспечения. Возможные значения: 
        /// «0» – для обычных счетов, «1» – для счета обеспечения. 
        /// </summary>
        [JsonProperty("firmuse")]
        public byte FirmUse { get; set; }
        
    }
}

// Copyright (C) 2015 Victor Baybekov

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using QuikSharp.DataStructures.Transaction;

namespace QuikSharp.DataStructures.Transaction
{


    // ��� ��������� �� ���� �������� "�������� ��� ��������" (TPH) � ����� ��������
    // ��� ������. �� ����� ��� �������� ������� - �� ������ �� TransactionAction
    // ����� ������� ����� ������� �� �������� �������
    // � ����� ������� DSL � F# � �� ��������� �������� ������ � ���� ������ ������ ����� - ��� ������ ���������


    /// <summary>
    /// ������ .tri-����� � ����������� ����������
    /// �������������� ��� QLua
    /// </summary>
    public class Transaction {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// ������� ���������� ���������� QUIK ��� ��������� ������ �� ���������� ������������. 
        /// </summary>
        public event TransReplyHandler OnTransReply;
        internal void OnTransReplyCall(TransactionReply reply) {
            if (OnTransReply != null) { OnTransReply(reply); }
            // this should happen only once per transaction id
            Trace.Assert(TransactionReply == null);
            TransactionReply = reply;
        }
        /// <summary>
        /// TransactionReply
        /// </summary>
        public TransactionReply TransactionReply { get; set; }

        /// <summary>
        /// ������� ���������� ���������� QUIK ��� ��������� ����� ������ ��� ��� ��������� ���������� ������������ ������.
        /// </summary>
        public event OrderHandler OnOrder;
        internal void OnOrderCall(Order order) {
            if (OnOrder != null) { OnOrder(order); }
            if (Orders == null) { Orders = new List<Order>(); }
            Orders.Add(order);
        }
        /// <summary>
        /// Orders
        /// </summary>
        public List<Order> Orders { get; set; }

        /// <summary>
        /// ������� ���������� ���������� QUIK ��� ��������� ������. 
        /// </summary>
        public event TradeHandler OnTrade;

        internal void OnTradeCall(Trade trade) {
            if (OnTrade != null) { OnTrade(trade); }
            if (Trades == null) { Trades = new List<Trade>(); }
            Trades.Add(trade);
        }
        /// <summary>
        /// Trades
        /// </summary>
        public List<Trade> Trades { get; set; }


        // TODO inspect with actual data
        /// <summary>
        /// ���������� ���������
        /// </summary>
        /// <returns></returns>
        public bool IsComepleted() {
            if (Orders == null || Orders.Count == 0) return false;
            var last = Orders[Orders.Count - 1];
            return !last.Flags.HasFlag(OrderTradeFlags.Active)
                   &&
                   !last.Flags.HasFlag(OrderTradeFlags.Canceled);
        }

        /// <summary>
        /// Error message returned by sendTransaction
        /// </summary>
        public string ErrorMessage { get; set; }


        ///////////////////////////////////////////////////////////////////////////////
        /// 
        ///  Transaction specification properties start here
        /// 
        ///////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// ��� ������, �� �������� ����������� ����������, �������� TQBR. ������������ ��������
        /// </summary>
        public string CLASSCODE { get; set; }
        /// <summary>
        /// ��� �����������, �� �������� ����������� ����������, �������� SBER
        /// </summary>
        public string SECCODE { get; set; }
        /// <summary>
        /// ��� ����������, ������� ���� �� ��������� ��������:
        /// </summary>
        public TransactionAction? ACTION { get; set; }
        /// <summary>
        /// ������������� ��������� ������ (��� �����)
        /// </summary>
        public string FIRM_ID { get; set; }
        /// <summary>
        /// ����� ����� ��������. �������� ���������� ��� �ACTION� = �KILL_ALL_FUTURES_ORDERS�. �������� ������������ � ��������/������� �������� ��������.
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        /// 20-�� ���������� ��������� ����, ����� ��������� ��� ������� � ��������� ����������� � ��� �� ������������, ��� � ��� ����� ������ �������. �������� ������������ ������ ��� ��������� ����������. �������������� ��������
        /// </summary>
        public string CLIENT_CODE { get; set; }

        /// <summary>
        /// ���������� ����� � ������, ������������ ��������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int>))]
        public int QUANTITY { get; set; }
        /// <summary>
        /// ���� ������, �� ������� �����������. ������������ ��������. 
        /// ��� ����������� �������� ������ (TYPE=M) �� ������� ����� FORTS 
        /// ���������� ��������� �������� ���� � ������� ��������� 
        /// (���������� ��� ����������� ��������� � � ����������� �� ��������������), 
        /// ������ ��� ����� ����� ��������� �� �������� ����. ��� ������ ������ ��� 
        /// ����������� �������� ������ ������� price= 0.
        /// </summary>
        [JsonConverter(typeof(DecimalG29ToStringConverter))]
        public decimal PRICE { get; set; }

        /// <summary>
        /// ����������� ������, ������������ ��������. ��������: �S� � �������, �B� � ������
        /// </summary>
        public TransactionOperation? OPERATION { get; set; }

        /// <summary>
        /// ���������� ����������������� ����� ������, �������� �� 1 �� 2 294 967 294
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? TRANS_ID { get; set; }

        // ReSharper restore InconsistentNaming

    

    

        // ReSharper disable InconsistentNaming

        /// <summary>
        /// ��� ������, �������������� ��������. 
        /// ��������: �L� � �������������� (�� ���������), �M� � ��������
        /// </summary>
        public TransactionType? TYPE { get; set; }

        /// <summary>
        /// ������� ����, �������� �� ������ ������� ������-�������. ��������� ��������: �YES� ��� �NO�. �������� �� ��������� (���� �������� �����������): �NO�
        /// </summary>
        public YesOrNo? MARKET_MAKER_ORDER { get; set; }

        /// <summary>
        /// ������� ���������� ������, �������������� ��������. ��������� ��������:
        /// </summary>
        public ExecutionCondition? EXECUTION_CONDITION { get; set; }

        /// <summary>
        /// ����� ������ ����-� � ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? REPOVALUE { get; set; }
        /// <summary>
        /// ��������� �������� �������� � ������ �� ������ ����-�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? START_DISCOUNT { get; set; }
        /// <summary>
        /// ������ ���������� �������� �������� � ������ �� ������ ����-�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? LOWER_DISCOUNT { get; set; }
        /// <summary>
        /// ������� ���������� �������� �������� � ������ �� ������ ����-�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? UPPER_DISCOUNT { get; set; }
        /// <summary>
        /// ����-����, �� ������� �����������. ������������ ������ ��� �ACTION� = �NEW_STOP_ORDER�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? STOPPRICE { get; set; }
        /// <summary>
        /// ��� ����-������. ��������� ��������:
        /// </summary>
        public StopOrderKind? STOP_ORDER_KIND { get; set; }
        /// <summary>
        /// ����� ����������� �������. ������������ ������ ��� �STOP_ORDER_KIND� = �CONDITION_PRICE_BY_OTHER_SEC�.
        /// </summary>
        public string STOPPRICE_CLASSCODE { get; set; }
        /// <summary>
        /// ��� ����������� �������. ������������ ������ ��� �STOP_ORDER_KIND� = �CONDITION_PRICE_BY_OTHER_SEC�
        /// </summary>
        public string STOPPRICE_SECCODE { get; set; }
        /// <summary>
        /// ����������� ����������� ��������� ����-����. ������������ ������ ��� �STOP_ORDER_KIND� = �CONDITION_PRICE_BY_OTHER_SEC�. ��������� ��������:� �<=� ��� �>= �
        /// </summary>
        public string STOPPRICE_CONDITION { get; set; }
        /// <summary>
        /// ���� ��������� �������������� ������. ������������ ������ ��� �STOP_ORDER_KIND� = �WITH_LINKED_LIMIT_ORDER�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? LINKED_ORDER_PRICE { get; set; }
        /// <summary>
        /// ���� �������� ����-������. ��������� ��������: �GTC� � �� ������, �TODAY� - �� ��������� ������� �������� ������, ���� � ������� ������Ļ.
        /// </summary>
        public string EXPIRY_DATE { get; set; }
        /// <summary>
        /// ���� ������� �����-����� ��� ������ ���� �����-������ � ����-�����
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? STOPPRICE2 { get; set; }
        /// <summary>
        /// ������� ���������� ������ �� �������� ���� ��� ����������� ������� �����-�����. �������� �YES� ��� �NO�. �������� ������ ���� �����-������ � ����-�����
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? MARKET_STOP_LIMIT { get; set; }
        /// <summary>
        /// ������� ���������� ������ �� �������� ���� ��� ����������� ������� �����-������. �������� �YES� ��� �NO�. �������� ������ ���� �����-������ � ����-�����
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? MARKET_TAKE_PROFIT { get; set; }
        /// <summary>
        /// ������� �������� ������ ���� �����-������ � ����-����� � ������� ������������� ��������� �������. �������� �YES� ��� �NO�
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? IS_ACTIVE_IN_TIME { get; set; }
        /// <summary>
        /// ����� ������ �������� ������ ���� �����-������ � ����-����� � ������� ������ѻ
        /// </summary>
        [JsonConverter(typeof(HHMMSSDateTimeConverter))]
        public DateTime? ACTIVE_FROM_TIME { get; set; }
        /// <summary>
        /// ����� ��������� �������� ������ ���� �����-������ � ����-����� � ������� ������ѻ
        /// </summary>
        [JsonConverter(typeof(HHMMSSDateTimeConverter))]
        public DateTime? ACTIVE_TO_TIME { get; set; }
        /// <summary>
        /// ��� ����������� � �������� �� ����������� ������.����������� ��� �ACTION� = �NEW_NEG_DEAL�, �ACTION� = �NEW_REPO_NEG_DEAL� ��� �ACTION� = �NEW_EXT_REPO_NEG_DEAL�
        /// </summary>
        public string PARTNER { get; set; }
        /// <summary>
        /// ����� ������, ��������� �� �������� �������. ����������� ��� �ACTION� = �KILL_ORDER� ��� �ACTION� = �KILL_NEG_DEAL����� �ACTION� = �KILL_QUOTE�
        /// </summary>
        public string ORDER_KEY { get; set; }
        /// <summary>
        /// ����� ����-������, ��������� �� �������� �������. ����������� ������ ��� �ACTION� = �KILL_STOP_ORDER�
        /// </summary>
        public string STOP_ORDER_KEY { get; set; }

        /// <summary>
        /// ��� �������� ��� ���������� ����������� ������
        /// </summary>
        public string SETTLE_CODE { get; set; }
        /// <summary>
        /// ���� ������ ����� ����
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? PRICE2 { get; set; }
        /// <summary>
        /// ���� ����. �������� ������ ����-�
        /// </summary>
        public string REPOTERM { get; set; }
        /// <summary>
        /// ������ ����, � ���������
        /// </summary>
        public string REPORATE { get; set; }
        /// <summary>
        /// ������� ���������� ����� �� ����� �������� ���� (�YES�, �NO�)
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? BLOCK_SECURITIES { get; set; }
        /// <summary>
        /// ������ �������������� ����������, �������������� � ������ ������������ ������ ����� ����, � ���������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? REFUNDRATE { get; set; }
        /// <summary>
        /// ��������� �����������, ��������� � ������ - ��������� (brokerref in Trades/Orders). 
        /// ������������ ��� ������ ������ ������
        /// </summary>
        [JsonProperty("brokerref")]
        public string Comment { get; set; }
        /// <summary>
        /// ������� ������� ������ (YES/NO). �������� ����������� ������
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? LARGE_TRADE { get; set; }
        /// <summary>
        /// ��� ������ �������� �� ����������� ������, �������� �SUR� � ����� ��, �USD� � ������� ���. �������� ����������� ������
        /// </summary>
        public string CURR_CODE { get; set; }
        /// <summary>
        /// ����, �� ����� �������� � �� ��� ���� �������������� ������ (�������� ����������� ������). ��������� ��������:
        /// </summary>
        public ForAccount? FOR_ACCOUNT { get; set; }
        /// <summary>
        /// ���� ���������� ����������� ������
        /// </summary>
        public string SETTLE_DATE { get; set; }
        /// <summary>
        /// ������� ������ ����-������ ��� ��������� ���������� ��������� �������������� ������. ������������ ������ ��� �STOP_ORDER_KIND� = �WITH_LINKED_LIMIT_ORDER�. ��������� ��������: �YES� ��� �NO�
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? KILL_IF_LINKED_ORDER_PARTLY_FILLED { get; set; }
        /// <summary>
        /// �������� ������� �� ��������� (��������) ���� ��������� ������. ������������ ��� �STOP_ORDER_KIND� = �TAKE_PROFIT_STOP_ORDER� ��� �ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? OFFSET { get; set; }
        /// <summary>
        /// ������� ��������� �������. ��������� ��������:
        /// </summary>
        public OffsetUnits? OFFSET_UNITS { get; set; }
        /// <summary>
        /// �������� ��������� ������. ������������ ��� �STOP_ORDER_KIND� = �TAKE_PROFIT_STOP_ORDER� ��� ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER�
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? SPREAD { get; set; }
        /// <summary>
        /// ������� ��������� ��������� ������. ������������ ��� �STOP_ORDER_KIND� = �TAKE_PROFIT_STOP_ORDER� ��� �ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER�
        /// </summary>
        public OffsetUnits? SPREAD_UNITS { get; set; }
        /// <summary>
        /// ��������������� ����� ������-�������. ������������ ��� �STOP_ORDER_KIND� = �ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER� ��� �ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER�
        /// </summary>
        public string BASE_ORDER_KEY { get; set; }
        /// <summary>
        /// ������� ������������� � �������� ������ ������ ��� ����������� ������������ ���������� ����� ������-�������. ��������� ��������: �YES� ��� �NO�. ������������ ��� �STOP_ORDER_KIND� = �ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER� ��� �ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER�
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? USE_BASE_ORDER_BALANCE { get; set; }
        /// <summary>
        /// ������� ��������� ������ ��� ����������� ��� ��������� ���������� ������-�������. ��������� ��������: �YES� ��� �NO�. ������������ ��� �STOP_ORDER_KIND� = �ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER� ��� �ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER�
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? ACTIVATE_IF_BASE_ORDER_PARTLY_FILLED { get; set; }
        /// <summary>
        /// ������������� �������� ��������� ��� ��������� ��� ��������. 
        /// ������������ �������� ������ ������ �� ����� FORTS
        /// </summary>
        public string BASE_CONTRACT { get; set; }
        /// <summary>
        /// ������ ������������ ������ �� ����� FORTS. �������� �������� �ACTION� = �MOVE_ORDERS� ��������� ��������: �0� � �������� ���������� � ������� ��� ���������, �1� � �������� ���������� � ������� �� �����, �2� � ��� ������������ ����� ��������� � ������� ���� �� � ����� ������, ��� ������ ���������
        /// </summary>
        public string MODE { get; set; }
        /// <summary>
        /// ����� ������ ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? FIRST_ORDER_NUMBER { get; set; }
        /// <summary>
        /// ���������� � ������ ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int?>))]
        public int? FIRST_ORDER_NEW_QUANTITY { get; set; }
        /// <summary>
        /// ���� � ������ ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? FIRST_ORDER_NEW_PRICE { get; set; }
        /// <summary>
        /// ����� ������ ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? SECOND_ORDER_NUMBER { get; set; }
        /// <summary>
        /// ���������� �� ������ ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int?>))]
        public int? SECOND_ORDER_NEW_QUANTITY { get; set; }
        /// <summary>
        /// ���� �� ������ ������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? SECOND_ORDER_NEW_PRICE { get; set; }
        /// <summary>
        /// ������� ������ �������� ������ �� ������� �����������. ������������ ������ ��� �ACTION� = �NEW_QUOTE�. ��������� ��������: �YES� ��� �NO�
        /// </summary>
        // TODO (?) Is No default here?	
        public YesOrNo? KILL_ACTIVE_ORDERS { get; set; }
        /// <summary>
        /// ����������� �������� � ������, �������������� �������
        /// </summary>
        public string NEG_TRADE_OPERATION { get; set; }
        /// <summary>
        /// ����� �������������� ������� ������ ��� ����������
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? NEG_TRADE_NUMBER { get; set; }
        /// <summary>
        /// ����� �������� �������, ��� ���� ������ = ����.�������� ��� ������
        /// </summary>
        public string VOLUMEMN { get; set; }
        /// <summary>
        /// ����� �������� �������, ��� ���� ������ = ���������� ���.��������
        /// </summary>
        public string VOLUMEPL { get; set; }
        /// <summary>
        /// ����������� �����������
        /// </summary>
        public string KFL { get; set; }
        /// <summary>
        /// ����������� ����������� ������������ �����������
        /// </summary>
        public string KGO { get; set; }
        /// <summary>
        /// ��������, ������� ����������, ����� �� ����������� �������� ��� ��� �������� ������� �� �����: ��� USE_KGO=Y � �������� ��� ���������. ��� USE_KGO=N � �������� ��� �� �����������. ��� ��������� ������ �� ������� ����� ���������� ����� � �������������� ���������� (��. �������� ������) ��������� ������� USE_KGO= Y
        /// </summary>
        public string USE_KGO { get; set; }
        /// <summary>
        /// ������� �������� ��������� ���� ������ � �������� ���������� ���. 
        /// �������� �������� ����� FORTS. �������������� �������� ���������� 
        /// ��������� ����� ������ �� ������� �������� ����ѻ � ����: ������� ����ѻ. 
        /// ��������� ��������: �YES��- ��������� ��������, �NO��- �����������
        /// </summary>
        public YesOrNo? CHECK_LIMITS { get; set; }
        /// <summary>
        /// ������, ������� ��������� ��� ������ ���� ��� ���. ������ ����� ���� ��������� ������ ����� �������������, ���������� ���������� �������� ����� ��������� � ����� �������. �������� ������������ ����� ����� ������������ ����� ����������� �� 10 �������� (����������� ����� � �����). �������������� ��������
        /// </summary>
        public string MATCHREF { get; set; }
        /// <summary>
        /// ����� ������������� ����������� �� ���������� ������. ��������� ��������: �Y� - �������, ���������� ������ ���������� ����������� ��������, �N� - �������� (�� ���������), ���������� ������ �������� ����� ��������
        /// </summary>
        public string CORRECTION { get; set; }


        // ReSharper restore InconsistentNaming

        [JsonIgnore] // do not pass to Quik
        public bool IsManual { get; set; }
    }
}
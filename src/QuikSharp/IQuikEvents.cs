// Copyright (C) 2015 Victor Baybekov

using System;

namespace QuikSharp {

    // TODO Redirect these callbacks to events or rather do with events from the beginning

    /// <summary>
    /// Implements all Quik callback functions to be processed on .NET side.
    /// These functions are called by Quik inside QLUA.
    /// 
    /// ������� ��������� ������
    /// ������� ���������� ��� ��������� ��������� ������ ��� ������� ���������� QUIK �� �������: 
    /// main - ���������� ��������� ������ ���������� � ������� 
    /// OnAccountBalance - ��������� ������� �� ����� 
    /// OnAccountPosition - ��������� ������� �� ����� 
    /// OnAllTrade - ����� ������������ ������ 
    /// OnCleanUp - ����� �������� ������ � ��� �������� ����� qlua.dll 
    /// OnClose - �������� ��������� QUIK 
    /// OnConnected - ������������ ����� � �������� QUIK 
    /// OnDepoLimit - ��������� ��������� ������ 
    /// OnDepoLimitDelete - �������� ��������� ������ 
    /// OnDisconnected - ���������� �� ������� QUIK 
    /// OnFirm - �������� ����� ����� 
    /// OnFuturesClientHolding - ��������� ������� �� �������� ����� 
    /// OnFuturesLimitChange - ��������� ����������� �� �������� ����� 
    /// OnFuturesLimitDelete - �������� ������ �� �������� ����� 
    /// OnInit - ������������� ������� main 
    /// OnMoneyLimit - ��������� ��������� ������ 
    /// OnMoneyLimitDelete - �������� ��������� ������ 
    /// OnNegDeal - ����� ������ �� ����������� ������ 
    /// OnNegTrade - ����� ������ ��� ���������� 
    /// OnOrder - ����� ������ ��� ��������� ���������� ������������ ������ 
    /// OnParam - ��������� ������� ���������� 
    /// OnQuote - ��������� ������� ��������� 
    /// OnStop - ��������� ������� �� ������� ���������� 
    /// OnStopOrder - ����� ����-������ ��� ��������� ���������� ������������ ����-������ 
    /// OnTrade - ����� ������ 
    /// OnTransReply - ����� �� ���������� 
    /// </summary>
    public interface IQuikEvents : IQuikService {
        event AccountBalanceHandler OnAccountBalance;
        event AccountPositionHandler OnAccountPosition;
        /// <summary>
        /// ����� ������������ ������
        /// </summary>
        event AllTradeHandler OnAllTrade;
        event VoidHandler OnCleanUp;
        /// <summary>
        /// ������� ���������� ����� ��������� ��������� QUIK. 
        /// </summary>
        event VoidHandler OnClose;
        event VoidHandler OnConnected;
        event DepoLimitHendler OnDepoLimit;
        event DepoLimitHendler OnDepoLimitDelete;
        event VoidHandler OnDisconnected;
        event EventHandler OnFirm;
        event EventHandler OnFuturesClientHolding;
        event EventHandler OnFuturesLimitChange;
        event EventHandler OnFuturesLimitDelete;
        /// <summary>
        /// ������� ���������� ���������� QUIK ����� ������� ������� main(). � �������� ��������� ��������� �������� ������� ���� � ������������ �������. 
        /// ����������: � ������ ������� ������������ ����� ����������� ���������������� ��� ����������� ���������� � ���������� ����� �������� ��������� ������ main()
        /// </summary>
        event InitHandler OnInit;
        event EventHandler OnMoneyLimit;
        event EventHandler OnMoneyLimitDelete;
        event EventHandler OnNegDeal;
        event EventHandler OnNegTrade;
        event OrderHandler OnOrder;
        event ParamHandler OnParam;
        event QuoteHandler OnQuote;
        /// <summary>
        /// ������� ���������� ���������� QUIK ��� ��������� ������� �� ������� ����������. 
        /// ����������: �������� ��������� �stop_flag� � �1�.����� ��������� ���������� ������� ������� ���������� ������ ������� 5 ������. �� ��������� ����� ��������� ������� main() ����������� �������������. ��� ���� �������� ������ ��������� ��������.
        /// </summary>
        event StopHandler OnStop;
        event TradeHandler OnTrade;
        event TransReplyHandler OnTransReply;

    }
}
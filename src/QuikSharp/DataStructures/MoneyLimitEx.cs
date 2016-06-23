// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// ������ �� �������� ���������
    /// </summary>
    public class MoneyLimitEx {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// �������� ����� �� �������� ���������
        /// </summary>
        public double money_open_limit { get; set; }
        
        /// <summary>
        /// ��������� �������������� ����� � ������� �� �������
        /// </summary>
        public double money_limit_locked_nonmarginal_value { get; set; }
        /// <summary>
        /// ��������������� � ������� �� ������� ���������� �������� �������
        /// </summary>
        public double money_limit_locked { get; set; }
        /// <summary>
        /// �������� ������� �� �������� ���������
        /// </summary>
        public double money_open_balance { get; set; }
        /// <summary>
        /// ������� ����� �� �������� ���������
        /// </summary>
        public double money_current_limit { get; set; }
        /// <summary>
        /// ������� ������� �� �������� ���������
        /// </summary>
        public double money_current_balance { get; set; }
        /// <summary>
        /// ��������� ���������� �������� �������
        /// </summary>
        public double money_limit_available { get; set; }

        /// <summary>
        /// ��� ������
        /// </summary>
        public string currcode { get; set; }
        /// <summary>
        /// ��� ��������
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// ������������� �����
        /// </summary>
        public string firmid { get; set; }
        /// <summary>
        /// ��� �������
        /// </summary>
        public string client_code { get; set; }
        /// <summary>
        /// �������� ������� �� �������
        /// </summary>
        public double openbal { get; set; }
        /// <summary>
        /// �������� ����� �� �������
        /// </summary>
        public double openlimit { get; set; }
        /// <summary>
        /// ������� ������� �� �������
        /// </summary>
        public double currentbal { get; set; }
        /// <summary>
        /// ������� ����� �� �������
        /// </summary>
        public double currentlimit { get; set; }
        /// <summary>
        /// ��������������� ����������
        /// </summary>
        public double locked { get; set; }
        /// <summary>
        /// ��������� ������� � ������� �� ������� �������������� �����
        /// </summary>
        public double locked_value_coef { get; set; }
        /// <summary>
        /// ��������� ������� � ������� �� ������� ������������ �����
        /// </summary>
        public double locked_margin_value { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public double leverage { get; set; }
        /// <summary>
        /// ��� ������. ��������� ��������:
        /// �0� � ������� ������,
        /// ����� � ��������������� ������
        /// </summary>
        public double limit_kind { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
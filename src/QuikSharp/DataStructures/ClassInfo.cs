// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// �������� ������
    /// </summary>
    public class ClassInfo {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// ��� �����
        /// </summary>
        public string firmid { get; set; }
        
        /// <summary>
        /// ������������ ������
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// ��� ������
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// ���������� ���������� � ������
        /// </summary>
        public int npars { get; set; }
        /// <summary>
        /// ���������� ����� � ������
        /// </summary>
        public int nsecs { get; set; }
        // ReSharper restore InconsistentNaming


        public override string ToString() {
            return this.ToJson();
        }
    }
}
using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Request
{
    /// <summary>
    /// ����λ����Ϣ
    /// </summary>
    public class LocationRequest : AbstractRequest
    {
        /// <summary>
        /// ����λ��ά��
        /// </summary>
        public float Location_X { get; set; }

        /// <summary>
        /// ����λ�þ���
        /// </summary>
        public float Location_Y { get; set; }

        /// <summary>
        /// ��ͼ���Ŵ�С
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// ����λ����Ϣ
        /// </summary>
        public string Label { get; set; }
    }
}

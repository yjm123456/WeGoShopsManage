using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tools.WebEnums
{
    /// <summary>
    /// 支付方式枚举
    /// </summary>
    public  class PayMentEnums
    {
        public enum PayWayEnum
        {
            /// <summary>
            /// 货到付款
            /// </summary>
            [Description("货到付款")]
            podrequest = 0,
            /// <summary>
            /// 线下付款
            /// </summary>
            [Description("线下付款")]
            offlinerequest = 99,
            /// <summary>
            /// 余额支付
            /// </summary>
            [Description("余额支付")]
            balancepayrequest = 66,
            /// <summary>
            /// 积分抵现
            /// </summary>
            [Description("积分抵现")]
            pointtocash = 77,
            /// <summary>
            /// 微信支付
            /// </summary>
            [Description("微信支付")]
            weixinrequest = 88,
        }
      
    }
}

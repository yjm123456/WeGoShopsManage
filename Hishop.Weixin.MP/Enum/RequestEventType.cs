﻿using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP
{
    /// <summary>
    /// 请求事件类型
    /// </summary>
    public enum RequestEventType
    {
        /// <summary>
        /// 关注
        /// </summary>
        Subscribe,
        /// <summary>
        /// 取消关注
        /// </summary>
        UnSubscribe,
        /// <summary>
        /// 扫描带参数二维码
        /// </summary>
        Scan,
        /// <summary>
        /// 上报地理位置
        /// </summary>
        Location,
        /// <summary>
        /// 自定义菜单
        /// </summary>
        Click,
        /// <summary>
        /// 链接按钮
        /// </summary>
        VIEW,
        /// <summary>
        /// 事件推送群发
        /// </summary>
        MASSSENDJOBFINISH,
        /// <summary>
        /// 模板消息推送
        /// </summary>
        TEMPLATESENDJOBFINISH
    }
}

﻿using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Request.Event
{
    public class UnSubscribeEventRequest : EventRequest
    {
        public override RequestEventType Event
        {
            get {
                return RequestEventType.UnSubscribe;
            }
            set { }
        }
    }
}

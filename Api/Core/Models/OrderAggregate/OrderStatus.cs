﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember (Value = "Pending")]
        Pending,

        [EnumMember(Value = " Accepted")]
        Accepted,

        [EnumMember(Value = "Refused")]
        Failed
    }
}

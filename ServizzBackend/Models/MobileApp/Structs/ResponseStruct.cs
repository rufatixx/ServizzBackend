﻿using System;
using System.Collections.Generic;

namespace WoozzyBackend.Models.MobileApp.Structs
{
    public class ResponseStruct<T>
    {
        public string requestToken { get; set; }
        public int status { get; set; }
        public List<T> data { get; set; }
    }
}

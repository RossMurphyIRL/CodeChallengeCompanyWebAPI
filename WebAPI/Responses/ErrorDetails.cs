﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Responses
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

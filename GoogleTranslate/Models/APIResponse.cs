using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoogleTranslate.Models
{
    public class APIResponse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
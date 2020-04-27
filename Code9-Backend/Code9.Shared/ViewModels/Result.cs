using System;
using System.Collections.Generic;
using System.Text;

namespace Code9.Shared.ViewModels
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Creating QRMessageObject for deserializing.
/// </summary>
namespace TransPerfectWebCommon.Models
{
    public class QRMessageObject
    {
        public string type { get; set; }
        public Symbol[] symbol { get; set; }
    }

    public class Symbol
    {
        public int seq { get; set; }
        public string data { get; set; }
        public string error { get; set; }
    }
}

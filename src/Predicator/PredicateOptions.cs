using System;

namespace Predicator
{
    public class PredicateOptions
    {
        public string StringValue1 { get; set; }
        public string StringValue2 { get; set; }
        public string[] StringArrayValue1 { get; set; }
        public string[] StringArrayValue2 { get; set; }
        public decimal? NumberValue1 { get; set; }
        public decimal? NumberValue2 { get; set; }
        public DateTime? DateValue1 { get; set; }
        public DateTime? DateValue2 { get; set; }
    }
}

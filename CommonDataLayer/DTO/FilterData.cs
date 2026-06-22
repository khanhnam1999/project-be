using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDataLayer.DTO
{
    public class FilterData
    {
        public List<FilterKeyValue>? Conditions { get; set; }
        public string? SortName { get; set; } = "ModifiedDate";
        public string? SortMethod { get; set; } = "DESC";
        public int Page { get; set; } = 0;
        public int Limit { get; set; } = 20;
    }

    public class FilterKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Core.Enums;

namespace Spirebyte.Services.Projects.Core.Entities
{
    public class Grant
    {
        public GrantTypes Type { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }

        public Grant(GrantTypes type, string value, string displayValue)
        {
            Type = type;
            Value = value;
            DisplayValue = displayValue;
        }
    }
}

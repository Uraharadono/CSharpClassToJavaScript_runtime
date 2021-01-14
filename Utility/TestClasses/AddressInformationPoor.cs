using System;
using System.Collections.Generic;

namespace Utility.TestClasses
{
    public class AddressInformationPoor
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }
        public DateTime SomeTimeAgo { get; set; }
        public OwnerInformation Owner { get; set; }
        public List<Feature> Features { get; set; }
        public List<string> Tags { get; set; }
    }
}

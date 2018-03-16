using System.Collections.Generic;

namespace Utility.TestClasses
{
    public class AddressInformation
    {
        //public Dictionary<int,string> MojDictionary { get; set; }
        //public ETestEnum EnumVar { get; set; }
        //public string Name { get; set; }
        //public string Address { get; set; }
        //public int ZipCode { get; set; }
        public OwnerInformation Owner { get; set; }
        public List<Feature> Features { get; set; }
        public List<string> Tags { get; set; }
    }

    public class OwnerInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class Feature
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public enum ETestEnum
    {
        Option1 = 0,
        Option2 = 1,
        Option3 = 2
    }

}

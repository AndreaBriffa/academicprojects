using System.Collections.Generic;

namespace _178390_AndreaBriffa_DDOOCP
{
    public class Language
    {
        public string name { get; set; } // The name of the Language
        public List<string> words { get; set; } = new List<string>(); // List of words pertinent to the Language

        // Primary Constructor
        public Language() { }

        // Secondary Constructor
        public Language(string name)
        {
            this.name = name;
        }
    }
}
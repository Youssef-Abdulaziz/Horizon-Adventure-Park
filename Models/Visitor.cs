using Horizon_Adventure_Park.Enums;
using System.Xml.Linq;

namespace Horizon_Adventure_Park.Models
{
    public class Visitor
    {
        public string VisitorId { get; set; }
        public string VisitorName { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public VisitorType Category { get; set; }
        public bool HasAccompanyingAdult { get; set; }

        public Visitor(string visitorId, string visitorName, int age, double height, VisitorType category, bool hasAccompanyinAdult)
        {
            VisitorId = visitorId;
            VisitorName = visitorName;
            Age = age;
            Height = height;
            Category = category;
            HasAccompanyingAdult = hasAccompanyinAdult;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Visitor ID: {VisitorId}");
            Console.WriteLine($"Name: {VisitorName}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Height: {Height} cm");
            Console.WriteLine($"Category: {Category}");
            Console.WriteLine($"Accompanying Adult: {HasAccompanyingAdult}");
        }
    }
}

using System.Collections.Generic;

namespace Webhallen.Models
{
    public class CrateType
    {
        public string name { get; set; }
        public string icon { get; set; }
        public double progress { get; set; }
        public int openableCount { get; set; }
        public int? nextResupplyIn { get; set; }
    }

    public class Drop
    {
        public Item item { get; set; }
        public int count { get; set; }
        public override string ToString() => $"({item}) x{count}";
    }

    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public string iconName { get; set; }
        public string description { get; set; }
        public override string ToString() => $"Id={id}, Name={name}, Description={description}";
    }

    public class SupplyDropResponse
    {
        public int airplanePosition { get; set; }
        public int nextDropTime { get; set; }
        public List<Drop> drops { get; set; }
        public List<CrateType> crateTypes { get; set; }
    }
}

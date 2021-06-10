namespace Gather.Models
{
    public class ResourceDetail
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public int GatherRate { get; set; }

        public int Workers { get; set; }

        public int Tools { get; set; }

        public int Foremen { get; set; }

        public bool Automated { get; set; }

        public bool CanAutomate { get; set; }

        public bool Enabled { get; set; }

        public bool CanEnable { get; set; }

        public bool CanAddWorker { get; set; }

        public bool CanAddTool { get; set; }

        public bool CanAddForeman { get; set; }

        public string ShortDescription => $"ID: {ID} - {Name}";
    }
}

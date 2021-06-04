using System;

namespace Gather.Models
{
    public class ResourceDetail
    {
        public long id { get; set; }

        public string name { get; set; }

        public string ShortDescription => $"ID: {id} - {name}";
    }
}

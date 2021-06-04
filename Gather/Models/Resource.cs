using System;
using System.Collections.Generic;

namespace Gather.Models
{
    public class Resource
    {
        public long id { get; set; }

        public string name { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }


        public ICollection<ResourceDetail> Details { get; set; }

        public override string ToString()
        {
            return $"{id} {name}";
        }

        public string ShortDescription => $"ID: {id}";
    }
}

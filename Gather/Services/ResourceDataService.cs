using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Gather.Models;

namespace Gather.Services
{
    public static class ResourceDataService
    {
        private static IEnumerable<Resource> _allResources;

        private static IEnumerable<Resource> AllResources()
        {
            return new List<Resource>()
            {
                new Resource()
                {
                    id = 1,
                    name = "Stone",
                },
                new Resource()
                {
                    id = 2,
                    name = "Water",
                },
                new Resource()
                {
                    id = 3,
                    name = "Iron",
                }
            };
        }

        public static async Task<IEnumerable<Resource>> GetContentGridDataAsync()
        {
            if (_allResources == null)
            {
                _allResources = AllResources();
            }

            await Task.CompletedTask;
            return _allResources;
        }
    }
}

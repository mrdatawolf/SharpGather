using System;
using System.Collections;
using System.Collections.Generic;

namespace Gather.Models
{
    public class Resources : IEnumerable
    {
        private Resource[] _resources;
        public Resources(Resource[] rArray)
        {
            _resources = new Resource[rArray.Length];

            for (int i = 0; i < rArray.Length; i++)
            {
                _resources[i] = rArray[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ResourcesEnum GetEnumerator()
        {
            return new ResourcesEnum(_resources);
        }

        public class ResourcesEnum : IEnumerator
        {
            public Resource[] _resources;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public ResourcesEnum(Resource[] list)
            {
                _resources = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _resources.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public Resource Current
            {
                get
                {
                    try
                    {
                        return _resources[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}

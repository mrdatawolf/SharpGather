using System;
using System.Collections;
using System.Collections.Generic;

namespace HttpUtils
{
   public class InitialGather
    {
        public int ID { get; set; }
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

        public override string ToString()
        {
            return string.Format("Initial Gather data:\n\tId: {0}, Name: {1}, Amount: {2}, GatherRate: {3}, Workers: {4}, Tools: {5}, Formen: {6}, Automated: {7}, CanAutomate: {8}, Enabled: {9}, " +
                "CanEnable: {10}, CanAddWorker: {11}, CanAddTool: {12}, CanAddForeman: {13}", ID, Name, Amount, GatherRate, Workers, Tools, Foremen, Automated, CanAutomate, Enabled, CanEnable, CanAddWorker, CanAddTool, CanAddForeman);
        }
    }

    public class InitialGathers : IEnumerable
    {
        private InitialGather[] _initialGathers;
        public InitialGathers(InitialGather[] iArray)
        {
            _initialGathers = new InitialGather[iArray.Length];

            for (int i = 0; i < iArray.Length; i++)
            {
                _initialGathers[i] = iArray[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public InitialGathersEnum GetEnumerator()
        {
            return new InitialGathersEnum(_initialGathers);
        }

        public class InitialGathersEnum : IEnumerator
        {
            public InitialGather[] _initialGathers;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public InitialGathersEnum(InitialGather[] list)
            {
                _initialGathers = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _initialGathers.Length);
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

            public InitialGather Current
            {
                get
                {
                    try
                    {
                        return _initialGathers[position];
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

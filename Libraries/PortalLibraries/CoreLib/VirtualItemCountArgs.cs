using System;

namespace Core
{
    [Serializable]
    public class VirtualItemCountArgs
    {
        public bool IsLastPage { get; set; }
        public int InitialSelectedPageSize { get; set; }
        public int CurrentPageSizeForLastPage { get; set; }

        public VirtualItemCountArgs(int initialSelectedPageSize)
        {
            InitialSelectedPageSize = initialSelectedPageSize;
        }
    }
}

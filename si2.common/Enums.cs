using System;
using System.Collections.Generic;
using System.Text;

namespace si2.common
{
    public class Enums
    {
        public enum ResourceUriType
        {
            PreviousPage,
            NextPage
        }
        public enum DataflowStatus
        {
            Started = 1,
            Ongoing = 2,
            Completed = 3
        }

        public enum SimplecohortStatus
        {
            Started = 1,
            Ongoing = 2,
            Completed = 3
        }
    }
}

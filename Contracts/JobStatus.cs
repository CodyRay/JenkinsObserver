using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contracts
{
    public enum JobStatus
    {
        Unknown,
        Aborted,
        Success,
        Disabled,
        Pending,
        NotBuilt,
        Failed,
        Unstable, 
    }
}

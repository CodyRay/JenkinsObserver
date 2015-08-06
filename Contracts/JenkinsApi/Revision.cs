using System.Collections.Generic;

namespace Contracts.JenkinsApi
{
    public class Revision
    {
        public string Sha1 { get; set; }
        public List<Branch> Branch { get; set; }
    }
}
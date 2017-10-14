using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Data
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServerResponse { get; set; }
        public TimeSpan ResponseTime { get; set; }
        public List<string> H1Headers { get; set; }
        public List<string> Images { get; set; }
        public List<string> Links { get; set; }
    }
}

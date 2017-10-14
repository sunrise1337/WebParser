using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    interface IParser<T> where T : class
    {
        T Parse(string url);
    }
}

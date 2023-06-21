using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service.Common
{
    public class Utility1
    {
        public string GetTextEnum(int Value)
        {
            string stringValue = Enum.GetName(typeof(CategoryType), Value);
            return stringValue;
        }

    }
}

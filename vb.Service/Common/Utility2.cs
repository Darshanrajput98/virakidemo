using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vb.Data;

namespace vb.Service
{
    public class Utility2
    {

        public string GetTextEnum(int Value)
        {
            string stringValue = Enum.GetName(typeof(CustomerType), Value);
            return stringValue;

        }


    }
}


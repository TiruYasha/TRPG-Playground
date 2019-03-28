using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class NormalRollCommandResultModel : CommandResultModel
    {
        public int Result { get; set; }

        public NormalRollCommandResultModel()
        {
            Type = 2;
        }
    }
}

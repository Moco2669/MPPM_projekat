using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RotatingMachine : RegulatingCondEq
    {
        public RotatingMachine(long globalId) : base(globalId)
        {

        }

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}

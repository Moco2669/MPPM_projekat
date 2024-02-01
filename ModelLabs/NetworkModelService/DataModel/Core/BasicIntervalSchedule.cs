using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class BasicIntervalSchedule : IdentifiedObject
    {
        private DateTime startTime = DateTime.MinValue;
        private UnitMultiplier value1Multiplier;
        private UnitSymbol value1Unit;
        private UnitMultiplier value2Multiplier;
        private UnitSymbol value2Unit;
        public BasicIntervalSchedule(long globalId) : base(globalId)
        {
        }

        public DateTime StartTime { get => startTime; set => startTime = value; }
        public UnitMultiplier Value1Multiplier { get => value1Multiplier; set => value1Multiplier = value; }
        public UnitSymbol Value1Unit { get => value1Unit; set => value1Unit = value; }
        public UnitMultiplier Value2Multiplier { get => value2Multiplier; set => value2Multiplier = value; }
        public UnitSymbol Value2Unit { get => value2Unit; set => value2Unit = value; }

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				BasicIntervalSchedule x = (BasicIntervalSchedule)obj;
				return (this.startTime == x.startTime && this.value1Multiplier == x.value1Multiplier && this.value1Unit == x.value1Unit && this.value2Multiplier == x.value2Multiplier
					&& this.value2Unit == x.value2Unit);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation

		public override bool HasProperty(ModelCode t)
		{
			switch (t)
			{
				case ModelCode.BASICINTERVALSCHEDULE_STARTTIME:
				case ModelCode.BASICINTERVALSCHEDULE_V1MP:
				case ModelCode.BASICINTERVALSCHEDULE_V1U:
				case ModelCode.BASICINTERVALSCHEDULE_V2MP:
				case ModelCode.BASICINTERVALSCHEDULE_V2U:
					return true;
				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.BASICINTERVALSCHEDULE_STARTTIME:
					property.SetValue(startTime);
					break;

				case ModelCode.BASICINTERVALSCHEDULE_V1MP:
					property.SetValue((short)value1Multiplier);
					break;
				
				case ModelCode.BASICINTERVALSCHEDULE_V1U:
					property.SetValue((short)value1Unit);
					break;
				
				case ModelCode.BASICINTERVALSCHEDULE_V2MP:
					property.SetValue((short)value2Multiplier);
					break;
				
				case ModelCode.BASICINTERVALSCHEDULE_V2U:
					property.SetValue((short)value2Unit);
					break;

				default:
					base.GetProperty(property);
					break;
			}
		}

		public override void SetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.BASICINTERVALSCHEDULE_STARTTIME:
					startTime = property.AsDateTime();
					break;
				
				case ModelCode.BASICINTERVALSCHEDULE_V1MP:
					value1Multiplier = (UnitMultiplier) property.AsEnum();
					break;
				
				case ModelCode.BASICINTERVALSCHEDULE_V1U:
					value1Unit = (UnitSymbol) property.AsEnum();
					break;

				case ModelCode.BASICINTERVALSCHEDULE_V2MP:
					value2Multiplier = (UnitMultiplier) property.AsEnum();
					break;

				case ModelCode.BASICINTERVALSCHEDULE_V2U:
					value2Unit = (UnitSymbol) property.AsEnum();
					break;

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation
	}
}

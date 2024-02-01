using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class RegulatingControl : Core.PowerSystemResource
    {
        private bool discrete;
        private RegulatingControlModeKind mode;
        private PhaseCode monitoredPhase;
        private float targetRange;
        private float targetValue;
        private long terminal = 0;
        private List<long> regulatingCondEqs = new List<long>();
        private List<long> regulationSchedules = new List<long>();

        public RegulatingControl(long globalId) : base(globalId) { }

        public bool Discrete { get => discrete; set => discrete = value; }
        public RegulatingControlModeKind Mode { get => mode; set => mode = value; }
        public PhaseCode MonitoredPhase { get => monitoredPhase; set => monitoredPhase = value; }
        public float TargetRange { get => targetRange; set => targetRange = value; }
        public float TargetValue { get => targetValue; set => targetValue = value; }
        public long Terminal { get => terminal; set => terminal = value; }
        public List<long> RegulatingCondEqs { get => regulatingCondEqs; set => regulatingCondEqs = value; }
        public List<long> RegulationSchedules { get => regulationSchedules; set => regulationSchedules = value; }

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				RegulatingControl x = (RegulatingControl)obj;
				return (this.discrete == x.discrete
					&& this.mode == x.mode
					&& this.monitoredPhase == x.monitoredPhase
					&& this.targetRange == x.targetRange
					&& this.targetValue == x.targetValue
					&& this.terminal == x.terminal
					&& CompareHelper.CompareLists(this.regulatingCondEqs, x.regulatingCondEqs)
					&& CompareHelper.CompareLists(this.regulationSchedules, x.regulationSchedules));
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
				case ModelCode.REGCONTROL_DISCRETE:
				case ModelCode.REGCONTROL_MODE:
				case ModelCode.REGCONTROL_MONITOREDPHASE:
				case ModelCode.REGCONTROL_TARGETRANGE:
				case ModelCode.REGCONTROL_TARGETVALUE:
				case ModelCode.REGCONTROL_TERMINAL:
				case ModelCode.REGCONTROL_REGCONDEQS:
				case ModelCode.REGCONTROL_REGULATIONSCHEDULES:
					return true;
				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.REGCONTROL_DISCRETE:
					property.SetValue(discrete);
					break;
				case ModelCode.REGCONTROL_MODE:
					property.SetValue((short)mode);
					break;
				case ModelCode.REGCONTROL_MONITOREDPHASE:
					property.SetValue((short)monitoredPhase);
					break;
				case ModelCode.REGCONTROL_TARGETRANGE:
					property.SetValue(targetRange);
					break;
				case ModelCode.REGCONTROL_TARGETVALUE:
					property.SetValue(targetValue);
					break;
				case ModelCode.REGCONTROL_TERMINAL:
					property.SetValue(terminal);
					break;
				case ModelCode.REGCONTROL_REGCONDEQS:
					property.SetValue(regulatingCondEqs);
					break;
				case ModelCode.REGCONTROL_REGULATIONSCHEDULES:
					property.SetValue(regulationSchedules);
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
				case ModelCode.REGCONTROL_DISCRETE:
					discrete = property.AsBool();
					break;
				case ModelCode.REGCONTROL_MODE:
					mode = (RegulatingControlModeKind)property.AsEnum();
					break;
				case ModelCode.REGCONTROL_MONITOREDPHASE:
					monitoredPhase = (PhaseCode)property.AsEnum();
					break;
				case ModelCode.REGCONTROL_TARGETRANGE:
					targetRange = property.AsFloat();
					break;
				case ModelCode.REGCONTROL_TARGETVALUE:
					targetValue = property.AsFloat();
					break;
				case ModelCode.REGCONTROL_TERMINAL:
					terminal = property.AsReference();
					break;

				default:
					base.SetProperty(property);
					break;
			}
		}

		#endregion IAccess implementation

		#region IReference implementation	

		public override bool IsReferenced
		{
			get
			{
				return regulatingCondEqs.Count > 0 || regulationSchedules.Count > 0 || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType) //OBAVEZNO SVUDA PROVERI REFTYPE
		{
			if (terminal != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.REGCONTROL_TERMINAL] = new List<long>();
				references[ModelCode.REGCONTROL_TERMINAL].Add(terminal);
			}

			if (regulatingCondEqs != null && regulatingCondEqs.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.REGCONTROL_TERMINAL] = regulatingCondEqs.GetRange(0, regulatingCondEqs.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.REGULATINGCONDEQ_REGCONTROL:
					regulatingCondEqs.Add(globalId);
					break;
				case ModelCode.REGULATIONSCHEDULE_REGCONTROL:
					regulationSchedules.Add(globalId);
					break;

				default:
					base.AddReference(referenceId, globalId);
					break;
			}
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.REGULATINGCONDEQ_REGCONTROL:

					if (regulatingCondEqs.Contains(globalId))
					{
						regulatingCondEqs.Remove(globalId);
					}
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;

				case ModelCode.REGULATIONSCHEDULE_REGCONTROL:

                    if (regulationSchedules.Contains(globalId))
                    {
						regulationSchedules.Remove(globalId);
                    }
					else
					{
						CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
					}

					break;

				default:
					base.RemoveReference(referenceId, globalId);
					break;
			}
		}

		#endregion IReference implementation
	}
}

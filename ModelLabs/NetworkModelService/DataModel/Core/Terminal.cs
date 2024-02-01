using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Terminal : IdentifiedObject
    {
        private long conductingEquipment = 0;
		private List<long> regulatingControls = new List<long>();

        public Terminal(long globalId) : base(globalId)
        {
        }

        public long ConductingEquipment { get => conductingEquipment; set => conductingEquipment = value; }
        public List<long> RegulatingControls { get => regulatingControls; set => regulatingControls = value; }

        public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Terminal x = (Terminal)obj;
				return (this.conductingEquipment == x.conductingEquipment && CompareHelper.CompareLists(this.regulatingControls, x.regulatingControls));
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
				case ModelCode.TERMINAL_CONDEQ:
				case ModelCode.TERMINAL_REGCONTROLS:
					return true;
				default:
					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property property)
		{
			switch (property.Id)
			{
				case ModelCode.TERMINAL_CONDEQ:
					property.SetValue(conductingEquipment);
					break;

				case ModelCode.TERMINAL_REGCONTROLS:
					property.SetValue(regulatingControls);
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
				case ModelCode.TERMINAL_CONDEQ:
					conductingEquipment = property.AsReference();
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
				return regulatingControls.Count > 0 || base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType) //OBAVEZNO SVUDA PROVERI REFTYPE
		{
			if (conductingEquipment != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
			{
				references[ModelCode.TERMINAL_CONDEQ] = new List<long>();
				references[ModelCode.TERMINAL_CONDEQ].Add(conductingEquipment);
			}

			if (regulatingControls != null && regulatingControls.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
			{
				references[ModelCode.TERMINAL_REGCONTROLS] = regulatingControls.GetRange(0, regulatingControls.Count);
			}

			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			switch (referenceId)
			{
				case ModelCode.REGCONTROL_TERMINAL:
					regulatingControls.Add(globalId);
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
				case ModelCode.REGCONTROL_TERMINAL:

					if (regulatingControls.Contains(globalId))
					{
						regulatingControls.Remove(globalId);
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

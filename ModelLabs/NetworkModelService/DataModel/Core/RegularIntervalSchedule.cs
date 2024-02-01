using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularIntervalSchedule : BasicIntervalSchedule
    {
        public RegularIntervalSchedule(long globalId) : base(globalId) { }

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#region IAccess implementation

		public override bool HasProperty(ModelCode t)
		{
			return base.HasProperty(t);
		}

		public override void GetProperty(Property property)
		{
			base.GetProperty(property);
		}

		public override void SetProperty(Property property)
		{
			base.SetProperty(property);
		}

		#endregion IAccess implementation

		#region IReference implementation	

		public override bool IsReferenced
		{
			get
			{
				return base.IsReferenced;
			}
		}

		public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType) //OBAVEZNO SVUDA PROVERI REFTYPE
		{
			base.GetReferences(references, refType);
		}

		public override void AddReference(ModelCode referenceId, long globalId)
		{
			base.AddReference(referenceId, globalId);
		}

		public override void RemoveReference(ModelCode referenceId, long globalId)
		{
			base.RemoveReference(referenceId, globalId);
		}

		#endregion IReference implementation
	}
}

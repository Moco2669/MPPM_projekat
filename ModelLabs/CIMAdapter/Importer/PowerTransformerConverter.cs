namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
				}
			}
		}

		public static void PopulateDayTypeProperties(FTN.DayType cimDayType, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimDayType != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimDayType, rd);
			}
		}

		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);
			}
		}

		public static void PopulateTerminalProperties(FTN.Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimTerminal != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimTerminal, rd);

				if (cimTerminal.ConductingEquipmentHasValue)
				{
					long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
					if(gid < 0)
                    {
						report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
						report.Report.Append("\" - Failed to set reference to ConductingEquipment: rdfID \"").Append(cimTerminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.TERMINAL_CONDEQ, cimTerminal.ConductingEquipment.ID));
				}
			}
		}

		public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);

				if (cimEquipment.AggregateHasValue)
				{
					rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
				}
				if (cimEquipment.NormallyInServiceHasValue)
				{
					rd.AddProperty(new Property(ModelCode.EQUIPMENT_NORMSERVICE, cimEquipment.NormallyInService));
				}
			}
		}

		public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimConductingEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
			}
		}

		public static void PopulateRegulatingControlProperties(FTN.RegulatingControl cimRegulatingControl, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRegulatingControl != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimRegulatingControl, rd, importHelper, report);

                if (cimRegulatingControl.DiscreteHasValue)
                {
					rd.AddProperty(new Property(ModelCode.REGCONTROL_DISCRETE, cimRegulatingControl.Discrete));
                }
				if (cimRegulatingControl.ModeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGCONTROL_MODE, (short)GetDMSRegulatingControlModeKind(cimRegulatingControl.Mode)));
				}
				if (cimRegulatingControl.MonitoredPhaseHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGCONTROL_MONITOREDPHASE, (short)GetDMSPhaseCode(cimRegulatingControl.MonitoredPhase)));
				}
				if (cimRegulatingControl.TargetRangeHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGCONTROL_TARGETRANGE, cimRegulatingControl.TargetRange));
				}
				if (cimRegulatingControl.TargetValueHasValue)
				{
					rd.AddProperty(new Property(ModelCode.REGCONTROL_TARGETVALUE, cimRegulatingControl.TargetValue));
				}
				if (cimRegulatingControl.TerminalHasValue)
				{
					long gid = importHelper.GetMappedGID(cimRegulatingControl.Terminal.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimRegulatingControl.GetType().ToString()).Append(" rdfID = \"").Append(cimRegulatingControl.ID);
						report.Report.Append("\" - Failed to set reference to Terminal: rdfID \"").Append(cimRegulatingControl.Terminal.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.REGCONTROL_TERMINAL, gid));
				}
			}
		}

		public static void PopulateRegulationScheduleProperties(FTN.RegulationSchedule cimRegulationSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimRegulationSchedule != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateSeasonDayTypeScheduleProperties(cimRegulationSchedule, rd, importHelper, report);

				if (cimRegulationSchedule.RegulatingControlHasValue)
				{
					long gid = importHelper.GetMappedGID(cimRegulationSchedule.RegulatingControl.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimRegulationSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimRegulationSchedule.ID);
						report.Report.Append("\" - Failed to set reference to RegulatingControl: rdfID \"").Append(cimRegulationSchedule.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.REGULATIONSCHEDULE_REGCONTROL, gid));
				}
			}
		}

		public static void PopulateSeasonDayTypeScheduleProperties(FTN.SeasonDayTypeSchedule cimSeasonDayTypeSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
			if((cimSeasonDayTypeSchedule != null) && (rd != null))
            {
				PowerTransformerConverter.PopulateRegularIntervalScheduleProperties(cimSeasonDayTypeSchedule, rd, importHelper, report);

                if (cimSeasonDayTypeSchedule.SeasonHasValue)
                {
					long gid = importHelper.GetMappedGID(cimSeasonDayTypeSchedule.Season.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimSeasonDayTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDayTypeSchedule.ID);
						report.Report.Append("\" - Failed to set reference to Season: rdfID \"").Append(cimSeasonDayTypeSchedule.Season.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_SEASON, gid));
                }
				if (cimSeasonDayTypeSchedule.DayTypeHasValue)
				{
					long gid = importHelper.GetMappedGID(cimSeasonDayTypeSchedule.DayType.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimSeasonDayTypeSchedule.GetType().ToString()).Append(" rdfID = \"").Append(cimSeasonDayTypeSchedule.ID);
						report.Report.Append("\" - Failed to set reference to DayType: rdfID \"").Append(cimSeasonDayTypeSchedule.DayType.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.SEASONDAYTYPESCHEDULE_DAYTYPE, gid));
				}
			}
        }

		public static void PopulateRegularIntervalScheduleProperties(FTN.RegularIntervalSchedule cimRegularIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegularIntervalSchedule != null) && (rd != null))
            {
				PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(cimRegularIntervalSchedule, rd, importHelper, report);
            }
        }

		public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cimBasicIntervalSchedule, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
			if((cimBasicIntervalSchedule != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimBasicIntervalSchedule, rd);

                if (cimBasicIntervalSchedule.StartTimeHasValue)
                {
					rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_STARTTIME, cimBasicIntervalSchedule.StartTime));
                }
				if (cimBasicIntervalSchedule.Value1MultiplierHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_V1MP, (short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value1Multiplier)));
				}
				if (cimBasicIntervalSchedule.Value1UnitHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_V1U, (short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value1Unit)));
				}
				if (cimBasicIntervalSchedule.Value2MultiplierHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_V2MP, (short)GetDMSUnitMultiplier(cimBasicIntervalSchedule.Value2Multiplier)));
				}
				if (cimBasicIntervalSchedule.Value2UnitHasValue)
				{
					rd.AddProperty(new Property(ModelCode.BASICINTERVALSCHEDULE_V2U, (short)GetDMSUnitSymbol(cimBasicIntervalSchedule.Value2Unit)));
				}
			}
        }

		public static void PopulateSynchronousMachineProperties(FTN.SynchronousMachine cimSynchronousMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
		{
			if ((cimSynchronousMachine != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateRotatingMachineProperties(cimSynchronousMachine, rd, importHelper, report);
			}
		}

		public static void PopulateRotatingMachineProperties(FTN.RotatingMachine cimRotatingMachine, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRotatingMachine != null) && (rd != null))
            {
				PowerTransformerConverter.PopulateRegulatingCondEqProperties(cimRotatingMachine, rd, importHelper, report);
            }
        }

		public static void PopulateRegulatingCondEqProperties(FTN.RegulatingCondEq cimRegulatingCondEq, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimRegulatingCondEq != null) && (rd != null))
            {
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimRegulatingCondEq, rd, importHelper, report);

                if (cimRegulatingCondEq.RegulatingControlHasValue)
                {
					long gid = importHelper.GetMappedGID(cimRegulatingCondEq.RegulatingControl.ID);
					if (gid < 0)
					{
						report.Report.Append("WARNING: Convert ").Append(cimRegulatingCondEq.GetType().ToString()).Append(" rdfID = \"").Append(cimRegulatingCondEq.ID);
						report.Report.Append("\" - Failed to set reference to RegulatingControl: rdfID \"").Append(cimRegulatingCondEq.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
					}
					rd.AddProperty(new Property(ModelCode.REGULATINGCONDEQ_REGCONTROL, gid));
				}
            }
        }

		public static void PopulateSeasonProperties(FTN.Season cimSeason, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimSeason != null) && (rd != null))
            {
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimSeason, rd);

                if (cimSeason.EndDateHasValue)
                {
					rd.AddProperty(new Property(ModelCode.SEASON_ENDDATE, cimSeason.EndDate));
                }
                if (cimSeason.StartDateHasValue)
                {
					rd.AddProperty(new Property(ModelCode.SEASON_STARTDATE, cimSeason.StartDate));
                }
            }
        }
		#endregion Populate ResourceDescription

		#region Enums convert
		public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
		{
			switch (phases)
			{
				case FTN.PhaseCode.A:
					return PhaseCode.A;
				case FTN.PhaseCode.AB:
					return PhaseCode.AB;
				case FTN.PhaseCode.ABC:
					return PhaseCode.ABC;
				case FTN.PhaseCode.ABCN:
					return PhaseCode.ABCN;
				case FTN.PhaseCode.ABN:
					return PhaseCode.ABN;
				case FTN.PhaseCode.AC:
					return PhaseCode.AC;
				case FTN.PhaseCode.ACN:
					return PhaseCode.ACN;
				case FTN.PhaseCode.AN:
					return PhaseCode.AN;
				case FTN.PhaseCode.B:
					return PhaseCode.B;
				case FTN.PhaseCode.BC:
					return PhaseCode.BC;
				case FTN.PhaseCode.BCN:
					return PhaseCode.BCN;
				case FTN.PhaseCode.BN:
					return PhaseCode.BN;
				case FTN.PhaseCode.C:
					return PhaseCode.C;
				case FTN.PhaseCode.CN:
					return PhaseCode.CN;
				case FTN.PhaseCode.N:
					return PhaseCode.N;
				case FTN.PhaseCode.s12N:
					return PhaseCode.ABN;
				case FTN.PhaseCode.s1N:
					return PhaseCode.AN;
				case FTN.PhaseCode.s2N:
					return PhaseCode.BN;
				default: return PhaseCode.Unknown;
			}
		}
		public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier unitMultiplier)
		{
			switch (unitMultiplier)
			{
				case FTN.UnitMultiplier.c:
					return UnitMultiplier.c;
				case FTN.UnitMultiplier.d:
					return UnitMultiplier.d;
				case FTN.UnitMultiplier.G:
					return UnitMultiplier.G;
				case FTN.UnitMultiplier.k:
					return UnitMultiplier.k;
				case FTN.UnitMultiplier.m:
					return UnitMultiplier.m;
				case FTN.UnitMultiplier.M:
					return UnitMultiplier.M;
				case FTN.UnitMultiplier.micro:
					return UnitMultiplier.micro;
				case FTN.UnitMultiplier.n:
					return UnitMultiplier.n;
				case FTN.UnitMultiplier.none:
					return UnitMultiplier.none;
				case FTN.UnitMultiplier.p:
					return UnitMultiplier.p;
				case FTN.UnitMultiplier.T:
					return UnitMultiplier.T;
				default:
					return UnitMultiplier.Unknown;
			}
		}

		public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol unitSymbol)
		{
			switch (unitSymbol)
			{
				case FTN.UnitSymbol.A:
					return UnitSymbol.A;
				case FTN.UnitSymbol.deg:
					return UnitSymbol.deg;
				case FTN.UnitSymbol.degC:
					return UnitSymbol.degC;
				case FTN.UnitSymbol.F:
					return UnitSymbol.F;
				case FTN.UnitSymbol.g:
					return UnitSymbol.g;
				case FTN.UnitSymbol.h:
					return UnitSymbol.h;
				case FTN.UnitSymbol.H:
					return UnitSymbol.H;
				case FTN.UnitSymbol.Hz:
					return UnitSymbol.Hz;
				case FTN.UnitSymbol.J:
					return UnitSymbol.J;
				case FTN.UnitSymbol.m:
					return UnitSymbol.m;
				case FTN.UnitSymbol.m2:
					return UnitSymbol.m2;
				case FTN.UnitSymbol.m3:
					return UnitSymbol.m3;
				case FTN.UnitSymbol.min:
					return UnitSymbol.min;
				case FTN.UnitSymbol.N:
					return UnitSymbol.N;
				case FTN.UnitSymbol.none:
					return UnitSymbol.none;
				case FTN.UnitSymbol.ohm:
					return UnitSymbol.ohm;
				case FTN.UnitSymbol.Pa:
					return UnitSymbol.Pa;
				case FTN.UnitSymbol.rad:
					return UnitSymbol.rad;
				case FTN.UnitSymbol.s:
					return UnitSymbol.s;
				case FTN.UnitSymbol.S:
					return UnitSymbol.S;
				case FTN.UnitSymbol.V:
					return UnitSymbol.V;
				case FTN.UnitSymbol.VA:
					return UnitSymbol.VA;
				case FTN.UnitSymbol.VAh:
					return UnitSymbol.VAh;
				case FTN.UnitSymbol.VAr:
					return UnitSymbol.VAr;
				case FTN.UnitSymbol.VArh:
					return UnitSymbol.VArh;
				case FTN.UnitSymbol.W:
					return UnitSymbol.W;
				case FTN.UnitSymbol.Wh:
					return UnitSymbol.Wh;
				default:
					return UnitSymbol.Unknown;
			}
		}

		public static RegulatingControlModeKind GetDMSRegulatingControlModeKind(FTN.RegulatingControlModeKind regulatingControlModeKind)
		{
			switch (regulatingControlModeKind)
			{
				case FTN.RegulatingControlModeKind.activePower:
					return RegulatingControlModeKind.ActivePower;
				case FTN.RegulatingControlModeKind.admittance:
					return RegulatingControlModeKind.Admittance;
				case FTN.RegulatingControlModeKind.currentFlow:
					return RegulatingControlModeKind.CurrentFlow;
				case FTN.RegulatingControlModeKind.@fixed:
					return RegulatingControlModeKind.Fixed;
				case FTN.RegulatingControlModeKind.powerFactor:
					return RegulatingControlModeKind.PowerFactor;
				case FTN.RegulatingControlModeKind.reactivePower:
					return RegulatingControlModeKind.ReactivePower;
				case FTN.RegulatingControlModeKind.temperature:
					return RegulatingControlModeKind.Temperature;
				case FTN.RegulatingControlModeKind.timeScheduled:
					return RegulatingControlModeKind.TimeScheduled;
				case FTN.RegulatingControlModeKind.voltage:
					return RegulatingControlModeKind.Voltage;
				default:
					return RegulatingControlModeKind.Unknown;
			}
		}
		#endregion Enums convert
	}
}

using System;

namespace FTN.Common
{	
	public enum PhaseCode : short
	{
		Unknown = 0x00,
		A = 0x01,
		AB = 0x02,
		ABC = 0x03,
		ABCN = 0x04,
		ABN = 0x05,
		AC = 0x06,
		ACN = 0x07,
		AN = 0x08,
		B = 0x09,
		BC = 0x0A,
		BCN = 0x0B,
		BN = 0x0C,
		C = 0x0D,
		CN = 0x0E,
		N = 0x0F,
		s1 = 0x10,
		s12 = 0x11,
		s12N = 0x12,
		s1N = 0x13,
		s2 = 0x14,
		s2N = 0x15
	}

	public enum RegulatingControlModeKind : short
    {
		Unknown = 0x00,
		ActivePower = 0x01,
		Admittance = 0x02,
		CurrentFlow = 0x03,
		Fixed = 0x04,
		PowerFactor = 0x05,
		ReactivePower = 0x06,
		Temperature = 0x07,
		TimeScheduled = 0x08,
		Voltage = 0x09
    }

	public enum UnitMultiplier : short
    {
		Unknown = 0x00,
		G = 0x01,
		M = 0x02,
		T = 0x03,
		c = 0x04,
		d = 0x05,
		k = 0x06,
		m = 0x07,
		micro = 0x08,
		n = 0x09,
		none = 0x0A,
		p = 0x0B
    }

	public enum UnitSymbol : short
    {
		Unknown = 0x00,
		A,
		F,
		H,
		Hz,
		J,
		N,
		Pa,
		S,
		V,
		VA,
		VAh,
		VAr,
		VArh,
		W,
		Wh,
		deg,
		degC,
		g,
		h,
		m,
		m2,
		m3,
		min,
		none,
		ohm,
		rad,
		s
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjects
{
	public enum VolumeType
	{
		Unknown = 0,
		FixedDrive = 1,
		RemovableDrive = 2,
		NetworkDrive = 3,
		FloppyDisk = 4,
		CdRom = 5,
	}
}

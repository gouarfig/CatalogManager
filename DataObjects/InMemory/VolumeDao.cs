using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.InMemory
{
	public class VolumeDao : IVolumeDao
	{
		public int CreateStructure()
		{
			return 0;
		}

		public List<BusinessObjects.Volume> GetVolumes()
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Volume GetVolumeById(int id)
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Volume GetVolumeByGuid(Guid guid)
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Volume GetVolumeByName(string name)
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Volume GetVolumeByVolumeId(string volumeId)
		{
			throw new NotImplementedException();
		}

		public bool InsertVolume(BusinessObjects.Volume volume)
		{
			throw new NotImplementedException();
		}

		public bool UpdateVolume(BusinessObjects.Volume volume)
		{
			throw new NotImplementedException();
		}

		public bool DeleteVolume(int volumeId)
		{
			throw new NotImplementedException();
		}
	}
}

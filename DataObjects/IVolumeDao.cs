using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects
{
	public interface IVolumeDao
	{
		int CreateStructure();

		/// <summary>
		/// Gets a list of volumes
		/// </summary>
		/// <returns></returns>
		List<Volume> GetVolumes();

		Volume GetVolumeById(int id);
		Volume GetVolumeByGuid(Guid guid);
		Volume GetVolumeByName(string name);
		Volume GetVolumeByVolumeId(string volumeId);
		bool InsertVolume(Volume volume);
		bool UpdateVolume(Volume volume);
		bool DeleteVolume(int volumeId);
	}
}

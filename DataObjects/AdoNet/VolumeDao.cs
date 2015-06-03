using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects.AdoNet
{
	/// <summary>
	/// Data Access Object for Volume
	/// </summary>
	public sealed class VolumeDao : IVolumeDao
	{
		private static readonly string _tableName = "Volume";
		private static readonly int _nameFieldMaxLength = 200;
		private static readonly int _volumeIdFieldMaxLength = 100;
		private static readonly int _pathFieldMaxLength = 250;
		private static readonly int _computerNameFieldMaxLength = 50;
		private static readonly string _createStructure = String.Format(
			"CREATE TABLE {0} (" +
			"Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
			"Guid STRING (36), " +
			"Name STRING ({1}), " +
			"VolumeType INTEGER, " +
			"VolumeId STRING ({2}), " +
			"Created DATETIME, " +
			"Cataloged DATETIME, " +
			"TotalSize INTEGER, " +
			"SpaceFree INTEGER, " +
			"RegularFiles INTEGER, " +
			"HiddenFiles INTEGER, " +
			"Path STRING ({3}), " +
			"ComputerName STRING ({4}), " +
			"IncludeInSearch BOOLEAN);",
			_tableName,
			_nameFieldMaxLength,
			_volumeIdFieldMaxLength,
			_pathFieldMaxLength,
			_computerNameFieldMaxLength);

		private readonly IDb _db;

		public VolumeDao()
		{
			_db = new Db();
		}

		public VolumeDao(IDb db)
		{
			_db = db;
		}

		public int CreateStructure()
		{
			return _db.CreateStructure(_createStructure);
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

		private static readonly Func<IDataReader, Volume> Make = reader =>
			new Volume
			{
				Id = reader["Id"].AsInt(),
				Guid = reader["Guid"].AsGuid(),
				Name = reader["Name"].AsString(),
				Type = (VolumeType)reader["VolumeType"].AsInt(),
				VolumeId = reader["VolumeId"].AsString(),
				Created = reader["Created"].AsDateTime(),
				Cataloged = reader["Cataloged"].AsDateTime(),
				TotalSize = reader["TotalSize"].AsInt(),
				SpaceFree = reader["SpaceFree"].AsInt(),
				RegularFiles = reader["RegularFiles"].AsInt(),
				HiddenFiles = reader["HiddenFiles"].AsInt(),
				Path = reader["Path"].AsString(),
				ComputerName = reader["ComputerName"].AsString(),
				IncludeInSearch = reader["IncludeInSearch"].AsBool(),
			};

		private object[] Take(Volume category)
		{
			return new object[]
			{
				"@Id", category.Id,
				"@Guid", category.Guid,
				"@Name", category.Name,
				"@VolumeType", category.Type,
				"@VolumeId", category.VolumeId,
				"@Created", category.Created,
				"@Cataloged", category.Cataloged,
				"@TotalSize", category.TotalSize,
				"@SpaceFree", category.SpaceFree,
				"@RegularFiles", category.RegularFiles,
				"@HiddenFiles", category.HiddenFiles,
				"@Path", category.Path,
				"@ComputerName", category.ComputerName,
				"@IncludeInSearch", category.IncludeInSearch,
			};
		}

		/// <summary>
		/// This function is only used for unit test of private methods
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public object[] TestMakeTake(IDataReader reader)
		{
			var temp = Make(reader);
			return Take(temp);
		}
	}
}

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

		private static readonly string _selectAll = String.Format(
			"SELECT Id, Guid, Name, VolumeType, VolumeId, Created, Cataloged, TotalSize, SpaceFree, RegularFiles, HiddenFiles, Path, ComputerName, IncludeInSearch FROM {0}", 
			_tableName);
		private static readonly string _insert = String.Format(
			"INSERT INTO {0} (Guid, Name, VolumeType, VolumeId, Created, Cataloged, TotalSize, SpaceFree, RegularFiles, HiddenFiles, Path, ComputerName, IncludeInSearch) " +
			"VALUES (@Guid, @Name, @VolumeType, @VolumeId, @Created, @Cataloged, @TotalSize, @SpaceFree, @RegularFiles, @HiddenFiles, @Path, @ComputerName, @IncludeInSearch)", _tableName);
		private static readonly string _update = string.Format(
			"UPDATE {0} SET Guid=@Guid, Name=@Name, VolumeType=@VolumeType, VolumeId=@VolumeId, Created=@Created, Cataloged=@Cataloged, TotalSize=@TotalSize, SpaceFree=@SpaceFree, RegularFiles=@RegularFiles, HiddenFiles=@HiddenFiles, Path=@Path, ComputerName=@ComputerName, IncludeInSearch=@IncludeInSearch WHERE Id = @Id", _tableName);

		private readonly IDb _db;

		public VolumeDao()
		{
			_db = new Db();
		}

		public VolumeDao(IDb db)
		{
			_db = db;
		}

		private void ValidateVolume(Volume volume, bool validateId = false)
		{
			if (validateId)
			{
				if (volume.Id <= 0) throw new ArgumentOutOfRangeException("volume", "volume.Id needs to be > 0");
			}
			if (volume == null) throw new ArgumentNullException("volume");
			if (volume.Guid == Guid.Empty) throw new ArgumentException("volume.Guid can't be empty", "volume");
			if (String.IsNullOrEmpty(volume.Name)) throw new ArgumentException("volume.Name can't be empty", "volume");
			if (volume.Name.Length > _nameFieldMaxLength)
				throw new ArgumentOutOfRangeException(
					String.Format("volume.Name can't be more than {0} characters length", _nameFieldMaxLength), "volume");
			if (volume.VolumeId == null) throw new ArgumentException("volume.VolumeId can't be null", "volume");
			if (volume.VolumeId.Length > _volumeIdFieldMaxLength)
				throw new ArgumentOutOfRangeException(
					String.Format("volume.VolumeId can't be more than {0} characters length", _volumeIdFieldMaxLength), "volume");
			if (String.IsNullOrEmpty(volume.Path)) throw new ArgumentException("volume.Path can't be null or empty", "volume");
			if (volume.Path.Length > _pathFieldMaxLength)
				throw new ArgumentOutOfRangeException(
					String.Format("volume.Path can't be more than {0} characters length", _pathFieldMaxLength), "volume");
			if (volume.ComputerName == null) throw new ArgumentException("volume.ComputerName can't be null", "volume");
			if (volume.ComputerName.Length > _computerNameFieldMaxLength)
				throw new ArgumentOutOfRangeException(
					String.Format("volume.ComputerName can't be more than {0} characters length", _computerNameFieldMaxLength), "volume");
		}

		public int CreateStructure()
		{
			return _db.CreateStructure(_createStructure);
		}

		public List<BusinessObjects.Volume> GetVolumes()
		{
			return _db.Read(_selectAll, Make).ToList();
		}

		public BusinessObjects.Volume GetVolumeById(int id)
		{
			if (id <= 0) throw new ArgumentException("id");

			var sql = String.Format("{0} WHERE Id = @Id", _selectAll);
			object[] parms = { "@Id", id };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public BusinessObjects.Volume GetVolumeByGuid(Guid guid)
		{
			if (guid == Guid.Empty) throw new ArgumentException("guid");

			var sql = String.Format("{0} WHERE Guid = @Guid", _selectAll);
			object[] parms = { "@Guid", guid };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public BusinessObjects.Volume GetVolumeByName(string name)
		{
			if (String.IsNullOrEmpty(name)) throw new ArgumentException("name");
			if (name.Length > _nameFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("name can't be more than {0} characters length", _nameFieldMaxLength), "name");

			var sql = String.Format("{0} WHERE Name = @Name", _selectAll);
			object[] parms = { "@Name", name };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public BusinessObjects.Volume GetVolumeByVolumeId(string volumeId)
		{
			if (String.IsNullOrEmpty(volumeId)) throw new ArgumentException("volumeId");
			if (volumeId.Length > _volumeIdFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("volumeId can't be more than {0} characters length", _volumeIdFieldMaxLength), "volumeId");

			var sql = String.Format("{0} WHERE VolumeId = @VolumeId", _selectAll);
			object[] parms = { "@VolumeId", volumeId };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public bool InsertVolume(BusinessObjects.Volume volume)
		{
			ValidateVolume(volume);

			var sql = _insert;
			return (_db.Insert(sql, Take(volume)) == 1);
		}

		public bool UpdateVolume(BusinessObjects.Volume volume)
		{
			ValidateVolume(volume, true);

			var sql = _update;
			return (_db.Update(sql, Take(volume)) == 1);
		}

		public bool DeleteVolume(int volumeId)
		{
			if (volumeId <= 0) throw new ArgumentOutOfRangeException("volumeId", "volumeId needs to be > 0");

			var sql = string.Format("DELETE FROM {0} WHERE Id = @Id", _tableName);
			object[] parms = { "@Id", volumeId };
			return (_db.Delete(sql, parms) == 1);
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

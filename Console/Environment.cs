using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ActionService;

namespace Console
{
	public class Environment
	{
		private ICatalogService _serviceLazyLoaded = null;
		private ICatalogService _service
		{
			get
			{
				if (_serviceLazyLoaded == null)
				{
					_serviceLazyLoaded = new CatalogService();
				}
				return _serviceLazyLoaded;
			}
		}

		private bool IsDatabaseValid()
		{
			bool valid = false;
			try
			{
				valid = (_service.GetDatabaseVersion() > 0);
			}
			catch
			{
				valid = false;
			}
			return valid;
		}


		public bool AssemblyVersion(IEnumerable<string> parameters)
		{
			var assembly = Assembly.GetExecutingAssembly().GetName();
			var name = "Catalog Master";
			var version = assembly.Version;
			System.Console.WriteLine(String.Format("{0} version {1}", name, version));
			return true;
		}

		public int GetDatabaseVersion()
		{
			int version = 0;
			return version;
		}

		public bool BuildDatabase(IEnumerable<string> parameters)
		{
			bool built = false;
			if (IsDatabaseValid())
			{
				System.Console.WriteLine("Error: The database already exists. It has not been rebuilt.");
			}
			else
			{
				built = _service.InitializeDatabase();
			}
			if (built)
			{
				System.Console.WriteLine("Database successfully built.");
			}
			return built;
		}

		public bool DisplayDriveList(IEnumerable<string> parameters)
		{
			bool displayed = true;
			var drives = System.IO.DriveInfo.GetDrives();
			foreach (var driveInfo in drives)
			{
				if (driveInfo.IsReady)
				{
					System.Console.WriteLine("Path={0} Name={1} Type={2} Label={3}", driveInfo.RootDirectory, driveInfo.Name,
						driveInfo.DriveType, driveInfo.VolumeLabel);
				}
				else
				{
					System.Console.WriteLine("Path={0} Name={1} Type={2}", driveInfo.RootDirectory, driveInfo.Name,
						driveInfo.DriveType.ToString());
				}
			}
			return displayed;
		}

		public bool DisplayDriveInfo(IEnumerable<string> drives)
		{
			bool displayed = true;
			if (drives != null)
			{
				foreach (var drive in drives)
				{
					var driveInfo = new DriveInfo(drive);
					System.Console.WriteLine("Details about drive '{0}':", driveInfo.RootDirectory);
					System.Console.WriteLine(" Name = '{0}'", driveInfo.Name);
					if (driveInfo.DriveType != DriveType.NoRootDirectory)
					{
						System.Console.WriteLine(" Type = {0}", driveInfo.DriveType);
					}
					if (driveInfo.IsReady)
					{
						System.Console.WriteLine(" Label = '{0}'", driveInfo.VolumeLabel);
						System.Console.WriteLine(" Format = {0}", driveInfo.DriveFormat);
						System.Console.WriteLine(" Size = {0}B", _service.DisplayBytesToHuman(driveInfo.TotalSize));
						System.Console.WriteLine(" Total Free space = {0}B", _service.DisplayBytesToHuman(driveInfo.TotalFreeSpace));
						System.Console.WriteLine(" Available Free space = {0}B", _service.DisplayBytesToHuman(driveInfo.AvailableFreeSpace));
					}
					else
					{
						System.Console.WriteLine(" Drive is not available (empty or not mounted)");
					}
				}
			}
			return displayed;
		}
	}
}

using System;
using System.Configuration;


namespace RotatePictures.Utilities
{
	public class UpdateConfigFile
	{
		private static Lazy<UpdateConfigFile> _inst = new Lazy<UpdateConfigFile>(() => new UpdateConfigFile());
		public static UpdateConfigFile Inst = _inst.Value;

		private UpdateConfigFile() { }

		public void UpdateConfig(string key, string val)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings[key].Value = val;
			config.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
		}
	}
}

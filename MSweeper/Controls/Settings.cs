using System;
using System.IO;

namespace MSweeper.Controls
{
    public class Settings
    {
        #region Fields

        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public int MineCount { get; set; }

        #endregion

        #region Ctor/~

        public Settings()
        {
            if (ConfigurationFileExists())
            {
                ReadConfigurationFile();
            }
            else
            {
                UseDefaultSettings();
            }
        }

        ~Settings()
        {
            File.WriteAllText("settings.config",
                RowCount.ToString() + "\n"
                + ColumnCount.ToString() + "\n"
                + MineCount.ToString());
        }

        #endregion

        #region Auxiliary functions

        private bool ConfigurationFileExists()
        {
            return File.Exists("settings.config")
                && new FileInfo("settings.config").Length != 0;
        }

        private void UseDefaultSettings()
        {
            RowCount = 9;
            ColumnCount = 9;
            MineCount = 10;
        }

        private void ReadConfigurationFile()
        {
            try
            {
                using (StreamReader sReader = new StreamReader("settings.config"))
                {
                    RowCount = Convert.ToInt32(sReader.ReadLine());
                    ColumnCount = Convert.ToInt32(sReader.ReadLine());
                    MineCount = Convert.ToInt32(sReader.ReadLine());

                    if (RowCount < 1 || ColumnCount < 1 || MineCount < 1
                        || RowCount > 40 || ColumnCount > 40
                        || MineCount >= RowCount * ColumnCount)
                    {
                        //Force execution of **catch** block
                        throw new FileFormatException();
                    }
                }
            }
            catch
            {
                //Wrong format of file text
                UseDefaultSettings();
            }
        }

        #endregion
    }
}

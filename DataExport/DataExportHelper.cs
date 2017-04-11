using System.Collections.Generic;
using System.IO;

namespace DataExport
{
    class DataExportHelper
    {



        public static void CalculateFreeSpace(Dictionary<string, long> statistics, Dictionary<string, long> rownums)
        {
            foreach (var stat in statistics)
            {
                

            }
        }

        public static long GetTotalFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == Path.GetPathRoot(driveName))
                {
                    return drive.AvailableFreeSpace;
                }
            }
            return -1;
        }
    }
}

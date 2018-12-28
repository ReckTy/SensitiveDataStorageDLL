using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            SensitiveDataStorage.SensitiveDataStorage sds = new SensitiveDataStorage.SensitiveDataStorage()
            {
                // 'SensitiveDataStorage.EncryptionPassword' has to be set to read/write. It can contain any characters, a string can handle.
                EncryptionPassword = "Jjd!%vbfSGEYmfk420"
            };

            // Get the storage folder path:
            Console.WriteLine("Storage Folder Path: " + sds.GetStorageFolderPath() + "\n");

            // Create the file:
            sds.CreateFile("ExampleFile");

            // Write information(string) to specified line:
            sds.WriteLine("ExampleFile", 0, "Encrypted string 1");
            sds.WriteLine("ExampleFile", 1, "Encrypted string 2");

            // Read information(string) from specified line:
            Console.WriteLine(sds.ReadLine("ExampleFile", 0));
            Console.WriteLine(sds.ReadLine("ExampleFile", 1) + "\n");
            
            // Read information(string) from specified line after file has been cleared:
            sds.ClearFile("ExampleFile");
            Console.WriteLine("ExampleFile Cleared.");
            Console.WriteLine(sds.ReadLine("ExampleFile", 0));
            Console.WriteLine(sds.ReadLine("ExampleFile", 1));

            // Delete the file
            sds.DeleteFile("ExampleFile");
        }
    }
}

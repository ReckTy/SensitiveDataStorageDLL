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
                // 'SensitiveDataStorage.EncryptionPassword' has to be set for the DLL to work. It can contain any characters, a string can handle.
                EncryptionPassword = "Jjd!%vbSGEYmfk420"
            };
            
            // Create the file:
            sds.Create("ExampleFile");

            // Write information(string) to specified line:
            sds.WriteLine("ExampleFile", 0, "Hej, Hej, Monika, Hej på dig Monika!");
            sds.WriteLine("ExampleFile", 1, "Skrrt Skrrt");

            // Read information(string) from specified line:
            Console.WriteLine(sds.ReadLine("ExampleFile", 0));
            Console.WriteLine(sds.ReadLine("ExampleFile", 1));
            
        }
    }
}

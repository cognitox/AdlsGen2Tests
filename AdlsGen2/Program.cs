namespace AdlsGen2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting operation.......");
            StorageDriver driver = new StorageDriver();
            try
            {
                //driver.CreateFileClient_Filesystem();
                //driver.CreateFileClient_Directory();

                //driver.Upload();
                //driver.Append_Simple();
                //driver.Append();
                driver.UploadFiles().Wait();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);                
            };
        }
    }
}

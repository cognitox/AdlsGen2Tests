// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;

namespace AdlsGen2
{
    public class StorageDriver : StorageDriverBase
    {
        /// <summary>
        /// Create a DataLake File using a DataLake Filesystem.
        /// </summary>       
        public void CreateFileClient_Filesystem()
        {
            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;

            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);
           
          
            // Create a DataLake Filesystem
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem1");
            filesystem.Create();

            // Create a DataLake file using a DataLake Filesystem
            DataLakeFileClient file = filesystem.GetFileClient("sample-file1");
            file.Create();
            

            // Verify we created one file
            //Assert.AreEqual(1, filesystem.GetPaths().Count());

            // Cleanup
            //filesystem.Delete();
        }

        /// <summary>
        /// Create a DataLake File using a DataLake Directory.
        /// </summary>
        
        public void CreateFileClient_Directory()
        {
            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;

            #region Snippet:SampleSnippetDataLakeFileSystemClient_Create
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Create a DataLake Filesystem
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem2");
            filesystem.Create();
            #endregion Snippet:SampleSnippetDataLakeFileSystemClient_Create
            #region Snippet:SampleSnippetDataLakeFileClient_Create_Directory
            // Create a DataLake Directory
            DataLakeDirectoryClient directory = filesystem.CreateDirectory("sample-directory2");
            directory.Create();

            // Create a DataLake File using a DataLake Directory
            DataLakeFileClient file = directory.GetFileClient("sample-file2");
            file.Create();
            #endregion Snippet:SampleSnippetDataLakeFileClient_Create_Directory

            // Verify we created one file
            //Assert.AreEqual(1, filesystem.GetPaths().Count());

            // Cleanup
            //filesystem.Delete();
        }

        /// <summary>
        /// Create a DataLake Directory.
        /// </summary>
        
        public void CreateDirectoryClient()
        {
            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;

            #region Snippet:SampleSnippetDataLakeDirectoryClient_Create
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-append" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-append");
            filesystem.Create();

            // Create
            DataLakeDirectoryClient directory = filesystem.GetDirectoryClient("sample-file");
            directory.Create();
            #endregion Snippet:SampleSnippetDataLakeDirectoryClient_Create

            // Verify we created one directory
            //Assert.AreEqual(1, filesystem.GetPaths().Count());

            // Cleanup
            //filesystem.Delete();
        }

        /// <summary>
        /// Upload a file to a DataLake File.
        /// </summary>

        public void Append_Simple()
        {
            // Create Sample File to read content from
            string sampleFilePath = CreateTempFile(SampleFileContent);

            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;

            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-append" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-container-with-scope");
            //filesystem.Create();
            try
            {
                #region Snippet:SampleSnippetDataLakeFileClient_Append
                // Create a file
                DataLakeFileClient file = filesystem.GetFileClient("sample-file");
                file.Create();

                // Append data to the DataLake File
                file.Append(File.OpenRead(sampleFilePath), 0);
                file.Flush(SampleFileContent.Length);
                #endregion Snippet:SampleSnippetDataLakeFileClient_Append

                // Verify the contents of the file
                PathProperties properties = file.GetProperties();
                //Assert.AreEqual(SampleFileContent.Length, properties.ContentLength);
            }
            finally
            {
                // Clean up after the test when we're finished
                //filesystem.Delete();
            }
        }

        /// <summary>
        /// Upload file by creating a file, and then appending each part to a DataLake File.
        /// </summary>
       
        public void Append()
        {
            // Create three temporary Lorem Ipsum files on disk that we can upload
            int contentLength = 100;
            string sampleFileContentPart1 = CreateTempFile(SampleFileContent.Substring(0, contentLength));
            string sampleFileContentPart2 = CreateTempFile(SampleFileContent.Substring(contentLength, contentLength));
            string sampleFileContentPart3 = CreateTempFile(SampleFileContent.Substring(contentLength * 2, contentLength));

            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;

            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-append" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-container-with-scope");
            //filesystem.Create();
            try
            {
                // Get a reference to a file named "sample-file" in a filesystem
                DataLakeFileClient file = filesystem.GetFileClient("sample-file");

                // Create the file
                file.CreateIfNotExists();

                // Verify we created one file
                //Assert.AreEqual(1, filesystem.GetPaths().Count());

                // Append data to an existing DataLake File.  Append is currently limited to 4000 MB per call.
                // To upload a large file all at once, consider using Upload() instead.
                file.Append(File.OpenRead(sampleFileContentPart1), 0);
                file.Append(File.OpenRead(sampleFileContentPart2), contentLength);
                file.Append(File.OpenRead(sampleFileContentPart3), contentLength * 2);

                file.Flush(contentLength * 3);

                // Verify the contents of the file
                PathProperties properties = file.GetProperties();
                //Assert.AreEqual(contentLength * 3, properties.ContentLength);
            }
            finally
            {
                // Clean up after the test when we're finished
                //filesystem.Delete();
            }
        }


        public  async Task UploadFiles()
        { 
            // Azure Storage credentials
             string accountName = StorageAccountName;
             string accountKey = StorageAccountKey;
             string fileSystemName = StorageContainer; // This is the container in ADLS Gen2
             string targetDirectoryInAzure = StorageTargetDirectory; // Directory in the file system

             Uri serviceUri = StorageAccountBlobUri;
            // Source directory to read files
            string sourceDirectory = SourceLocalDirectory;

            // Define chunk size (in bytes)
            int chunkSize = 1024 * 1; // 1kB chunks

            // Build the DataLakeServiceClient
            var sharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
            var dataLakeServiceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get the file system (container) client
            DataLakeFileSystemClient fileSystemClient = dataLakeServiceClient.GetFileSystemClient(fileSystemName);

            // Get all files in the source directory
            string[] files = Directory.GetFiles(sourceDirectory);

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[chunkSize];
                    int bytesRead;
                    int chunkIndex = 0;

                    // Create a new file in Azure Data Lake for each file from the source
                    DataLakeDirectoryClient directoryClient = fileSystemClient.GetDirectoryClient(targetDirectoryInAzure);
                    DataLakeFileClient fileClient = directoryClient.GetFileClient(fileName);

                    // Create the file in Azure Data Lake
                    await fileClient.CreateAsync();

                    long offset = 0;

                    while ((bytesRead = sourceStream.Read(buffer, 0, chunkSize)) > 0)
                    {
                        // Append the chunk to the file in ADLS Gen2
                        using (MemoryStream chunkStream = new MemoryStream(buffer, 0, bytesRead))
                        {
                            await fileClient.AppendAsync(chunkStream, offset);
                        }

                        // Increment the offset by the number of bytes written
                        offset += bytesRead;

                        Console.WriteLine($"Chunk {chunkIndex} of {fileName} uploaded to Azure");

                        // Uncomment to flush after every append
                        if (chunkIndex%3 == 0)
                        {
                            await fileClient.FlushAsync(offset);
                        }

                        chunkIndex++;
                    }

                    // Once all chunks are uploaded, flush the data to commit the file
                    await fileClient.FlushAsync(offset);
                }
            }

            Console.WriteLine("File chunking and upload to Azure completed.");
        }


        /// <summary>
        /// Upload file by appending each part to a DataLake File.
        /// </summary>
      
        public void Upload()
        {
            // Create three temporary Lorem Ipsum files on disk that we can upload
            int contentLength = 10;
            string sampleFileContent = CreateTempFile(SampleFileContent.Substring(0, contentLength));

            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;

            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-append" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-container-with-scope");
            //filesystem.Create();
            try
            {
                // Get a reference to a file named "sample-file" in a filesystem
                DataLakeFileClient file = filesystem.GetFileClient("sample-file");

                // Create the file
                file.Create();

                // Verify we created one file
               // Assert.AreEqual(1, filesystem.GetPaths().Count());

                // Upload content to the file.  When using the Upload API, you don't need to create the file first.
                // If the file already exists, it will be overwritten.
                // For larger files, Upload() will upload the file in multiple sequential requests.
                file.Upload(File.OpenRead(sampleFileContent),true);

                // Verify the contents of the file
                PathProperties properties = file.GetProperties();
                //Assert.AreEqual(contentLength, properties.ContentLength);
            }
            finally
            {
                // Clean up after the test when we're finished
                //filesystem.Delete();
            }
        }

        /// <summary>
        /// Download a DataLake File to a file.
        /// </summary>
        
        public void Read()
        {
            // Create a temporary Lorem Ipsum file on disk that we can upload
            string originalPath = CreateTempFile(SampleFileContent);

            // Get a temporary path on disk where we can download the file
            string downloadPath = CreateTempPath();

            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-read" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-read");
            filesystem.Create();
            try
            {
                // Get a reference to a file named "sample-file" in a filesystem
                DataLakeFileClient file = filesystem.GetFileClient("sample-file");

                // First upload something the DataLake file so we have something to download
                file.Upload(File.OpenRead(originalPath));

                // Download the DataLake file's contents and save it to a file
                // The ReadAsync() API downloads a file in a single requests.
                // For large files, it may be faster to call ReadTo()
                #region Snippet:SampleSnippetDataLakeFileClient_Read
                var fileContents = file.Read();
                #endregion Snippet:SampleSnippetDataLakeFileClient_Read
                using (FileStream stream = File.OpenWrite(downloadPath))
                {
                    fileContents.Value.Content.CopyTo(stream);
                }

                // Verify the contents
                //Assert.AreEqual(SampleFileContent, File.ReadAllText(downloadPath));
            }
            finally
            {
                // Clean up after the test when we're finished
                filesystem.Delete();
            }
        }

        /// <summary>
        /// Download a DataLake File to a file.
        /// </summary>
        
        public void ReadTo()
        {
            // Create a temporary Lorem Ipsum file on disk that we can upload
            string originalPath = CreateTempFile(SampleFileContent);

            // Get a temporary path on disk where we can download the file
            string downloadPath = CreateTempPath();

            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-read" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-read");
            filesystem.Create();
            try
            {
                // Get a reference to a file named "sample-file" in a filesystem
                DataLakeFileClient file = filesystem.GetFileClient("sample-file");

                // First upload something the DataLake file so we have something to download
                file.Upload(File.OpenRead(originalPath));

                // Download the DataLake file's directly to a file.
                // For larger files, ReadTo() will download the file in multiple sequential requests.
                file.ReadTo(downloadPath);

                // Verify the contents
                //Assert.AreEqual(SampleFileContent, File.ReadAllText(downloadPath));
            }
            finally
            {
                // Clean up after the test when we're finished
                filesystem.Delete();
            }
        }

        /// <summary>
        /// List all the DataLake directories in a filesystem.
        /// </summary>
        
        public void List()
        {
            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-list" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-list");
            filesystem.Create();
            try
            {
                // Upload a couple of directories so we have something to list
                filesystem.CreateDirectory("sample-directory1");
                filesystem.CreateDirectory("sample-directory2");
                filesystem.CreateDirectory("sample-directory3");

                // List all the directories
                List<string> names = new List<string>();
                #region Snippet:SampleSnippetDataLakeFileClient_List
                foreach (PathItem pathItem in filesystem.GetPaths())
                {
                    names.Add(pathItem.Name);
                }
                #endregion Snippet:SampleSnippetDataLakeFileClient_List
                //Assert.AreEqual(3, names.Count);
                //Assert.Contains("sample-directory1", names);
                //Assert.Contains("sample-directory2", names);
                //Assert.Contains("sample-directory3", names);
            }
            finally
            {
                // Clean up after the test when we're finished
                filesystem.Delete();
            }
        }

        /// <summary>
        /// Traverse the DataLake Files and DataLake Directories in a DataLake filesystem.
        /// </summary>
       
        public void Traverse()
        {
            // Create a temporary Lorem Ipsum file on disk that we can upload
            string originalPath = CreateTempFile(SampleFileContent);

            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-traverse" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-traverse");

            filesystem.Create();
            try
            {
                // Create a bunch of directories and files within the directories
                DataLakeDirectoryClient first = filesystem.CreateDirectory("first");
                first.CreateSubDirectory("a");
                first.CreateSubDirectory("b");
                DataLakeDirectoryClient second = filesystem.CreateDirectory("second");
                second.CreateSubDirectory("c");
                second.CreateSubDirectory("d");
                filesystem.CreateDirectory("third");
                DataLakeDirectoryClient fourth = filesystem.CreateDirectory("fourth");
                DataLakeDirectoryClient deepest = fourth.CreateSubDirectory("e");

                // Upload a DataLake file named "file"
                DataLakeFileClient file = deepest.GetFileClient("file");
                file.Create();
                using (FileStream stream = File.OpenRead(originalPath))
                {
                    file.Append(stream, 0);
                }

                // Keep track of all the names we encounter
                List<string> names = new List<string>();
                foreach (PathItem pathItem in filesystem.GetPaths(recursive: true))
                {
                    names.Add(pathItem.Name);
                }

                // Verify we've seen everything
                //Assert.AreEqual(10, names.Count);
                //Assert.Contains("first", names);
                //Assert.Contains("second", names);
                //Assert.Contains("third", names);
                //Assert.Contains("fourth", names);
                //Assert.Contains("first/a", names);
                //Assert.Contains("first/b", names);
                //Assert.Contains("second/c", names);
                //Assert.Contains("second/d", names);
                //Assert.Contains("fourth/e", names);
                //Assert.Contains("fourth/e/file", names);
            }
            finally
            {
                // Clean up after the test when we're finished
                filesystem.Delete();
            }
        }

        /// <summary>
        /// Trigger a recoverable error.
        /// </summary>
        
        public void Errors()
        {
            // Make StorageSharedKeyCredential to pass to the serviceClient
            string storageAccountName = StorageAccountName;
            string storageAccountKey = StorageAccountKey;
            Uri serviceUri = StorageAccountBlobUri;
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

            // Create DataLakeServiceClient using StorageSharedKeyCredentials
            DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

            // Get a reference to a filesystem named "sample-filesystem-errors" and then create it
            DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-errors");
            filesystem.Create();
            try
            {
                // Try to create the filesystem again
                filesystem.Create();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == "ContainerAlreadyExists")
            {
                // Ignore any errors if the filesystem already exists
            }
            catch (RequestFailedException ex)
            {
                //Assert.Fail($"Unexpected error: {ex}");
            }

            // Clean up after the test when we're finished
            filesystem.Delete();
        }

       
    }
}

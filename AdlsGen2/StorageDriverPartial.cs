using Azure.Storage.Files.DataLake.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdlsGen2
{
    internal class StorageDriverPartial
    {

        ///// <summary>
        ///// Set and gets access control list on a DataLake File
        ///// </summary>
        
        //public void SetGetAcls()
        //{
        //    // Make StorageSharedKeyCredential to pass to the serviceClient
        //    string storageAccountName = NamespaceStorageAccountName;
        //    string storageAccountKey = NamespaceStorageAccountKey;
        //    Uri serviceUri = NamespaceBlobUri;
        //    StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

        //    // Create DataLakeServiceClient using StorageSharedKeyCredentials
        //    DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

        //    // Get a reference to a filesystem named "sample-filesystem-acl" and then create it
        //    DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-acl");
        //    filesystem.Create();
        //    try
        //    {
        //        #region Snippet:SampleSnippetDataLakeFileClient_SetAcls
        //        // Create a DataLake file so we can set the Access Controls on the files
        //        DataLakeFileClient fileClient = filesystem.GetFileClient("sample-file");
        //        fileClient.Create();

        //        // Set Access Control List
        //        IList<PathAccessControlItem> accessControlList
        //            = PathAccessControlExtensions.ParseAccessControlList("user::rwx,group::r--,mask::rwx,other::---");
        //        fileClient.SetAccessControlList(accessControlList);
        //        #endregion Snippet:SampleSnippetDataLakeFileClient_SetAcls
        //        #region Snippet:SampleSnippetDataLakeFileClient_GetAcls
        //        // Get Access Control List
        //        PathAccessControl accessControlResponse = fileClient.GetAccessControl();
        //        #endregion Snippet:SampleSnippetDataLakeFileClient_GetAcls

        //        // Check Access Control permissions
        //        Assert.AreEqual(
        //            PathAccessControlExtensions.ToAccessControlListString(accessControlList),
        //            PathAccessControlExtensions.ToAccessControlListString(accessControlResponse.AccessControlList.ToList()));
        //    }
        //    finally
        //    {
        //        // Clean up after the test when we're finished
        //        filesystem.Delete();
        //    }
        //}

        ///// <summary>
        ///// Rename a DataLake file and a DataLake directory in a DataLake Filesystem.
        ///// </summary>
     
        //public void Rename()
        //{
        //    // Make StorageSharedKeyCredential to pass to the serviceClient
        //    string storageAccountName = StorageAccountName;
        //    string storageAccountKey = StorageAccountKey;
        //    Uri serviceUri = StorageAccountBlobUri;
        //    StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

        //    // Create DataLakeServiceClient using StorageSharedKeyCredentials
        //    DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

        //    // Get a reference to a filesystem named "sample-filesystem-rename" and then create it
        //    DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem-rename");
        //    filesystem.Create();
        //    try
        //    {
        //        // Create a DataLake Directory to rename it later
        //        DataLakeDirectoryClient directoryClient = filesystem.GetDirectoryClient("sample-directory");
        //        directoryClient.Create();

        //        // Rename directory with new path/name and verify by making a service call (e.g. GetProperties)
        //        #region Snippet:SampleSnippetDataLakeFileClient_RenameDirectory
        //        DataLakeDirectoryClient renamedDirectoryClient = directoryClient.Rename("sample-directory2");
        //        #endregion Snippet:SampleSnippetDataLakeFileClient_RenameDirectory
        //        PathProperties directoryPathProperties = renamedDirectoryClient.GetProperties();

        //        // Delete the sample directory using the new path/name
        //        filesystem.DeleteDirectory("sample-directory2");

        //        // Create a DataLake file.
        //        DataLakeFileClient fileClient = filesystem.GetFileClient("sample-file");
        //        fileClient.Create();

        //        // Rename file with new path/name and verify by making a service call (e.g. GetProperties)
        //        #region Snippet:SampleSnippetDataLakeFileClient_RenameFile
        //        DataLakeFileClient renamedFileClient = fileClient.Rename("sample-file2");
        //        #endregion Snippet:SampleSnippetDataLakeFileClient_RenameFile
        //        PathProperties filePathProperties = renamedFileClient.GetProperties();

        //        // Delete the sample directory using the new path/name
        //        filesystem.DeleteFile("sample-file2");
        //    }
        //    finally
        //    {
        //        // Clean up after the test when we're finished
        //        filesystem.Delete();
        //    }
        //}

        ///// <summary>
        ///// Get Properties on a DataLake File and a Directory
        ///// </summary>
      
        //public void GetProperties()
        //{
        //    // Make StorageSharedKeyCredential to pass to the serviceClient
        //    string storageAccountName = StorageAccountName;
        //    string storageAccountKey = StorageAccountKey;
        //    Uri serviceUri = StorageAccountBlobUri;
        //    StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);

        //    // Create DataLakeServiceClient using StorageSharedKeyCredentials
        //    DataLakeServiceClient serviceClient = new DataLakeServiceClient(serviceUri, sharedKeyCredential);

        //    // Get a reference to a filesystem named "sample-filesystem-rename" and then create it
        //    DataLakeFileSystemClient filesystem = serviceClient.GetFileSystemClient("sample-filesystem");
        //    filesystem.Create();
        //    try
        //    {
        //        // Create a DataLake Directory to rename it later
        //        DataLakeDirectoryClient directoryClient = filesystem.GetDirectoryClient("sample-directory");
        //        directoryClient.Create();

        //        #region Snippet:SampleSnippetDataLakeDirectoryClient_GetProperties
        //        // Get Properties on a Directory
        //        PathProperties directoryPathProperties = directoryClient.GetProperties();
        //        #endregion Snippet:SampleSnippetDataLakeDirectoryClient_GetProperties

        //        // Create a DataLake file
        //        DataLakeFileClient fileClient = filesystem.GetFileClient("sample-file");
        //        fileClient.Create();

        //        #region Snippet:SampleSnippetDataLakeFileClient_GetProperties
        //        // Get Properties on a File
        //        PathProperties filePathProperties = fileClient.GetProperties();
        //        #endregion Snippet:SampleSnippetDataLakeFileClient_GetProperties
        //    }
        //    finally
        //    {
        //        // Clean up after the test when we're finished
        //        filesystem.Delete();
        //    }
        //}
    }
}

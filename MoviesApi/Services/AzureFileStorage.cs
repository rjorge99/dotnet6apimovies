﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesApi.Services
{
    public class AzureFileStorage : IFileStorage
    {
        private readonly string _connection;

        public AzureFileStorage(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            var client = new BlobContainerClient(_connection, container);
            await client.CreateIfNotExistsAsync();

            var fileName = $"{Guid.NewGuid()}¨{extension}";
            var blob = client.GetBlobClient(fileName);

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeader = new BlobHttpHeaders();
            blobHttpHeader.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeader;

            await blob.UploadAsync(new BinaryData(content), blobUploadOptions);
            return blob.Uri.ToString();
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string path, string contentType)
        {
            await DeleteFile(path, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path)) return;

            var client = new BlobContainerClient(_connection, container);
            await client.CreateIfNotExistsAsync();

            var file = Path.GetFileName(path);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();
        }
    }
}

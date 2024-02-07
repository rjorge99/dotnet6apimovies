namespace MoviesApi.Services
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _context;

        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor context)
        {
            _env = env;
            _context = context;
        }
        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folder = Path.Combine(_env.WebRootPath, container);

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(path, content);

            var urlActual = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";
            var databasePath = Path.Combine(urlActual, container, fileName).Replace("\\", "/");
            return databasePath;
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string path, string contentType)
        {
            await DeleteFile(path, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path)) return Task.CompletedTask;

            var fileName = Path.GetFileName(path);
            var fileDirectory = Path.Combine(_env.WebRootPath, container, fileName);

            if (File.Exists(fileDirectory))
                File.Delete(fileDirectory);

            return Task.CompletedTask;
        }
    }
}

using DikkeTennisLijst.Core.Interfaces.Repositories;

namespace DikkeTennisLijst.Infrastructure.Repositories
{
    public class TextFileRepository : ITextFileRepository
    {
        public string Read(string folderPath, string fileName)
        {
            var path = Path.Combine(folderPath, fileName);
            return Read(path);
        }

        public string Read(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();
            return File.ReadAllText(path);
        }

        public async Task<string> ReadAsync(string fileFolderPath, string fileName)
        {
            var filePath = Path.Combine(fileFolderPath, fileName);
            return await ReadAsync(filePath);
        }

        public async Task<string> ReadHTMLAsync(string fileFolderPath, string fileName)
        {
            var filePath = Path.Combine(fileFolderPath, fileName);
            return string.Concat(await File.ReadAllLinesAsync(filePath));
        }

        public async Task<string> ReadAsync(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            return await File.ReadAllTextAsync(filePath);
        }
    }
}
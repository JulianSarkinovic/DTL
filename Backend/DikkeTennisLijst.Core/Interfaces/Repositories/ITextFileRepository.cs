namespace DikkeTennisLijst.Core.Interfaces.Repositories
{
    public interface ITextFileRepository
    {
        public string Read(string folderPath, string fileName);

        public string Read(string path);

        public Task<string> ReadAsync(string folderPath, string fileName);

        public Task<string> ReadHTMLAsync(string folderPath, string fileName);

        public Task<string> ReadAsync(string path);
    }
}
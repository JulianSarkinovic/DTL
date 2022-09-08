namespace DikkeTennisLijst.Core.Shared.Helpers
{
    public class FileResponse
    {
        // Disabling the default constructor?

        public FileResponse(Stream content, string contentType)
        {
            Content = content;
            ContentType = contentType;
        }

        public Stream Content { get; set; }
        public string ContentType { get; set; }
    }
}
﻿namespace DikkeTennisLijst.Core.Shared.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream memStream) return memStream.ToArray();

            using var memoryStream = new MemoryStream();
            instream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
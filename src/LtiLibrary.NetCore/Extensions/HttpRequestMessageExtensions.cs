using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LtiLibrary.NetCore.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="HttpRequestMessageExtensions"/> type.
    /// </summary>
    internal static class HttpRequestMessageExtensions
    {
        public static void SetupHeaders(this HttpRequestMessage msg, string authToken = null)
        {
            msg.Headers.Clear();
            msg.Headers.Add("Connection", "keep-alive");
            msg.Headers.Add("Keep-Alive", "600");

            msg.Headers.Accept.Clear();

            if (!string.IsNullOrEmpty(authToken))
                msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

        public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage request, MemoryStream stream, StreamContent streamContent, bool isGzip = false)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = await request.Content.CloneAsync(stream, streamContent, isGzip).ConfigureAwait(false),
                Version = request.Version
            };
            foreach (KeyValuePair<string, object> prop in request.Properties)
            {
                clone.Properties.Add(prop);
            }
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        public static async Task<HttpContent> CloneAsync(this HttpContent content, MemoryStream stream, StreamContent streamContent, bool isGzip = false)
        {
            if (content == null)
                return null;

            stream.Seek(0, SeekOrigin.Begin);
            await stream.FlushAsync().ConfigureAwait(false);

            if (isGzip)
            {
                using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Compress, true))
                    await content.CopyToAsync(gzipStream).ConfigureAwait(false);

                stream.Seek(0, SeekOrigin.Begin);
                streamContent.Headers.Add("Content-Encoding", "gzip");
                streamContent.Headers.ContentLength = stream.Length;

                foreach (var header in content.Headers)
                    streamContent.Headers.Add(header.Key, header.Value);

                return streamContent;
            }
            else
            {
                await content.CopyToAsync(stream).ConfigureAwait(false);
                stream.Position = 0;

                foreach (var header in content.Headers)
                    streamContent.Headers.Add(header.Key, header.Value);

                return streamContent;
            }

        }
    }
}

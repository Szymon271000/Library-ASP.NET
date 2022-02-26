using ImageMagick;
using Library.Models.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Models.Services.Database
{
    public class MagickNetImagePersister : IBookCoverPersister
    {
        private readonly SemaphoreSlim semaphore;
        private readonly IWebHostEnvironment env;

        public MagickNetImagePersister(IWebHostEnvironment env)
        {
            ResourceLimits.Height = 4000;
            ResourceLimits.Width = 4000;
            semaphore = new SemaphoreSlim(2);
            this.env = env;
        }
        public async Task<string> SaveBookCoverAsync(int bookId, IFormFile formFile)
        {
            await semaphore.WaitAsync();
            try
            {
                string path = $"/books/{bookId}.jpg";
                string physicalPath = Path.Combine(env.WebRootPath, "books", $"{bookId}.jpg");
                using Stream inputStream = formFile.OpenReadStream();
                using MagickImage image = new MagickImage(inputStream);
                int width = 300;
                int height = 300;
                var resizeGeometry = new MagickGeometry(width, height)
                {
                    FillArea = false
                };
                image.Resize(resizeGeometry);
                image.Extent(width, height, MagickColor.FromRgb(255, 255, 255));
                image.Write(physicalPath, MagickFormat.Jpg);
                return path;
            }
            catch (Exception exc)
            {
                throw new ImagePersistenceException(exc);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}

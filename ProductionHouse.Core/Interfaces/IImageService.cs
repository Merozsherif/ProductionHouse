using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Interfaces
{
    using Microsoft.AspNetCore.Http;

    namespace ProductionHouse.Core.Interfaces
    {
        public interface IImageService
        {
            Task<string> UploadAsync(IFormFile file, string folderName);

            Task<List<string>> UploadManyAsync(
                List<IFormFile> files,
                string folderName);

            void DeleteImage(string imagePath);
        }
    }
}

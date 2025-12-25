using FlipShop.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using static NuGet.Packaging.PackagingConstants;

namespace FlipShop.WebApi.Common
{
    public static class HandleImage
    {
        public static string userImageFolder = "Profile/Images/";
        public static string productImageFolder = "Product/Images/";
        public static async Task<string?> AddImageToServer<TEntity>(IFormFile image, TEntity entity, string imageFolder, IWebHostEnvironment webHostEnvironment)
        {
            string imageExtension = System.IO.Path.GetExtension(image.FileName);
            if (imageExtension.ToLower().Equals(".jpg") || imageExtension.ToLower().Equals(".png") || imageExtension.ToLower().Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                imageFolder += string.Format("{0}_{1}", Guid.NewGuid().ToString(), image.FileName);
                string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, imageFolder);
                using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return "/" + imageFolder;
            }
            return null;
        }
    }
}

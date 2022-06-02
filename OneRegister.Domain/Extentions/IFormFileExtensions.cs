using Microsoft.AspNetCore.Http;
using System.IO;

namespace OneRegister.Domain.Extentions
{
    public static class IFormFileExtension
    {
        public static byte[] ToByte(this IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            var ms = new MemoryStream();
            var s = file.OpenReadStream();
            s.CopyTo(ms);
            return ms.ToArray();
        }
    }
}

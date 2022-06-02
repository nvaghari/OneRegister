using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileAttribute : ValidationAttribute
    {
        public int MaxSize { get; set; } = 2;

        private readonly string[] AcceptableFormats = { "jpg", "jpeg", "png", "pdf" };
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (value is IFormFile file)
            {
                return IsFormatValid(file) && IsSizeValid(file);
            }
            else
            {
                ErrorMessage = "Type of property is not supported";
                return false;
            }
        }

        private bool IsSizeValid(IFormFile file)
        {
            ErrorMessage = $"File size can not be more than {MaxSize}MB";
            return file.Length <= MaxSize * 1024 * 1000;
        }
        private bool IsFormatValid(IFormFile file)
        {
            ErrorMessage = $"Only this formats are acceptable {ListOfItems(AcceptableFormats)}";
            var extension = Path.GetExtension(file.FileName).Remove(0, 1);
            return AcceptableFormats.Contains(extension);
        }
        private static string ListOfItems(string[] arr)
        {
            return arr.Aggregate((a, b) => a + ", " + b);
        }
    }
}

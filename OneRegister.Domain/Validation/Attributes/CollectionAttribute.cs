using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OneRegister.Domain.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class CollectionAttribute : ValidationAttribute
    {
        private string[] _list;
        public CollectionAttribute(string[] List)
        {
            _list = List;
        }
        public CollectionAttribute(string ListString)
        {
            _list = ListString.Split(",");
        }
        public override bool IsValid(object value)
        {
            ErrorMessage = $"Acceptable values: {ListOfItems(_list)}";
            if (string.IsNullOrEmpty(value?.ToString()))
            {
                return true;
            }
            return _list.Contains(value.ToString());
        }
        private static string ListOfItems(string[] arr)
        {
            return arr.Aggregate((a, b) => a + ", " + b);
        }
    }
}

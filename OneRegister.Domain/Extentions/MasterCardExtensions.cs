using OneRegister.Domain.Model.Enum.MasterCard;
using System;

namespace OneRegister.Domain.Extentions
{
    public static class MasterCardExtensions
    {
        public static string ToCharacter(this Gender gender)
        {
            return gender switch
            {
                Gender.Female => "F",
                Gender.Male => "M",
                _ => throw new ArgumentException("Gender Enumeration out of range"),
            };
        }
    }
}

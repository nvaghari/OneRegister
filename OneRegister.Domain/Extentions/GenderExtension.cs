using OneRegister.Data.SuperEntities;

namespace OneRegister.Domain.Extentions
{
    public static class GenderExtension
    {
        public static string ToGenderString(this Gender gender)
        {
            if (gender == Gender.Female) return "F";
            return "M";
        }
    }
}

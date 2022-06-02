using OneRegister.Data.Entities.MerchantRegistration;
using OneRegister.Domain.Model.Enum.Merchant;

namespace OneRegister.Domain.Extentions
{
    public static class MerchantExtensions
    {
        public static BankPromotionalPremisesType? ToBankPromoType(this bool? boolean)
        {
            if (boolean == null) return null;
            return boolean.Value == true ? BankPromotionalPremisesType.Yes : BankPromotionalPremisesType.No;
        }
        public static string ToKeyString(this MerchantRegisterState state)
        {
            return ((int)state).ToString();
        }
        public static string ToPositionName(this MerchantFilePosition position)
        {
            var index = (int)position;
            if (index == 0)
            {
                return "DmsRef";
            }
            return "DmsRef" + index.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneRegister.Data.Contract
{
    public static class Constants
    {
        public static class DataLength
        {
            public const int PHONE = 32;
            public const int IDENTITY = 32;
            public const int PostCode = 32;
            public const int EMAIL = 64;
            public const int SHORTNAME = 64;
            public const int Name = 128;
            public const int RefId = 128;
            public const int URL = 256;
            public const int LONGNAME = 256;
            public const int ADDRESS = 512;
            public const int Description = 512;
            public const int Remark = 1024;
            public const int ShortJson = 2048;
            public const int MediumJson = 4096;

        }
        public static class BasicUser
        {
            public static string AdminUser => "admin";
            public static Guid AdminId => Guid.Parse("9271AABC-FDCF-440E-A29A-EA72EC766396");
            public static string AdminPassword => "[]";
            public static string AdminEmail => "nvaghari@gmail.com";
            public static string AdminPhone => "134962653";
            public static string AdminName => "Admin";
        }
        public static class BasicRoles
        {
            public static (string name, Guid organizationId) SuperAdmin => ("SuperAdmin", BasicOrganizations.OneRegister);


            public static (string name, Guid organizationId) Merchant => ("Merchant", BasicOrganizations.Merchant);
            public static (string name, Guid organizationId) MerchantOPLvl1 => ("Operation Level 1", BasicOrganizations.Merchant);
            public static (string name, Guid organizationId) MerchantOPLvl2 => ("Operation Level 2", BasicOrganizations.Merchant);
            public static (string name, Guid organizationId) MerchantRiskHead => ("Risk Head", BasicOrganizations.Merchant);
            public static (string name, Guid organizationId) SalesPerson => ("Salesperson", BasicOrganizations.Merchant);

        }
        public static class BasicOrganizations
        {
            public const string OneRegister_ID = "2E8D8408-10E3-41B1-8483-3B44D5F201FE";
            public const string Agropreneur_ID = "BD9ACF07-850E-49A1-B997-B95F2EDD5664";
            public const string Merchant_ID = "B4C23F12-8F64-45AD-BC21-6FD1662EAF19";
            public const string School_ID = "72A02872-F415-4D5D-A5EF-B4F8EAC60142";
            public const string MasterCard_ID = "89B6D466-F95F-4F4B-970E-E04C60556B2C";


            private static readonly Dictionary<Guid, string> _organizationList = new() { 
                {Guid.Parse(OneRegister_ID), "OneRegister" }, 
                {Guid.Parse(Agropreneur_ID), "Agropreneur" }, 
                {Guid.Parse(Merchant_ID), "Merchant" }, 
                {Guid.Parse(School_ID), "School" }, 
                {Guid.Parse(MasterCard_ID), "MasterCard" }, 
            };
            public static Guid OneRegister => Guid.Parse(OneRegister_ID);
            public static Guid Agropreneur => Guid.Parse(Agropreneur_ID);
            public static Guid Merchant => Guid.Parse(Merchant_ID);
            public static Guid School => Guid.Parse(School_ID);

            public static Guid MasterCard => Guid.Parse(MasterCard_ID);
            public static string GetName(Guid id)
            {
                return _organizationList[id];
            }
            public static string GetName(string id)
            {
                if(Guid.TryParse(id, out var organizationId))
                {
                    return _organizationList[organizationId];
                }
                return string.Empty;
            }
            public static List<(Guid Id,string Name)> GetList()
            {
                return _organizationList.Select(o => (o.Key,o.Value)).ToList();
            }
        }
    }
}

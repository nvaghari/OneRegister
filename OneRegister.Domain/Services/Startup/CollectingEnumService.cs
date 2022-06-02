using OneRegister.Core.Model.ControllerResponse;
using OneRegister.Data.Context;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Extentions;
using OneRegister.Domain.Services.EqualityComparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneRegister.Domain.Services.Startup
{
    public class CollectingEnumService
    {
        private readonly OneRegisterContext _context;

        public CollectingEnumService(OneRegisterContext context)
        {
            _context = context;
        }

        public SimpleResponse UpdateEnumList()
        {
            try
            {
                var currentList = _context.CodeLists.ToList();
                var allEnums = GetAllEnums();
                var allList = GetInitList(allEnums);
                var insertList = GetDifferentiationList(currentList, allList);
                if(insertList.Count > 0)
                {
                    _context.AddRange(insertList);
                    _context.SaveChanges();
                }
                return SimpleResponse.Success($"{insertList.Count} new record(s) was synced");
            }
            catch (Exception ex)
            {
                return ex.ToSimpleResponse();
            }

        }

        private static List<CodeList> GetDifferentiationList(List<CodeList> currentList, List<CodeList> allEnumsList)
        {
            if (currentList.Count == 0) return allEnumsList;
            var diffList = allEnumsList.Except(currentList, new CodeListComparer()).ToList();
            foreach (var codeList in diffList)
            {
                if (currentList.Any(l => l.Name == codeList.Name))
                {
                    codeList.Code = currentList.Where(l => l.Name == codeList.Name).Single().Code;
                }
                else
                {
                    codeList.Code = currentList.OrderByDescending(l => l.Code).First().Code + 1;
                    currentList.Add(codeList);
                }
            }

            return diffList;
        }

        private static List<Type> GetAllEnums()
        {
            var dataAssembly = Assembly.Load("OneRegister.Data");
            var enums = dataAssembly.GetTypes()
                .Where(t => t.IsEnum && t.IsPublic)
                .OrderBy(t => t.Name)
                .ToList();
            return enums;
        }
        private static List<CodeList> GetInitList(List<Type> enums)
        {
            List<CodeList> yellowList = new();
            for (int i = 0; i < enums.Count; i++)
            {
                var values = enums[i].GetEnumValues();
                for (int j = 0; j < values.Length; j++)
                {
                    var value = values.GetValue(j);
                    yellowList.Add(new CodeList
                    {
                        Name = enums[i].Name,
                        Value = value.ToString(),
                        Key = (int)value,
                        Code = i + 1
                    });
                }
            }

            return yellowList;
        }
    }
}

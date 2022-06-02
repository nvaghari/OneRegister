using OneRegister.Security.Model;
using System;
using System.Collections.Generic;

namespace OneRegister.Security.Services.Collecting
{
    public interface IPermissionCollector
    {
        List<PermissionAttibuteModel> CollectMethodAttributes(string assemblyName);
        Dictionary<Guid, PermissionAttibuteModel> ValidatePermissionCollection(List<PermissionAttibuteModel> permissions);
    }
}

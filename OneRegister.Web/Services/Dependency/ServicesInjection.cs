using Microsoft.Extensions.DependencyInjection;
using OneRegister.Data.Contract;
using OneRegister.Data.Repository.Generic;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Repository;
using OneRegister.Domain.Services.Account;
using OneRegister.Domain.Services.AgropreneurRegistration;
using OneRegister.Domain.Services.Dms;
using OneRegister.Domain.Services.Email;
using OneRegister.Domain.Services.KYCApi;
using OneRegister.Domain.Services.MasterCard;
using OneRegister.Domain.Services.MerchantRegistration;
using OneRegister.Domain.Services.Settings;
using OneRegister.Domain.Services.Shared;
using OneRegister.Domain.Services.Startup;
using OneRegister.Domain.Services.StudentRegistration;
using OneRegister.Domain.Services.Webhooks;
using OneRegister.Web.Services.Audit;
using OneRegister.Web.Services.Menu.SideMenu;
using OneRegister.Web.Services.ViewService;

namespace OneRegister.Web.Services.Dependency;

public static class ServicesInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ISideMenuService, SideMenuService>();
        services.AddScoped<SeedingService>();
        services.AddScoped<StudentService>();
        services.AddScoped<OrganizationService>();
        services.AddScoped<AuthorizationService>();
        services.AddScoped<UserService>();
        services.AddScoped<StudentImportService>();
        services.AddScoped<SchoolService>();
        services.AddScoped<ClassRoomService>();
        services.AddScoped<HomeRoomService>();
        services.AddScoped<EDuitRepository>();
        services.AddScoped<DmsService>();
        services.AddScoped<MerchantService>();
        services.AddScoped<AgropreneurService>();
        services.AddScoped<GemsRepository>();
        services.AddScoped<CodeListService>();
        services.AddScoped<RoleService>();
        services.AddScoped<PermissionEntityService>();

        services.AddScoped<ViewRenderService>();
        services.AddScoped<AuditService>();
        services.AddScoped<SettingService>();
        services.AddScoped<EmailService>();
        services.AddScoped<MasterCardService>();
        services.AddScoped<MasterCardRepository>();
        services.AddScoped<CollectingEnumService>();
        services.AddScoped<KYCService>();

        services.AddScoped<WebhookAuditAttribute>();
        services.AddSingleton<WebhookLogService>();
        services.AddScoped<WebookService>();

        services.AddScoped(typeof(IOrganizedRepository<>), typeof(OrganizedRepository<>));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IAuthorizedRepository<>), typeof(AuthorizedRepository<>));
        services.AddScoped(typeof(IOrganizationRepository<>), typeof(OrganizationRepository<>));
    }
}

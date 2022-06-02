using Microsoft.Extensions.DependencyInjection;
using OneRegister.Data.Repository.MasterCard;
using OneRegister.Domain.Services.MasterCard;
using OneRegister.Domain.Services.MasterCard.InquiryFactory;
using OneRegister.Domain.Services.MasterCard.JobFactory;
using OneRegister.Domain.Services.RPPApi;

namespace OneRegister.Web.Services.Dependency;

public static class MasterCardJobServicesInjection
{
    public static IServiceCollection RegisterMasterCardJobServices(this IServiceCollection services)
    {
        services.AddScoped<MCTasksJobService>();
        services.AddScoped<AMLService>();
        services.AddScoped<RPPService>();
        services.AddScoped<MasterCardInquiryRepository>();
        services.AddScoped<IMasterCardJob, GetBankAccountInfoJob>();
        services.AddScoped<IMasterCardJob, GetCddActionDvListJob>();
        services.AddScoped<IMasterCardJob, GetSanctionScreeningNamesJob>();
        services.AddScoped<IMasterCardJob, GetCddActionIvListJob>();
        services.AddScoped<MCInquiryService>();
        services.AddScoped<MasterCardTasksRepository>();
        services.AddScoped<IInquirer, BankAccountInquirer>();
        services.AddScoped<IInquirer, DVInquirer>();
        services.AddScoped<IInquirer, IVInquirer>();
        services.AddScoped<IInquirer, SanctionScreeningInquirer>();

        return services;
    }
}

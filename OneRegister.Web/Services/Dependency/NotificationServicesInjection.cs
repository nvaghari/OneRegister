using Microsoft.Extensions.DependencyInjection;
using OneRegister.Domain.Services.NotificationFactory;
using OneRegister.Domain.Services.NotificationFactory.Makers;
using OneRegister.Domain.Services.NotificationFactory.Sender;
using OneRegister.Domain.Services.ScheduleTask.Jobs;

namespace OneRegister.Web.Services.Dependency;

public static class NotificationServicesInjection
{
    public static IServiceCollection RegisterNotificationServices(this IServiceCollection services)
    {
        services.AddScoped<NotificationJobService>();
        services.AddScoped<NotificationService>();

        services.AddScoped<TaskExtractorJob>();

        services.AddScoped<INotificationMaker, CommissionProvidedNotifMaker>();
        services.AddScoped<INotificationMaker, MerchantCompletedNotifMaker>();
        services.AddScoped<INotificationMaker, NewMerchantNotifMaker>();
        services.AddScoped<INotificationMaker, OP1AcceptedNotifMaker>();
        services.AddScoped<INotificationMaker, OP1RejectedNotifMaker>();
        services.AddScoped<INotificationMaker, OP2AcceptedNotifMaker>();
        services.AddScoped<INotificationMaker, OP2RejectedNotifMaker>();
        services.AddScoped<INotificationMaker, RejectedNotifMaker>();
        services.AddScoped<INotificationMaker, RiskAcceptedNotifMaker>();
        services.AddScoped<INotificationMaker, RiskRejectedNotifMaker>();

        services.AddScoped<TaskGrabberJob>();

        services.AddScoped<INotificationSender, EmailSender>();
        return services;
    }
}

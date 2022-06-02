using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneRegister.Domain.Services.ScheduleTask;
using OneRegister.Web.Models.Configuration;
using Quartz;
using Serilog;

namespace OneRegister.Web.Services.Setup;

public static class QuartzConfiguration
{
    public static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        var notificationConfig = configuration.GetSection("Services:NotificationJob").Get<NotificationJobConfigModel>();
        var masterCardConfig = configuration.GetSection("Services:MasterCardInquiriesJob").Get<MasterCardInquiriesConfigModel>();
        var masterCardTaskConfig = configuration.GetSection("Services:MasterCardTasksJob").Get<MasterCardTaskJobConfigModel>();

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            AddNotificationJob(q, notificationConfig);
            AddMasterCardInquiriesJob(q, masterCardConfig);
            AddMasterCardTasksJob(q, masterCardTaskConfig);
        });
        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }
    private static void AddMasterCardTasksJob(IServiceCollectionQuartzConfigurator q, MasterCardTaskJobConfigModel config)
    {
        const string JOBKEY = "MasterCardTasks";
        if (!config.IsEnable)
        {
            Log.Logger.Information($"{JOBKEY} job is not Enable");
            return;
        }
        var taskJob = new JobKey(JOBKEY);
        q.AddJob<MasterCardTasksJob>(o => o.WithIdentity(taskJob));
        q.AddTrigger(options =>
        options.ForJob(taskJob)
        .WithIdentity("MasterCardTasksSchedule")
        .WithSimpleSchedule(o =>
                            o.WithIntervalInMinutes(config.MinutesToCheck)
                            .RepeatForever()
                            .Build()
                            )
                       );
        Log.Logger.Information($"{JOBKEY} job was registered");
    }
    private static void AddMasterCardInquiriesJob(IServiceCollectionQuartzConfigurator q, MasterCardInquiriesConfigModel config)
    {
        const string JOBKEY = "MasterCardInqueries";
        if (!config.IsEnable)
        {
            Log.Logger.Information($"{JOBKEY} job is not Enable");
            return;
        }
        var inquiryJob = new JobKey(JOBKEY);
        q.AddJob<MasterCardInquiriesJob>(o => o.WithIdentity(inquiryJob));
        q.AddTrigger(options =>
        options.ForJob(inquiryJob)
        .WithIdentity("MasterCardInqueriesSchedule")
        .WithSimpleSchedule(o =>
                            o.WithIntervalInMinutes(config.MinutesToCheck)
                            .RepeatForever()
                            .Build()
                            )
                       );
        Log.Logger.Information($"{JOBKEY} job was registered");
    }
    private static void AddNotificationJob(IServiceCollectionQuartzConfigurator q, NotificationJobConfigModel config)
    {
        const string JOBKEY = "Notifications";
        if (!config.IsEnable)
        {
            Log.Logger.Information($"{JOBKEY} job is not Enable");
            return;
        }
        var notifJob = new JobKey(JOBKEY);
        q.AddJob<NotificationDispenserJob>(o => o.WithIdentity(notifJob));
        q.AddTrigger(options =>
                        options.ForJob(notifJob)
                        .WithIdentity("NotificationsSchedule")
                        .WithSimpleSchedule(o =>
                                            o.WithIntervalInMinutes(config.MinutesToCheck)
                                            .RepeatForever()
                                            .Build()
                                            )
                    );
        Log.Logger.Information($"{JOBKEY} job was registered");
    }
}

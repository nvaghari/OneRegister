using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OneRegister.Data.Context;
using OneRegister.Data.Context.MasterCard;
using OneRegister.Data.Contract;

namespace OneRegister.Web.Services.Dependency;

public static class AppContextInjection
{
    public static void RegisterContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OneRegisterContext>(
            o => o.UseSqlServer(
                configuration.GetConnectionString("OneRegisterConnection")
                , b =>
                {
                    b.MigrationsAssembly("OneRegister.Web");
                    b.MigrationsHistoryTable("MigrationHistory", SchemaNames.Application);
                }
                )
            //.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
            );
        //eDuit EF
        services.AddDbContext<EDuitContext>(
            o => o.UseSqlServer(
                configuration.GetConnectionString("eDuitConnection")
                )
            );

        //GEMS Context EF
        services.AddDbContext<GemsContext>(
                o => o.UseSqlServer(
                        configuration.GetConnectionString("GemConnection")
                    )
            );
        //Notification Context EF
        services.AddDbContext<NotificationContext>(
                o => o.UseSqlServer(
                        configuration.GetConnectionString("OneRegisterConnection")
                    )
            );

        //MasterCard Context
        services.AddDbContext<MasterCardContext>(
                o => o.UseSqlServer(
                        configuration.GetConnectionString("GemConnection")
                    )
            );

    }
}

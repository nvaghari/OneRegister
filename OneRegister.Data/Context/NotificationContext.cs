using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Entities.Notification;

namespace OneRegister.Data.Context
{
    public class NotificationContext : DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options): base(options){}


        public DbSet<NotificationJob> NotificationJobs { get; set; }
        public DbSet<NotificationTask> NotificationTasks { get; set; }
    }
}

using Borg.Framework.Services.Background;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Platform.Hangfire
{
    public class HangfireBackgroundRunner : IFireAndForgetWorker, IDeleteJobWorker, IScheduleWorker, IRecurringWorker, IBackgroundRunner
    {

        #region Fire And Forget
        public string Enqueue(Expression<Action> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }

        public string Enqueue<T>(Expression<Func<T, Task>> methodCall)
        {
            return BackgroundJob.Enqueue<T>(methodCall);
        }

        public string Enqueue<T>(Expression<Action<T>> methodCall)
        {
            return BackgroundJob.Enqueue<T>(methodCall);
        }

        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
        #endregion
        #region Delete Job 
        public bool Delete(string jobId)
        {
            return BackgroundJob.Delete(jobId);
        }
        #endregion
        #region Schedule
        public string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule<T>(methodCall, enqueueAt);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule<T>(methodCall, enqueueAt);
        }

        public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule<T>(methodCall, delay);
        }

        public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule<T>(methodCall, delay);
        }

        public string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt)
        {
            return BackgroundJob.Schedule(methodCall, enqueueAt);
        }

        public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }

        public string Schedule(Expression<Action> methodCall, TimeSpan delay)
        {
            return BackgroundJob.Schedule(methodCall, delay);
        }


        #endregion
        #region Recurring
        public void AddOrUpdate(Expression<Action> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(recurringJobId,methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(Expression<Func<T, Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>( methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(Expression<Func<Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(Expression<Func<T, Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(Expression<Func<Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>( methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate(Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate( methodCall, cronExpression, timeZone, queue);
        }

        public void AddOrUpdate<T>(Expression<Action<T>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default")
        {
            RecurringJob.AddOrUpdate<T>( methodCall, cronExpression, timeZone, queue);
        }

        public void RemoveIfExists(string recurringJobId)
        {
            RecurringJob.RemoveIfExists(recurringJobId);
        }

        public void Trigger(string recurringJobId)
        {
            RecurringJob.Trigger(recurringJobId);
        }


        #endregion

        public void Dispose()
        {
            //nothing
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Borg.Framework.Services.Background
{
    public interface IBackgroundRunner : IFireAndForgetWorker, IScheduleWorker, IDeleteJobWorker, IRecurringWorker, IDisposable
    {
    }

    public interface IFireAndForgetWorker
    {
        string Enqueue(Expression<Action> methodCall);

        string Enqueue<T>(Expression<Func<T, Task>> methodCall);

        string Enqueue<T>(Expression<Action<T>> methodCall);

        string Enqueue(Expression<Func<Task>> methodCall);
    }

    public interface IScheduleWorker
    {
        string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);

        string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt);

        string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);

        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);

        string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt);

        string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt);

        string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay);

        string Schedule(Expression<Action> methodCall, TimeSpan delay);
    }

    public interface IDeleteJobWorker
    {
        bool Delete(string jobId);
    }

    public interface IRecurringWorker
    {
        void AddOrUpdate(Expression<Action> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(string recurringJobId, Expression<Func<T, Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(string recurringJobId, Expression<Func<Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(Expression<Func<T, Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(Expression<Func<Task>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(Expression<Func<T, Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(Expression<Func<Task>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(string recurringJobId, Expression<Action<T>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(string recurringJobId, Expression<Action> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(Expression<Action<T>> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate(Expression<Action> methodCall, string cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void AddOrUpdate<T>(Expression<Action<T>> methodCall, Func<string> cronExpression, TimeZoneInfo timeZone = null, string queue = "default");

        void RemoveIfExists(string recurringJobId);

        void Trigger(string recurringJobId);
    }
}
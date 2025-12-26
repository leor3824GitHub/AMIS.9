using System.Linq.Expressions;
using AMIS.Framework.Core.Jobs;

namespace AMIS.Framework.Infrastructure.Jobs;

internal sealed class NoopJobService : IJobService
{
    public bool Delete(string jobId) => false;

    public bool Delete(string jobId, string fromState) => false;

    public string Enqueue(Expression<Action> methodCall) => string.Empty;

    public string Enqueue(string queue, Expression<Func<Task>> methodCall) => string.Empty;

    public string Enqueue(Expression<Func<Task>> methodCall) => string.Empty;

    public string Enqueue<T>(Expression<Action<T>> methodCall) => string.Empty;

    public string Enqueue<T>(Expression<Func<T, Task>> methodCall) => string.Empty;

    public bool Requeue(string jobId) => false;

    public bool Requeue(string jobId, string fromState) => false;

    public string Schedule(Expression<Action> methodCall, TimeSpan delay) => string.Empty;

    public string Schedule(Expression<Func<Task>> methodCall, TimeSpan delay) => string.Empty;

    public string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueAt) => string.Empty;

    public string Schedule(Expression<Func<Task>> methodCall, DateTimeOffset enqueueAt) => string.Empty;

    public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay) => string.Empty;

    public string Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay) => string.Empty;

    public string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueAt) => string.Empty;

    public string Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt) => string.Empty;
}

using Borg;
using Borg.Framework;
using Borg.Framework.EF;
using Borg.Framework.Services.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;


/// <summary>
/// Abstraction over the <see cref="DbContext"/>
/// Used as BaseClass for applicarion db contexts
/// </summary>
public abstract class BaseBorgDbContext : DbContext
{
    protected enum SetUpMode
    {
        None = 0,
        Constructor = 1,
        Configuration = 2
    }

    private const string TrailingDbContext = "DbContext";
    protected ICollection<Func<bool, CancellationToken, Task>> PreSave = new HashSet<Func<bool, CancellationToken, Task>>();
    protected ICollection<Func<bool, CancellationToken, Task>> PostSave = new HashSet<Func<bool, CancellationToken, Task>>();

    public bool EnablePreSave = true;
    public bool EnablePostSave = true;
    protected SetUpMode setUpMode;
    private BorgDbContextConfiguration DbOptions;

    protected BaseBorgDbContext() : base()
    {
        setUpMode = SetUpMode.Configuration;
    }

    protected BaseBorgDbContext([NotNull] DbContextOptions options) : base(options)
    {
        setUpMode = SetUpMode.Constructor;
    }

    private static string ConfigKey(Type type)
    {
        if (type.IsAbstract) throw new InvalidOperationException("Can not configure abstract db context");
        var parts = type.FullName.Split('.');
        var removeTrailingDbContext = parts[parts.Length - 1].EndsWith(TrailingDbContext);
        var output = string.Join(":", parts);
        return removeTrailingDbContext ? output.Substring(0, output.Length - TrailingDbContext.Length) : output;
    }

    public override int SaveChanges()
    {
        PreSaveInternal();
        var result = base.SaveChanges();
        PostSaveInternal();
        return result;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        PreSaveInternal();
        var result = base.SaveChanges(acceptAllChangesOnSuccess);
        PostSaveInternal();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await PreSaveInternalAsync(cancellationToken);
        var result = await base.SaveChangesAsync();
        await PostSaveInternalAsync(cancellationToken);
        return result;
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await PreSaveInternalAsync(cancellationToken);
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        var eventsToRaise = new List<Task>();
        await PostSaveInternalAsync(cancellationToken);
        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
        var configuration = ServiceLocator.Current.GetInstance<IConfiguration>();
        if (setUpMode == SetUpMode.Configuration)
        {
            SetUpConfig(options, configuration, ConfigKey(GetType()));
        };
    }

    private void SetUpConfig(DbContextOptionsBuilder options, IConfiguration configuration, string key = "")
    {
        var confKey = string.IsNullOrWhiteSpace(key) ? ConfigKey(GetType()) : key;
        DbOptions = Configurator<BorgDbContextConfiguration>.Build(configuration, confKey);
        options.UseSqlServer(DbOptions.ConnectionString, opt =>
        {
            if (DbOptions.Overrides.RetryTimes > 0)
            {
                opt.EnableRetryOnFailure(DbOptions.Overrides.RetryTimes, TimeSpan.FromMilliseconds(DbOptions.Overrides.RetryDelayMilliseconds), new int[0]);
            }
            if (DbOptions.Overrides.CommandTimeout > 0)
            {
                opt.CommandTimeout(DbOptions.Overrides.CommandTimeout);
            }
        });
        options.EnableDetailedErrors(DbOptions.Overrides.EnableDetailedErrors);
    }

    private void PostSaveInternal()
    {
        if (EnablePreSave)
        {
            foreach (var post in PostSave)
            {
                AsyncHelpers.RunSync(async () => { await post(true, default).AnyContext(); });
            }
        }
    }

    private void PreSaveInternal()
    {
        if (EnablePreSave)
        {
            foreach (var pre in PreSave)
            {
                AsyncHelpers.RunSync(async () => { await pre(true, default).AnyContext(); });
            }
        }
    }

    private async Task PostSaveInternalAsync(CancellationToken cancellationToken)
    {
        var eventsToRaise = new List<Task>();
        foreach (var post in PostSave)
        {
            eventsToRaise.Add(post(true, cancellationToken));
        }
        await Task.WhenAll(eventsToRaise).AnyContext();
    }

    private async Task PreSaveInternalAsync(CancellationToken cancellationToken)
    {
        if (EnablePreSave)
        {
            foreach (var pre in PreSave)
            {
                await pre(true, cancellationToken).AnyContext();
            }
        }
    }
}
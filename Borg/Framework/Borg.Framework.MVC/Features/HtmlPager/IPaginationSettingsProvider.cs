using Microsoft.Extensions.Options;
using System;

namespace Borg.Framework.MVC.Features.HtmlPager
{
    public interface IPaginationSettingsProvider
    {
        IPaginationInfoStyle Style { get; }
    }

    public class NullPaginationSettingsProvider : IPaginationSettingsProvider
    {
        private static readonly IPaginationInfoStyle _paginationInfoStyle;

        static NullPaginationSettingsProvider()
        {
            _paginationInfoStyle = new PaginationConfiguration();
        }

        public IPaginationInfoStyle Style => _paginationInfoStyle;
    }

    //public class FactoryPaginationSettingsProvider<TSettings> : IPaginationSettingsProvider where TSettings : IPaginationInfoStyle
    //{
    //    private readonly TSettings _paginationInfoStyle;

    //    public FactoryPaginationSettingsProvider(Func<TSettings> factory)
    //    {
    //        _paginationInfoStyle = factory.Invoke();
    //    }

    //    public IPaginationInfoStyle Style => _paginationInfoStyle;
    //}

    public class InstancePaginationSettingsProvider<TSettings> : IPaginationSettingsProvider where TSettings : IPaginationInfoStyle
    {
        private readonly IOptionsMonitor<TSettings> _paginationInfoStyle;

        public InstancePaginationSettingsProvider(IOptionsMonitor<TSettings> instance)
        {
            _paginationInfoStyle = instance;
        }

        public IPaginationInfoStyle Style => _paginationInfoStyle.CurrentValue;
    }
}
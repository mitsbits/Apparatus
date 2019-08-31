using System;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Assets
{
    public delegate Task AssetCreatedEventHandler<TKey>(AssetCreatedEventArgs<TKey> args) where TKey : IEquatable<TKey>;
}
﻿using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Cms.Contracts
{
  public  interface IModelStore<TModel> where TModel : IIdentifiable
    {
        Task<IEnumerable<TModel>> Items(int page, int rowCount);
        Task<TModel> Item(CompositeKey key);
        Task Delete(params CompositeKey[] keys);
        Task<TModel> Update(TModel model);
        Task<TModel> Create(TModel model);
        Task<TModel> Instance();
    }
}

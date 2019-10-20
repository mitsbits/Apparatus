﻿using Borg.Infrastructure.Core;
using MediatR;

namespace Borg.Framework.SQLServer.ApplicationSettings.Migration
{
    public class CheckForSchemaCommand : IRequest<CheckForSchemaCommandResult>
    {
        public CheckForSchemaCommand(int currnetSchemaVersion)
        {
            CurrnetSchemaVersion = Preconditions.Positive(currnetSchemaVersion, nameof(currnetSchemaVersion));
        }

        public int CurrnetSchemaVersion { get; }
    }
}
﻿// Copyright 2007-2013 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Topshelf.HostConfigurators
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Configurators;
    using Runtime;


    public class StartModeHostConfigurator :
        IHostBuilderConfigurator
    {
        public StartModeHostConfigurator(HostStartMode startMode)
        {
            StartMode = startMode;
        }

        public HostStartMode StartMode { get; private set; }

        public IEnumerable<IValidateResult> Validate()
        {
#if NET35
            if (StartMode == HostStartMode.AutomaticDelayed)
                yield return this.Failure("StartMode", "Automatic (Delayed) is only available on .NET 4.0 or later");
#endif
            yield break;
        }

        public IHostBuilder Configure(IHostBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException("builder");

            builder.Match<InstallBuilder>(x => x.SetStartMode(StartMode));

            return builder;
        }
    }
}
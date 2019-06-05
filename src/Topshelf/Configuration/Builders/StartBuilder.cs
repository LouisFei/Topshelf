// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace Topshelf.Builders
{
    using System;
    using Hosts;
    using Runtime;

    public class StartBuilder :
        IHostBuilder
    {
        readonly IHostBuilder _builder;
        readonly IHostEnvironment _environment;
        readonly IHostSettings _settings;

        public StartBuilder(IHostBuilder builder)
        {
            _builder = GetParentBuilder(builder);
            _settings = builder.Settings;
            _environment = builder.Environment;
        }

        public IHostEnvironment Environment
        {
            get { return _environment; }
        }

        public IHostSettings Settings
        {
            get { return _settings; }
        }

        public IHost Build(IServiceBuilder serviceBuilder)
        {
            if (_builder != null)
            {
                IHost parentHost = _builder.Build(serviceBuilder);

                return new StartHost(_environment, _settings, parentHost);
            }

            return new StartHost(_environment, _settings);
        }

        public void Match<T>(Action<T> callback)
            where T : class, IHostBuilder
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            var self = this as T;
            if (self != null)
            {
                callback(self);
            }
        }

        static IHostBuilder GetParentBuilder(IHostBuilder builder)
        {
            IHostBuilder result = null;

            builder.Match<InstallBuilder>(x => { result = builder; });

            return result;
        }
    }
}
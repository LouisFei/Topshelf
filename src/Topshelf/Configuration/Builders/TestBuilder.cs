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
    using Logging;
    using Runtime;

    public class TestBuilder :
        IHostBuilder
    {
        static readonly LogWriter _log = HostLogger.Get<TestBuilder>();

        readonly IHostEnvironment _environment;
        readonly IHostSettings _settings;

        public TestBuilder(IHostEnvironment environment, IHostSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _environment = environment;
            _settings = settings;
        }

        public IHostEnvironment Environment
        {
            get { return _environment; }
        }

        public IHostSettings Settings
        {
            get { return _settings; }
        }

        public virtual IHost Build(IServiceBuilder serviceBuilder)
        {
            IServiceHandle serviceHandle = serviceBuilder.Build(_settings);

            return CreateHost(serviceHandle);
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

        IHost CreateHost(IServiceHandle serviceHandle)
        {
            _log.Debug("Running as a test host.");
            return new TestHost(_settings, _environment, serviceHandle);
        }
    }
}
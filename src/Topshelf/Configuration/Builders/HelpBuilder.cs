﻿// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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

    public class HelpBuilder :
        IHostBuilder
    {
        readonly IHostEnvironment _environment;
        readonly IHostSettings _settings;
        string _prefixText;
        bool _systemHelpTextOnly;

        public HelpBuilder(IHostEnvironment environment, IHostSettings settings)
        {
            _settings = settings;
            _environment = environment;
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
            string prefixText = _systemHelpTextOnly
                                    ? null
                                    : _prefixText;

            return new HelpHost(prefixText);
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

        public void SetAdditionalHelpText(string prefixText)
        {
            _prefixText = prefixText;
        }

        public void SystemHelpTextOnly()
        {
            _systemHelpTextOnly = true;
        }
    }
}
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
namespace Topshelf.HostConfigurators
{
    using System;
    using CommandLineParser;
    using Options;

    /// <summary>
    /// 命令行开关配置
    /// </summary>
    class CommandLineSwitchConfigurator :
        ICommandLineConfigurator
    {
        readonly Action<bool> _callback;
        readonly string _name;

        /// <summary>
        /// 创建命令行开关配置实例
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public CommandLineSwitchConfigurator(string name, Action<bool> callback)
        {
            _name = name;
            _callback = callback;
        }

        public void Configure(ICommandLineElementParser<IOption> parser)
        {
            parser.Add(from s in parser.Switch(_name)
                       select (IOption)new ServiceSwitchOption(s, _callback));
        }

        /// <summary>
        /// 服务开关参数
        /// </summary>
        class ServiceSwitchOption :
            IOption
        {
            readonly Action<bool> _callback;
            readonly bool _value;

            /// <summary>
            /// 创建服务开关参数实例
            /// </summary>
            /// <param name="element"></param>
            /// <param name="callback"></param>
            public ServiceSwitchOption(ISwitchElement element, Action<bool> callback)
            {
                _callback = callback;
                _value = element.Value;
            }

            /// <summary>
            /// 应用服务开关参数
            /// </summary>
            /// <param name="configurator"></param>
            public void ApplyTo(IHostConfigurator configurator)
            {
                _callback(_value);
            }
        }
    }
}
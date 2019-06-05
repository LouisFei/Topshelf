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
namespace Topshelf.ServiceConfigurators
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Configurators;
    using Runtime;

    /// <summary>
    /// 控制服务配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlServiceConfigurator<T> :
        ServiceConfiguratorBase,
        ServiceConfigurator,
        IConfigurator
        where T : class, ServiceControl
    {
        /// <summary>
        /// 服务工厂
        /// </summary>
        readonly Func<IHostSettings, T> _serviceFactory;

        /// <summary>
        /// 创建控制服务配置
        /// </summary>
        /// <param name="serviceFactory"></param>
        public ControlServiceConfigurator(Func<IHostSettings, T> serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public IEnumerable<IValidateResult> Validate()
        {
            yield break;
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public IServiceBuilder Build()
        {
            var serviceBuilder = new ControlServiceBuilder<T>(_serviceFactory, ServiceEvents);
            return serviceBuilder;
        }
    }
}
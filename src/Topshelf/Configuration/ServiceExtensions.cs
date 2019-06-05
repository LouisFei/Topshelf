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
namespace Topshelf
{
    using System;
    using System.Linq;
    using Builders;
    using Configurators;
    using HostConfigurators;
    using Runtime;
    using ServiceConfigurators;

    /// <summary>
    /// 服务扩展方法
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="configurator">主机配置器</param>
        /// <param name="serviceFactory">服务工厂</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static IHostConfigurator Service<TService>(this IHostConfigurator configurator,
            Func<IHostSettings, TService> serviceFactory,
            Action<ServiceConfigurator> callback)
            where TService : class, ServiceControl
        {
            if (configurator == null)
                throw new ArgumentNullException("configurator");

            ServiceBuilderFactoryDelegate serviceBuilderFactory = CreateServiceBuilderFactory(serviceFactory, callback);

            configurator.UseServiceBuilder(serviceBuilderFactory);

            return configurator;
        }

        /// <summary>
        /// 创建服务生成器工厂
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="serviceFactory">服务工厂</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public static ServiceBuilderFactoryDelegate CreateServiceBuilderFactory<TService>(
            Func<IHostSettings, TService> serviceFactory,
            Action<ServiceConfigurator> callback)
            where TService : class, ServiceControl
        {
            if (serviceFactory == null)
                throw new ArgumentNullException("serviceFactory");
            if (callback == null)
                throw new ArgumentNullException("callback");

            var serviceConfigurator = new ControlServiceConfigurator<TService>(serviceFactory);

            callback(serviceConfigurator);

            ServiceBuilderFactoryDelegate serviceBuilderFactory = x =>
                {
                    IConfigurationResult configurationResult =
                        ValidateConfigurationResult.CompileResults(serviceConfigurator.Validate());
                    if (configurationResult.Results.Any())
                        throw new HostConfigurationException("The service was not properly configured");

                    IServiceBuilder serviceBuilder = serviceConfigurator.Build();

                    return serviceBuilder;
                };
            return serviceBuilderFactory;
        }

        public static IHostConfigurator Service<T>(this IHostConfigurator configurator)
            where T : class, ServiceControl, new()
        {
            return Service(configurator,
                x => new T(), //serviceFactory
                x => { }); //callback
        }

        public static IHostConfigurator Service<T>(this IHostConfigurator configurator, Func<T> serviceFactory)
            where T : class, ServiceControl
        {
            return Service(configurator, x => serviceFactory(), x => { });
        }

        public static IHostConfigurator Service<T>(this IHostConfigurator configurator, Func<T> serviceFactory,
            Action<ServiceConfigurator> callback)
            where T : class, ServiceControl
        {
            return Service(configurator, x => serviceFactory(), callback);
        }

        public static IHostConfigurator Service<T>(this IHostConfigurator configurator,
            Func<IHostSettings, T> serviceFactory)
            where T : class, ServiceControl
        {
            return Service(configurator, serviceFactory, x => { });
        }


        public static IHostConfigurator Service<TService>(this IHostConfigurator configurator,
            Action<ServiceConfigurator<TService>> callback)
            where TService : class
        {
            if (configurator == null)
                throw new ArgumentNullException("configurator");
            
            ServiceBuilderFactoryDelegate serviceBuilderFactory = CreateServiceBuilderFactory(callback);

            configurator.UseServiceBuilder(serviceBuilderFactory);

            return configurator;
        }

        public static ServiceBuilderFactoryDelegate CreateServiceBuilderFactory<TService>(Action<ServiceConfigurator<TService>> callback)
            where TService : class
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            var serviceConfigurator = new DelegateServiceConfigurator<TService>();

            callback(serviceConfigurator);

            ServiceBuilderFactoryDelegate serviceBuilderFactory = x =>
                {
                    IConfigurationResult configurationResult =
                        ValidateConfigurationResult.CompileResults(serviceConfigurator.Validate());
                    if (configurationResult.Results.Any())
                        throw new HostConfigurationException("The service was not properly configured");

                    IServiceBuilder serviceBuilder = serviceConfigurator.Build();

                    return serviceBuilder;
                };
            return serviceBuilderFactory;
        }
    }
}
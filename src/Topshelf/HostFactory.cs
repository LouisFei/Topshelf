// Copyright 2007-2013 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
    using Configurators;
    using HostConfigurators;
    using Logging;


    /// <summary>
    ///   Configure and run a service host using the HostFactory
    ///   使用主机工厂配置和运行一个服务主机。
    /// </summary>
    public static class HostFactory
    {
        /// <summary>
        ///   Configures a new service host
        ///   配置一个新服务主机
        /// </summary>
        /// <param name="configureCallback"> Configuration method to call 主机配置回调方法</param>
        /// <returns> A Topshelf service host, ready to run 返回一个Topshelf服务主机（准备运行）</returns>
        public static Host New(Action<HostConfigurator> configureCallback)
        {
            try
            {
                if (configureCallback == null)
                    throw new ArgumentNullException("configureCallback");

                var configurator = new HostConfiguratorImpl();

                Type declaringType = configureCallback.Method.DeclaringType;
                if (declaringType != null)
                {
                    string defaultServiceName = declaringType.Namespace;
                    if (!string.IsNullOrEmpty(defaultServiceName))
                        configurator.SetServiceName(defaultServiceName);
                }

                configureCallback(configurator);

                configurator.ApplyCommandLine();

                ConfigurationResult result = ValidateConfigurationResult.CompileResults(configurator.Validate());

                if (result.Message.Length > 0)
                {
                    HostLogger.Get(typeof(HostFactory))
                              .InfoFormat("Configuration Result:\n{0}", result.Message);
                }

                return configurator.CreateHost();
            }
            catch (Exception ex)
            {
                HostLogger.Get(typeof(HostFactory)).Error("An exception occurred creating the host", ex);
                HostLogger.Shutdown();
                throw;
            }
        }

        /// <summary>
        ///   Configures and runs a new service host, handling any exceptions and writing them to the log.
        ///   配置并运行一个新的服务主机，所有异常将被捕获并记到日志里。
        /// </summary>
        /// <param name="configureCallback"> Configuration method to call 配置回调方法</param>
        /// <returns> Returns the exit code of the process that should be returned by your application's main method </returns>
        public static TopshelfExitCode Run(Action<HostConfigurator> configureCallback)
        {
            try
            {
                return New(configureCallback)
                    .Run();
            }
            catch (Exception ex)
            {
                HostLogger.Get(typeof(HostFactory))
                          .Error("The service terminated abnormally", ex);
                HostLogger.Shutdown();
                
                return TopshelfExitCode.AbnormalExit;
            }
        }
    }
}
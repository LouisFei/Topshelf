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
    using Runtime;

    /// <summary>
    /// Using the service configuration, the host builder will create the host that will be ran by the service console.
    /// 使用服务配置，主机构建器将创建将由服务控制台运行的主机。
    /// </summary>
    public interface IHostBuilder
    {
        /// <summary>
        /// 主机环境
        /// </summary>
        IHostEnvironment Environment { get; }

        /// <summary>
        /// 主机设置
        /// </summary>
        IHostSettings Settings { get; }

        /// <summary>
        /// 构建主机
        /// </summary>
        /// <param name="serviceBuilder"></param>
        /// <returns></returns>
        IHost Build(IServiceBuilder serviceBuilder);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        void Match<T>(Action<T> callback)
            where T : class, IHostBuilder;
    }
}
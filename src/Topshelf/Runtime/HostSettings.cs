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
namespace Topshelf.Runtime
{
    using System;

    /// <summary>
    ///   The settings that have been configured for the operating system service
    ///   主机（宿主）设置
    /// </summary>
    public interface IHostSettings
    {
        /// <summary>
        ///   服务名称
        ///   The name of the service
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   服务显示名
        ///   The name of the service as it should be displayed in the service control manager
        ///   应在服务控制管理器中显示的服务名称
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        ///   服务描述
        ///   The description of the service that is displayed in the service control manager
        ///   在服务控制管理器中显示的服务的描述
        /// </summary>
        string Description { get; }

        /// <summary>
        ///   服务实例名
        ///   The service instance name that should be used when the service is registered
        ///   服务注册时应该使用的服务实例名
        /// </summary>
        string InstanceName { get; }

        /// <summary>
        ///   Windows服务名
        ///   返回Windows服务名，包括在SCM示例中注册的实例名:myservice$bob
        ///   Returns the Windows service name, including the instance name, which is registered with the SCM Example: myservice$bob
        /// </summary>
        /// <returns> </returns>
        string ServiceName { get; }

        /// <summary>
        ///   True if the service supports pause and continue
        ///   如果服务支持暂停和继续，则为True
        /// </summary>
        bool CanPauseAndContinue { get; }

        /// <summary>
        ///   True if the service can handle the shutdown event
        ///   如果服务可以处理关闭事件，则为True
        /// </summary>
        bool CanShutdown { get; }

        /// <summary>
        /// True if the service handles session change events
        /// 如果服务处理会话更改事件，则为True
        /// </summary>
        bool CanSessionChanged { get; }

        /// <summary>
        /// True if the service handles power change events
        /// 如果服务处理电源更改事件，则为True
        /// </summary>
        bool CanHandlePowerEvent { get; }

        /// <summary>
        /// The amount of time to wait for the service to start before timing out. Default is 10 seconds.
        /// 在超时之前等待服务启动的时间量。默认是10秒。
        /// </summary>
        TimeSpan StartTimeOut { get; }

        /// <summary>
        /// The amount of time to wait for the service to stop before timing out. Default is 10 seconds.
        /// 在超时之前等待服务停止的时间量。默认是10秒。
        /// </summary>
        TimeSpan StopTimeOut { get; }

        /// <summary>
        /// A callback to provide visibility into exceptions while Topshelf is performing its own handling.
        /// 一个回调函数，在Topshelf执行自己的处理时提供对异常的可见性。
        /// </summary>
        Action<Exception> ExceptionCallback { get; }
    }
}
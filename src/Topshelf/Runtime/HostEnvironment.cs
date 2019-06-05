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
    /// Abstracts the environment in which the host in running (different OS versions, platforms, bitness, etc.)
    /// 抽象运行中的主机所处的环境(不同的操作系统版本、平台、位等)。
    /// </summary>
    public interface IHostEnvironment
    {
        string CommandLine { get; }

        /// <summary>
        /// Determines if the service is running as an administrator
        /// 确定服务是否作为管理员运行
        /// </summary>
        bool IsAdministrator { get; }

        /// <summary>
        /// Determines if the process is running as a service
        /// 确定流程是否作为服务运行
        /// </summary>
        bool IsRunningAsAService { get; }

        /// <summary>
        /// Determines if the service is installed
        /// 确定是否安装了服务
        /// </summary>
        /// <param name="serviceName">The name of the service as it is registered</param>
        /// <returns>True if the service is installed, otherwise false</returns>
        bool IsServiceInstalled(string serviceName);

        /// <summary>
        /// Determines if the service is stopped, to prevent a debug instance from being started
        /// 确定是否停止服务，以防止启动调试实例
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        bool IsServiceStopped(string serviceName);

        /// <summary>
        /// Start the service using operating system controls
        /// 使用操作系统控件启动服务
        /// </summary>
        /// <param name="serviceName">The name of the service</param>
        /// <param name="startTimeOut">Waits for the service to reach the running status in the specified time.</param>
        void StartService(string serviceName, TimeSpan startTimeOut);

        /// <summary>
        /// Stop the service using operating system controls
        /// 使用操作系统控件停止服务
        /// </summary>
        /// <param name="serviceName">The name of the service</param>
        /// <param name="stopTimeOut">
        /// Waits for the service to reach the stopeed status in the specified time.
        /// 等待服务在指定的时间内达到停止状态。
        /// </param>
        void StopService(string serviceName, TimeSpan stopTimeOut);

        /// <summary>
        /// Install the service using the settings provided
        /// 使用提供的设置安装服务
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="beforeInstall"> </param>
        /// <param name="afterInstall"> </param>
        /// <param name="beforeRollback"> </param>
        /// <param name="afterRollback"> </param>
        void InstallService(IInstallHostSettings settings, 
            Action<IInstallHostSettings> beforeInstall, 
            Action afterInstall, 
            Action beforeRollback, 
            Action afterRollback);

        /// <summary>
        /// Uninstall the service using the settings provided
        /// 使用提供的设置卸载服务
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="beforeUninstall"></param>
        /// <param name="afterUninstall"></param>
        void UninstallService(IHostSettings settings, Action beforeUninstall, Action afterUninstall);

        /// <summary>
        /// Restarts the service as an administrator which has permission to modify the service configuration
        /// 作为具有修改服务配置权限的管理员重新启动服务
        /// </summary>
        /// <returns>
        /// True if the child process was executed, otherwise false
        /// 如果执行子进程，则为True，否则为false
        /// </returns>
        bool RunAsAdministrator();

        /// <summary>
        /// Create a service host appropriate for the host environment
        /// 创建适合主机环境的服务主机
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="serviceHandle"></param>
        /// <returns></returns>
        IHost CreateServiceHost(IHostSettings settings, IServiceHandle serviceHandle);

        /// <summary>
        /// Send a command to a service to make it do something
        /// 向服务发送命令以使其执行某些操作
        /// </summary>
        /// <param name="serviceName">The service name</param>
        /// <param name="command">The command value</param>
        void SendServiceCommand(string serviceName, int command);
    }
}
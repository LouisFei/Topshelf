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
namespace Topshelf.HostConfigurators
{
    using System;

    /// <summary>
    /// 主机配置器接口。
    /// </summary>
    public interface IHostConfigurator
    {
        /// <summary>
        ///   Specifies the name of the service as it should be displayed in the service control manager
        ///   设置服务的显示名称。将在Windows服务控制管理器中显示。
        /// </summary>
        /// <param name="name"> </param>
        void SetDisplayName(string name);

        /// <summary>
        ///   Specifies the name of the service as it is registered in the service control manager
        ///   设置服务的注册名称。将在Windows服务控制管理器中注册服务。
        /// </summary>
        /// <param name="name"> </param>
        void SetServiceName(string name);

        /// <summary>
        ///   Specifies the description of the service that is displayed in the service control manager
        ///   设置服务的描述。
        /// </summary>
        /// <param name="description"> </param>
        void SetDescription(string description);

        /// <summary>
        ///   Specifies the service instance name that should be used when the service is registered
        ///   指定服务的实例名。
        /// </summary>
        /// <param name="instanceName"> </param>
        void SetInstanceName(string instanceName);

        /// <summary>
        /// Sets the amount of time to wait for the service to start before timing out. Default is 10 seconds.
        /// 设置服务启动前的等待时间，默认为10秒。
        /// </summary>
        /// <param name="startTimeOut"></param>
        void SetStartTimeout(TimeSpan startTimeOut);

        /// <summary>
        /// Sets the amount of time to wait for the service to stop before timing out. Default is 10 seconds.
        /// 设置服务停止前的等待时间。默认为10秒。
        /// </summary>
        /// <param name="stopTimeOut"></param>
        void SetStopTimeout(TimeSpan stopTimeOut);

        /// <summary>
        /// Enable pause and continue support for the service (default is disabled)
        /// 设置支持服务的暂停和继续。（默认为不可用）
        /// </summary>
        void EnablePauseAndContinue();

        /// <summary>
        /// Enable support for service shutdown (signaled by the host OS)
        /// 设置支持服务关闭。
        /// </summary>
        void EnableShutdown();

        /// <summary>
        /// Enabled support for the session changed event
        /// 设置支持会话改变事件。？？不太理解
        /// </summary>
        void EnableSessionChanged();

        /// <summary>
        ///   Specifies the builder factory to use when the service is invoked
        ///   指定调用服务时要使用的构建器工厂
        /// </summary>
        /// <param name="hostBuilderFactory"> </param>
        void UseHostBuilder(HostBuilderFactoryDelegate hostBuilderFactory);

        /// <summary>
        ///   Sets the service builder to use for creating the service
        /// </summary>
        /// <param name="serviceBuilderFactory"> </param>
        void UseServiceBuilder(ServiceBuilderFactoryDelegate serviceBuilderFactory);

        /// <summary>
        ///   Sets the environment builder to use for creating the service (defaults to Windows)
        /// </summary>
        /// <param name="environmentBuilderFactory"> </param>
        void UseEnvironmentBuilder(EnvironmentBuilderFactoryDelegate environmentBuilderFactory);

        /// <summary>
        ///   Adds a a configurator for the host builder to the configurator
        /// </summary>
        /// <param name="configurator"> </param>
        void AddConfigurator(IHostBuilderConfigurator configurator);

        /// <summary>
        /// Parses the command line options and applies them to the host configurator
        /// </summary>
        void ApplyCommandLine();

        /// <summary>
        /// Parses the command line options from the specified command line and applies them to the host configurator.
        /// 解析指定命令行中的命令行选项，并将其应用于主机配置程序。
        /// </summary>
        /// <param name="commandLine"></param>
        void ApplyCommandLine(string commandLine);

        /// <summary>
        /// Adds a command line switch (--name) that can be either true or false. Switches are CASE SeNsITiVe
        /// 添加一个命令行开关(—name)，该开关可以为真也可以为假。开关区分大小写
        /// </summary>
        /// <param name="name">
        /// The name of the switch, as it will appear on the command line
        /// 开关的名称，因为它将出现在命令行中
        /// </param>
        /// <param name="callback"></param>
        void AddCommandLineSwitch(string name, Action<bool> callback);

        /// <summary>
        /// Adds a command line definition (-name:value) that can be specified. the name is case sensitive. If the 
        /// definition 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        void AddCommandLineDefinition(string name, Action<string> callback);

        /// <summary>
        /// Specifies a callback to be run when Topshelf encounters an exception while starting, running
        /// or stopping. This callback does not replace Topshelf's default handling of any exceptions, and 
        /// is intended to allow for local cleanup, logging, etc. This is not required, and is only invoked
        /// if a callback is provided.
        /// </summary>
        /// <param name="callback">The action to run when an exception occurs.</param>
        void OnException(Action<Exception> callback);
    }
}
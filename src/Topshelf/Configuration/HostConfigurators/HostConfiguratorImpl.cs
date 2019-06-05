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
namespace Topshelf.HostConfigurators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Builders;
    using CommandLineParser;
    using Configurators;
    using Logging;
    using Options;
    using Runtime;
    using Runtime.Windows;

    /// <summary>
    /// 主机配置器的具体实现类。
    /// </summary>
    public class HostConfiguratorImpl :
        IHostConfigurator,
        IConfigurator
    {
        readonly IList<ICommandLineConfigurator> _commandLineOptionConfigurators;
        readonly IList<IHostBuilderConfigurator> _configurators;
        /// <summary>
        /// 主机设置
        /// </summary>
        readonly WindowsHostSettings _settings;
        bool _commandLineApplied;
        EnvironmentBuilderFactoryDelegate _environmentBuilderFactory;
        HostBuilderFactoryDelegate _hostBuilderFactory;
        ServiceBuilderFactoryDelegate _serviceBuilderFactory;

        /// <summary>
        /// 返回主机配置具体实现的实例
        /// 默认构造函数
        /// </summary>
        public HostConfiguratorImpl()
        {
            _configurators = new List<IHostBuilderConfigurator>();
            _commandLineOptionConfigurators = new List<ICommandLineConfigurator>();
            _settings = new WindowsHostSettings();

            _environmentBuilderFactory = DefaultEnvironmentBuilderFactory;
            _hostBuilderFactory = DefaultHostBuilderFactory;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IValidateResult> Validate()
        {
            if (_hostBuilderFactory == null)
                yield return this.Failure("HostBuilderFactory", "must not be null");

            if (_serviceBuilderFactory == null)
                yield return this.Failure("ServiceBuilderFactory", "must not be null");

            if (_environmentBuilderFactory == null)
                yield return this.Failure("EnvironmentBuilderFactory", "must not be null");

            if (string.IsNullOrEmpty(_settings.DisplayName) && string.IsNullOrEmpty(_settings.Name))
                yield return this.Failure("DisplayName", "must be specified and not empty");

            if (string.IsNullOrEmpty(_settings.Name))
                yield return this.Failure("Name", "must be specified and not empty");
            else
            {
                var disallowed = new[] {'\t', '\r', '\n', '\\', '/'};
                if (_settings.Name.IndexOfAny(disallowed) >= 0)
                    yield return this.Failure("Name", "must not contain whitespace, '/', or '\\' characters");
            }

            foreach (IValidateResult result in _configurators.SelectMany(x => x.Validate()))
                yield return result;

            yield return this.Success("Name", _settings.Name);

            if (_settings.Name != _settings.DisplayName)
                yield return this.Success("DisplayName", _settings.DisplayName);

            if (_settings.Name != _settings.Description)
                yield return this.Success("Description", _settings.Description);

            if (!string.IsNullOrEmpty(_settings.InstanceName))
                yield return this.Success("InstanceName", _settings.InstanceName);

            yield return this.Success("ServiceName", _settings.ServiceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void SetDisplayName(string name)
        {
            _settings.DisplayName = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void SetServiceName(string name)
        {
            _settings.Name = name;
        }

        public void SetDescription(string description)
        {
            _settings.Description = description;
        }

        public void SetInstanceName(string instanceName)
        {
            _settings.InstanceName = instanceName;
        }

        public void SetStartTimeout(TimeSpan startTimeOut)
        {
          _settings.StartTimeOut = startTimeOut;
        }

        public void SetStopTimeout(TimeSpan stopTimeOut)
        {
          _settings.StopTimeOut = stopTimeOut;
        }

        public void EnablePauseAndContinue()
        {
            _settings.CanPauseAndContinue = true;
        }

        public void EnableShutdown()
        {
            _settings.CanShutdown = true;
        }

        public void EnableSessionChanged()
        {
            _settings.CanSessionChanged = true;
        }

        public void UseHostBuilder(HostBuilderFactoryDelegate hostBuilderFactory)
        {
            _hostBuilderFactory = hostBuilderFactory;
        }

        public void UseServiceBuilder(ServiceBuilderFactoryDelegate serviceBuilderFactory)
        {
            _serviceBuilderFactory = serviceBuilderFactory;
        }

        public void UseEnvironmentBuilder(EnvironmentBuilderFactoryDelegate environmentBuilderFactory)
        {
            _environmentBuilderFactory = environmentBuilderFactory;
        }

        /// <summary>
        /// 添加主机生成配置
        /// </summary>
        /// <param name="configurator"></param>
        public void AddConfigurator(IHostBuilderConfigurator configurator)
        {
            _configurators.Add(configurator);
        }

        #region ApplyCommandLine
        /// <summary>
        /// 应用命令行
        /// </summary>
        public void ApplyCommandLine()
        {
            if (_commandLineApplied)
                return;

            IEnumerable<IOption> options = CommandLine.Parse<IOption>(ConfigureCommandLineParser);
            ApplyCommandLineOptions(options);
        }
        #endregion

        /// <summary>
        /// 解析指定命令行中的命令行选项，并将其应用于主机配置程序。
        /// </summary>
        /// <param name="commandLine"></param>
        public void ApplyCommandLine(string commandLine)
        {
            IEnumerable<IOption> options = CommandLine.Parse<IOption>(ConfigureCommandLineParser, commandLine);
            ApplyCommandLineOptions(options);

            //设置允许命令行执行
            _commandLineApplied = true;
        }

        /// <summary>
        /// 添加一个命令行开关
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void AddCommandLineSwitch(string name, Action<bool> callback)
        {
            var configurator = new CommandLineSwitchConfigurator(name, callback);

            _commandLineOptionConfigurators.Add(configurator);
        }

        public void AddCommandLineDefinition(string name, Action<string> callback)
        {
            var configurator = new CommandLineDefinitionConfigurator(name, callback);

            _commandLineOptionConfigurators.Add(configurator);
        }

        public void OnException(Action<Exception> callback)
        {
            _settings.ExceptionCallback = callback;
        }

        /// <summary>
        /// 创建并返回主机实例
        /// </summary>
        /// <returns></returns>
        public IHost CreateHost()
        {
            Type type = typeof(HostFactory);
            HostLogger.Get<HostConfiguratorImpl>()
                      .InfoFormat("{0} v{1}, .NET Framework v{2}", type.Namespace, type.Assembly.GetName().Version,
                          Environment.Version);

            IEnvironmentBuilder environmentBuilder = _environmentBuilderFactory(this);

            //主机环境
            IHostEnvironment environment = environmentBuilder.Build();

            IServiceBuilder serviceBuilder = _serviceBuilderFactory(_settings);

            IHostBuilder builder = _hostBuilderFactory(environment, _settings);

            foreach (IHostBuilderConfigurator configurator in _configurators)
                builder = configurator.Configure(builder);

            return builder.Build(serviceBuilder);
        }

        void ApplyCommandLineOptions(IEnumerable<IOption> options)
        {
            foreach (IOption option in options)
                option.ApplyTo(this);
        }

        /// <summary>
        /// 配置命令行元素解析器
        /// </summary>
        /// <param name="parser"></param>
        void ConfigureCommandLineParser(ICommandLineElementParser<IOption> parser)
        {
            CommandLineParserOptions.AddTopshelfOptions(parser);

            foreach (ICommandLineConfigurator optionConfigurator in _commandLineOptionConfigurators)
                optionConfigurator.Configure(parser);

            CommandLineParserOptions.AddUnknownOptions(parser);
        }

        static IHostBuilder DefaultHostBuilderFactory(IHostEnvironment environment, IHostSettings settings)
        {
            return new RunBuilder(environment, settings);
        }

        /// <summary>
        /// 默认的环境生成工厂
        /// </summary>
        /// <param name="configurator"></param>
        /// <returns></returns>
        static IEnvironmentBuilder DefaultEnvironmentBuilderFactory(IHostConfigurator configurator)
        {
            return new WindowsHostEnvironmentBuilder(configurator);
        }
    }
}
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
namespace Topshelf.Hosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using Logging;
    using Runtime;

    /// <summary>
    /// 安装主机
    /// </summary>
    public class InstallHost :
        IHost
    {
        static readonly LogWriter _log = HostLogger.Get<InstallHost>();

        readonly IHostEnvironment _environment; //主机环境
        readonly IInstallHostSettings _installSettings; //安装主机设置
        readonly IEnumerable<Action<IInstallHostSettings>> _postActions; //
        readonly IEnumerable<Action<IInstallHostSettings>> _preActions;
        readonly IEnumerable<Action<IInstallHostSettings>> _postRollbackActions;
        readonly IEnumerable<Action<IInstallHostSettings>> _preRollbackActions;
        readonly IHostSettings _settings; //主机设置
        readonly bool _sudo;

        /// <summary>
        /// 创建安装主机实例
        /// </summary>
        /// <param name="environment">主机环境</param>
        /// <param name="settings">主机设置</param>
        /// <param name="startMode">主机启动方式</param>
        /// <param name="dependencies">依赖</param>
        /// <param name="credentials">登录凭证</param>
        /// <param name="preActions"></param>
        /// <param name="postActions"></param>
        /// <param name="preRollbackActions"></param>
        /// <param name="postRollbackActions"></param>
        /// <param name="sudo"></param>
        public InstallHost(IHostEnvironment environment, 
            IHostSettings settings, 
            HostStartMode startMode,
            IEnumerable<string> dependencies,
            Credentials credentials, 
            IEnumerable<Action<IInstallHostSettings>> preActions,
            IEnumerable<Action<IInstallHostSettings>> postActions,
            IEnumerable<Action<IInstallHostSettings>> preRollbackActions,
            IEnumerable<Action<IInstallHostSettings>> postRollbackActions,
            bool sudo)
        {
            _environment = environment;
            _settings = settings;

            _installSettings = new InstallServiceSettingsImpl(settings, credentials, startMode, dependencies.ToArray());

            _preActions = preActions;
            _postActions = postActions;
            _preRollbackActions = preRollbackActions;
            _postRollbackActions = postRollbackActions;
            _sudo = sudo;
        }

        /// <summary>
        /// 安装主机设置
        /// </summary>
        public IInstallHostSettings InstallSettings
        {
            get { return _installSettings; }
        }

        /// <summary>
        /// 主机设置
        /// </summary>
        public IHostSettings Settings
        {
            get { return _settings; }
        }

        public TopshelfExitCode Run()
        {
            if (_environment.IsServiceInstalled(_settings.ServiceName))
            {
                _log.ErrorFormat("The {0} service is already installed.", _settings.ServiceName);
                return TopshelfExitCode.ServiceAlreadyInstalled;
            }

            if (!_environment.IsAdministrator)
            {
                if (_sudo)
                {
                    if (_environment.RunAsAdministrator())
                        return TopshelfExitCode.Ok;
                }

                _log.ErrorFormat("The {0} service can only be installed as an administrator", _settings.ServiceName);
                return TopshelfExitCode.SudoRequired;
            }

            _log.DebugFormat("Attempting to install '{0}'", _settings.ServiceName);

            _environment.InstallService(_installSettings, ExecutePreActions, ExecutePostActions, ExecutePreRollbackActions, ExecutePostRollbackActions);

            return TopshelfExitCode.Ok;
        }

        void ExecutePreActions(IInstallHostSettings settings)
        {
            foreach (Action<IInstallHostSettings> action in _preActions)
            {
                action(_installSettings);
            }
        }

        void ExecutePostActions()
        {
            foreach (Action<IInstallHostSettings> action in _postActions)
            {
                action(_installSettings);
            }
        }

        void ExecutePreRollbackActions()
        {
            foreach (Action<IInstallHostSettings> action in _preRollbackActions)
            {
                action(_installSettings);
            }
        }

        void ExecutePostRollbackActions()
        {
            foreach (Action<IInstallHostSettings> action in _postRollbackActions)
            {
                action(_installSettings);
            }
        }

        #region InstallServiceSettingsImpl
        /// <summary>
        /// 安装服务设置具体实现类
        /// </summary>
        class InstallServiceSettingsImpl :
            IInstallHostSettings
        {
            private Credentials _credentials;
            readonly string[] _dependencies;
            readonly IHostSettings _settings;
            readonly HostStartMode _startMode;

            public InstallServiceSettingsImpl(IHostSettings settings, Credentials credentials, HostStartMode startMode,
                string[] dependencies)
            {
                _credentials = credentials;
                _settings = settings;
                _startMode = startMode;
                _dependencies = dependencies;
            }

            /// <summary>
            /// 服务名
            /// </summary>
            public string Name
            {
                get { return _settings.Name; }
            }

            /// <summary>
            /// 服务显示名
            /// </summary>
            public string DisplayName
            {
                get { return _settings.DisplayName; }
            }

            /// <summary>
            /// 服务描述
            /// </summary>
            public string Description
            {
                get { return _settings.Description; }
            }

            /// <summary>
            /// 服务实例名
            /// </summary>
            public string InstanceName
            {
                get { return _settings.InstanceName; }
            }

            /// <summary>
            /// Windows服务名
            /// </summary>
            public string ServiceName
            {
                get { return _settings.ServiceName; }
            }

            /// <summary>
            /// 表示服务是否支持暂停和继续
            /// </summary>
            public bool CanPauseAndContinue
            {
                get { return _settings.CanPauseAndContinue; }
            }

            /// <summary>
            /// 表示服务是否支持关闭
            /// </summary>
            public bool CanShutdown
            {
                get { return _settings.CanShutdown; }
            }

            /// <summary>
            /// 表示服务是否支持会话改变
            /// </summary>
            public bool CanSessionChanged
            {
                get { return _settings.CanSessionChanged; }
            }

            /// <summary>
            /// True if the service handles power change events
            /// </summary>
            public bool CanHandlePowerEvent
            {
                get { return _settings.CanHandlePowerEvent; }
            }

            /// <summary>
            /// 登录凭证
            /// </summary>
            public Credentials Credentials
            {
                get { return _credentials; }
                set { _credentials = value; }
            }

            /// <summary>
            /// 依赖
            /// </summary>
            public string[] Dependencies
            {
                get { return _dependencies; }
            }

            /// <summary>
            /// 服务启动模式
            /// </summary>
            public HostStartMode StartMode
            {
                get { return _startMode; }
            }

            /// <summary>
            /// 启动超时
            /// </summary>
            public TimeSpan StartTimeOut
            {
              get { return _settings.StartTimeOut; }
            }
            
            /// <summary>
            /// 关闭超时
            /// </summary>
            public TimeSpan StopTimeOut
            {
              get { return _settings.StopTimeOut; }
            }

            /// <summary>
            /// 异常回调
            /// </summary>
            public Action<Exception> ExceptionCallback
            {
                get { return _settings.ExceptionCallback; }
            }
        }
        #endregion
    }
}
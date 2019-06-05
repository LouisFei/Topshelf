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
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using Hosts;
    using Runtime;

    /// <summary>
    /// 安装生成器
    /// </summary>
    public class InstallBuilder :
        IHostBuilder
    {
        readonly IList<string> _dependencies;
        readonly IHostEnvironment _environment;
        readonly IList<Action<IInstallHostSettings>> _postActions;
        readonly IList<Action<IInstallHostSettings>> _preActions;
        readonly IList<Action<IInstallHostSettings>> _postRollbackActions;
        readonly IList<Action<IInstallHostSettings>> _preRollbackActions;
        readonly IHostSettings _settings;
        Credentials _credentials;
        HostStartMode _startMode;
        bool _sudo;

        /// <summary>
        /// 创建安装生成器实例
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="settings"></param>
        public InstallBuilder(IHostEnvironment environment, IHostSettings settings)
        {
            _preActions = new List<Action<IInstallHostSettings>>();
            _postActions = new List<Action<IInstallHostSettings>>();
            _preRollbackActions = new List<Action<IInstallHostSettings>>();
            _postRollbackActions = new List<Action<IInstallHostSettings>>();
            _dependencies = new List<string>();
            _startMode = HostStartMode.Automatic;
            _credentials = new Credentials("", "", ServiceAccount.LocalSystem);

            _environment = environment;
            _settings = settings;
        }

        /// <summary>
        /// 环境
        /// </summary>
        public IHostEnvironment Environment
        {
            get { return _environment; }
        }

        /// <summary>
        /// 主机设置
        /// </summary>
        public IHostSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// 构建主机
        /// </summary>
        /// <param name="serviceBuilder"></param>
        /// <returns></returns>
        public IHost Build(IServiceBuilder serviceBuilder)
        {
            return new InstallHost(_environment, _settings, _startMode, _dependencies.ToArray(), _credentials,
                _preActions, _postActions, _preRollbackActions, _postRollbackActions, _sudo);
        }

        public void Match<T>(Action<T> callback)
            where T : class, IHostBuilder
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            var self = this as T;
            if (self != null)
            {
                callback(self);
            }
        }

        public void RunAs(string username, string password, ServiceAccount accountType)
        {
            _credentials = new Credentials(username, password, accountType);
        }

        public void Sudo()
        {
            _sudo = true;
        }

        /// <summary>
        /// 设置主机启动模式
        /// </summary>
        /// <param name="startMode"></param>
        public void SetStartMode(HostStartMode startMode)
        {
            _startMode = startMode;
        }

        #region 添加主机事件监听
        /// <summary>
        /// 添加主机安装前监听
        /// </summary>
        /// <param name="callback"></param>
        public void BeforeInstall(Action<IInstallHostSettings> callback)
        {
            _preActions.Add(callback);
        }

        public void AfterInstall(Action<IInstallHostSettings> callback)
        {
            _postActions.Add(callback);
        }

        public void BeforeRollback(Action<IInstallHostSettings> callback)
        {
            _preRollbackActions.Add(callback);
        }

        public void AfterRollback(Action<IInstallHostSettings> callback)
        {
            _postRollbackActions.Add(callback);
        }
        #endregion

        public void AddDependency(string name)
        {
            _dependencies.Add(name);
        }
    }
}
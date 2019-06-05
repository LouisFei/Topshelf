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
    /// 控制服务构建器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlServiceBuilder<T> :
        IServiceBuilder
        where T : class, ServiceControl
    {
        readonly ServiceEvents _serviceEvents;
        readonly Func<IHostSettings, T> _serviceFactory;

        /// <summary>
        /// 创建控制服务构建器实例
        /// </summary>
        /// <param name="serviceFactory"></param>
        /// <param name="serviceEvents"></param>
        public ControlServiceBuilder(Func<IHostSettings, T> serviceFactory, ServiceEvents serviceEvents)
        {
            _serviceFactory = serviceFactory;
            _serviceEvents = serviceEvents;
        }

        /// <summary>
        /// 构建服务
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IServiceHandle Build(IHostSettings settings)
        {
            try
            {
                T service = _serviceFactory(settings);

                return new ControlServiceHandle(service, _serviceEvents);
            }
            catch (Exception ex)
            {
                throw new ServiceBuilderException("An exception occurred creating the service: " + typeof(T).Name, ex);
            }
        }

        /// <summary>
        /// 控制服务处理
        /// </summary>
        class ControlServiceHandle :
            IServiceHandle
        {
            readonly T _service;
            readonly ServiceEvents _serviceEvents;

            /// <summary>
            /// 创建控制服务处理实例
            /// </summary>
            /// <param name="service"></param>
            /// <param name="serviceEvents"></param>
            public ControlServiceHandle(T service, ServiceEvents serviceEvents)
            {
                _service = service;
                _serviceEvents = serviceEvents;
            }

            public void Dispose()
            {
                var disposable = _service as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            /// <summary>
            /// 启动服务
            /// </summary>
            /// <param name="hostControl"></param>
            /// <returns></returns>
            public bool Start(IHostControl hostControl)
            {
                _serviceEvents.BeforeStart(hostControl);
                bool started = _service.Start(hostControl);
                if (started)
                    _serviceEvents.AfterStart(hostControl);
                return started;
            }

            /// <summary>
            /// 停止服务
            /// </summary>
            /// <param name="hostControl"></param>
            /// <returns></returns>
            public bool Stop(IHostControl hostControl)
            {
                _serviceEvents.BeforeStop(hostControl);
                bool stopped = _service.Stop(hostControl);
                if (stopped)
                    _serviceEvents.AfterStop(hostControl);
                return stopped;
            }

            /// <summary>
            /// 暂停服务
            /// </summary>
            /// <param name="hostControl"></param>
            /// <returns></returns>
            public bool Pause(IHostControl hostControl)
            {
                var service = _service as ServiceSuspend;

                return service != null && service.Pause(hostControl);
            }

            /// <summary>
            /// 继续服务
            /// </summary>
            /// <param name="hostControl"></param>
            /// <returns></returns>
            public bool Continue(IHostControl hostControl)
            {
                var service = _service as ServiceSuspend;

                return service != null && service.Continue(hostControl);
            }

            /// <summary>
            /// 关闭服务
            /// </summary>
            /// <param name="hostControl"></param>
            public void Shutdown(IHostControl hostControl)
            {
                var serviceShutdown = _service as ServiceShutdown;
                if (serviceShutdown != null)
                {
                    serviceShutdown.Shutdown(hostControl);
                }
            }

            /// <summary>
            /// 会话改变事件处理
            /// </summary>
            /// <param name="hostControl"></param>
            /// <param name="arguments"></param>
            public void SessionChanged(IHostControl hostControl, ISessionChangedArguments arguments)
            {
                var sessionChange = _service as ServiceSessionChange;
                if (sessionChange != null)
                {
                    sessionChange.SessionChange(hostControl, arguments);
                }
            }

            /// <summary>
            /// 电源改变事件处理？？
            /// </summary>
            /// <param name="hostControl"></param>
            /// <param name="arguments"></param>
            /// <returns></returns>
            public bool PowerEvent(IHostControl hostControl, IPowerEventArguments arguments)
            {
                var powerEvent = _service as ServicePowerEvent;
                if (powerEvent != null)
                {
                    return powerEvent.PowerEvent(hostControl, arguments);
                }

                return false;
            }

            /// <summary>
            /// 处理自定义命令
            /// </summary>
            /// <param name="hostControl"></param>
            /// <param name="command"></param>
            public void CustomCommand(IHostControl hostControl, int command)
            {
                var customCommand = _service as ServiceCustomCommand;
                if (customCommand != null)
                {
                    customCommand.CustomCommand(hostControl, command);
                }
            }
        }
    }
}
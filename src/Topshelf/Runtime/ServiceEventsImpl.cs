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
    /// 服务事件集
    /// </summary>
    public class ServiceEventsImpl :
        ServiceEvents
    {
        readonly EventCallbackList<HostStartedContext> _afterStart;
        readonly EventCallbackList<HostStoppedContext> _afterStop;
        readonly EventCallbackList<HostStartContext> _beforeStart;
        readonly EventCallbackList<HostStopContext> _beforeStop;

        /// <summary>
        /// 创建服务事件集
        /// </summary>
        public ServiceEventsImpl()
        {
            _afterStart = new EventCallbackList<HostStartedContext>();
            _afterStop = new EventCallbackList<HostStoppedContext>();
            _beforeStart = new EventCallbackList<HostStartContext>();
            _beforeStop = new EventCallbackList<HostStopContext>();
        }

        #region 事件处理
        public void BeforeStart(IHostControl hostControl)
        {
            var context = new HostStartContextImpl(hostControl);

            _beforeStart.Notify(context);
        }

        public void AfterStart(IHostControl hostControl)
        {
            var context = new HostStartedContextImpl(hostControl);

            _afterStart.Notify(context);
        }

        public void BeforeStop(IHostControl hostControl)
        {
            var context = new HostStopContextImpl(hostControl);

            _beforeStop.Notify(context);
        }

        public void AfterStop(IHostControl hostControl)
        {
            var context = new HostStoppedContextImpl(hostControl);

            _afterStop.Notify(context);
        }
        #endregion

        #region 添加事件回调
        public void AddBeforeStart(Action<HostStartContext> callback)
        {
            _beforeStart.Add(callback);
        }

        public void AddAfterStart(Action<HostStartedContext> callback)
        {
            _afterStart.Add(callback);
        }

        public void AddBeforeStop(Action<HostStopContext> callback)
        {
            _beforeStop.Add(callback);
        }

        public void AddAfterStop(Action<HostStoppedContext> callback)
        {
            _afterStop.Add(callback);
        }
        #endregion

        #region 上下文类定义
        /// <summary>
        /// 上下文抽象基类
        /// </summary>
        abstract class ContextImpl
        {
            readonly IHostControl _hostControl;

            public ContextImpl(IHostControl hostControl)
            {
                _hostControl = hostControl;
            }

            public void RequestAdditionalTime(TimeSpan timeRemaining)
            {
                _hostControl.RequestAdditionalTime(timeRemaining);
            }

            public void Stop()
            {
                _hostControl.Stop();
            }

            public void Restart()
            {
                _hostControl.Restart();
            }
        }

        /// <summary>
        /// 主机启动上下文
        /// </summary>
        class HostStartContextImpl :
            ContextImpl,
            HostStartContext
        {
            public HostStartContextImpl(IHostControl hostControl)
                : base(hostControl)
            {
            }

            /// <summary>
            /// 取消启动
            /// </summary>
            public void CancelStart()
            {
                throw new ServiceControlException("The start service operation was canceled.");
            }
        }

        /// <summary>
        /// 主机已启动上下文
        /// </summary>
        class HostStartedContextImpl :
            ContextImpl,
            HostStartedContext
        {
            public HostStartedContextImpl(IHostControl hostControl)
                : base(hostControl)
            {
            }
        }

        /// <summary>
        /// 主机停止上下文
        /// </summary>
        class HostStopContextImpl :
            ContextImpl,
            HostStopContext
        {
            public HostStopContextImpl(IHostControl hostControl)
                : base(hostControl)
            {
            }
        }

        /// <summary>
        /// 主要已停下上下文
        /// </summary>
        class HostStoppedContextImpl :
            ContextImpl,
            HostStoppedContext
        {
            public HostStoppedContextImpl(IHostControl hostControl)
                : base(hostControl)
            {
            }
        }
        #endregion
    }
}
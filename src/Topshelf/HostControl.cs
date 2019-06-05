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
namespace Topshelf
{
    using System;

    /// <summary>
    /// Allows the service to control the host while running
    /// 允许服务在运行时控制主机
    /// </summary>
    public interface IHostControl
    {
        /// <summary>
        /// Tells the Host that the service is still starting, which resets the timeout.
        /// 告诉主机服务仍在启动，这将重置超时。
        /// </summary>
        void RequestAdditionalTime(TimeSpan timeRemaining);

        /// <summary>
        /// Stops the Host
        /// 停止主机
        /// </summary>
        void Stop();

        /// <summary>
        /// Restarts the Host
        /// 重新启动主机
        /// </summary>
        void Restart();
    }
}
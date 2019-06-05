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
    /// <summary>
    /// 服务事件集
    /// </summary>
    public interface ServiceEvents
    {
        /// <summary>
        /// 服务启动前
        /// </summary>
        /// <param name="hostControl"></param>
        void BeforeStart(IHostControl hostControl);
        /// <summary>
        /// 服务启动后
        /// </summary>
        /// <param name="hostControl"></param>
        void AfterStart(IHostControl hostControl);
        /// <summary>
        /// 服务停止前
        /// </summary>
        /// <param name="hostControl"></param>
        void BeforeStop(IHostControl hostControl);
        /// <summary>
        /// 服务停止后
        /// </summary>
        /// <param name="hostControl"></param>
        void AfterStop(IHostControl hostControl);
    }
}
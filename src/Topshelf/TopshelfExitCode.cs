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
    /// <summary>
    /// 退出代码
    /// </summary>
    public enum TopshelfExitCode
    {
        /// <summary>
        /// 正常
        /// </summary>
        Ok = 0,
        /// <summary>
        /// 非正常退出
        /// </summary>
        AbnormalExit = 1,
        /// <summary>
        /// 需要其它的权限？？
        /// </summary>
        SudoRequired = 2,
        /// <summary>
        /// 服务已经安装
        /// </summary>
        ServiceAlreadyInstalled = 3,
        /// <summary>
        /// 服务没有安装
        /// </summary>
        ServiceNotInstalled = 4,
        /// <summary>
        /// 启动服务失败
        /// </summary>
        StartServiceFailed = 5,
        /// <summary>
        /// 停止服务失败
        /// </summary>
        StopServiceFailed = 6,
        /// <summary>
        /// 服务已经运行
        /// </summary>
        ServiceAlreadyRunning = 7,
        /// <summary>
        /// 未处理的服务异常
        /// </summary>
        UnhandledServiceException = 8,
        /// <summary>
        /// 服务没在运行
        /// </summary>
        ServiceNotRunning = 9,
        /// <summary>
        /// 发送命令失败
        /// </summary>
        SendCommandFailed = 10,
    }
}
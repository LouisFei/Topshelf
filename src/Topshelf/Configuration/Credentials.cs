// Copyright 2007-2011 The Apache Software Foundation.
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
	using System.ServiceProcess;

    /// <summary>
    /// 登录凭证
    /// </summary>
	public class Credentials
	{
        /// <summary>
        /// 创建登录凭证
        /// </summary>
        /// <param name="username">登录用户名</param>
        /// <param name="password">登录密码</param>
        /// <param name="account">登录类型</param>
		public Credentials(string username, string password, ServiceAccount account)
		{
			Username = username;
			Account = account;
			Password = password;
		}

        /// <summary>
        /// 登录用户名
        /// </summary>
		public string Username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
		public string Password { get; set; }
        /// <summary>
        /// 登录类型
        /// </summary>
		public ServiceAccount Account { get; set; }
	}
}
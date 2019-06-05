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
namespace Topshelf.Configurators
{
    using System.Collections.Generic;

    /// <summary>
    /// 配置器接口。
    /// 接口命名建议以“I”为前辍。
    /// </summary>
    public interface IConfigurator
    {
        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        IEnumerable<IValidateResult> Validate();
    }
}
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
namespace Topshelf.CommandLineParser
{
    // Reference to further information
    //
    // http://blogs.msdn.com/lukeh/archive/2007/08/19/monadic-parser-combinators-using-c-3-0.aspx

    /// <summary>
    /// 解析器方法委托（强类型函数指针）
    /// </summary>
    /// <typeparam name="TInput">输入参数的类型</typeparam>
    /// <typeparam name="TResultValue">输出参数值类型</typeparam>
    /// <param name="input">输入参数</param>
    /// <returns></returns>
    delegate Result<TInput, TResultValue> ParserDelegate<TInput, TResultValue>(TInput input);
}
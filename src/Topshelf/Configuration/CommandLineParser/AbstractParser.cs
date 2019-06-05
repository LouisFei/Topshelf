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
    using System.Linq;

    /// <summary>
    /// 解析器抽象基类
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    abstract class AbstractParser<TInput>
    {
        /// <summary>
        /// 直接给定一个可以成功返回的值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public ParserDelegate<TInput, TValue> Succeed<TValue>(TValue value)
        {
            return input => new Result<TInput, TValue>(value, input);
        }

        /// <summary>
        /// 把单值的返回包装成列表值的返回
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ParserDelegate<TInput, TValue[]> Rep<TValue>(ParserDelegate<TInput, TValue> parser)
        {
            return Rep1(parser).Or(Succeed(new TValue[0]));
        }

        /// <summary>
        /// 把单值的返回包装成列表值的返回
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="parser"></param>
        /// <returns></returns>
        public ParserDelegate<TInput, TValue[]> Rep1<TValue>(ParserDelegate<TInput, TValue> parser)
        {
            return from x in parser
                   from xs in Rep(parser)
                   select (new[] {x}).Concat(xs).ToArray();
        }
    }

}
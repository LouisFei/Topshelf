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
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 解析器扩展方法
    /// </summary>
    static class MonadParserExtensions
    {
        public static ParserDelegate<TInput, TValue> Where<TInput, TValue>(this ParserDelegate<TInput, TValue> parser,
            Func<TValue, bool> pred)
        {
            return input =>
                {
                    Result<TInput, TValue> result = parser(input);
                    if (result == null || !pred(result.Value))
                        return null;

                    return result;
                };
        }

        public static ParserDelegate<TInput, TSelect> Select<TInput, TValue, TSelect>(this ParserDelegate<TInput, TValue> parser,
            Func<TValue, TSelect> selector)
        {
            return input =>
                {
                    Result<TInput, TValue> result = parser(input);
                    if (result == null)
                        return null;

                    return new Result<TInput, TSelect>(selector(result.Value), result.Rest);
                };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TIntermediate"></typeparam>
        /// <typeparam name="TSelect"></typeparam>
        /// <param name="parser">一个要投影的值序列</param>
        /// <param name="selector">应用于每个元素的转换函数。</param>
        /// <param name="projector"></param>
        /// <returns></returns>
        public static ParserDelegate<TInput, TSelect> SelectMany<TInput, TValue, TIntermediate, TSelect>(
            this ParserDelegate<TInput, TValue> parser, 
            Func<TValue, ParserDelegate<TInput, TIntermediate>> selector,
            Func<TValue, TIntermediate, TSelect> projector)
        {
            return input =>
                {
                    Result<TInput, TValue> result = parser(input);
                    if (result == null)
                        return null;

                    TValue val = result.Value;
                    Result<TInput, TIntermediate> nextResult = selector(val)(result.Rest);
                    if (nextResult == null)
                        return null;

                    return new Result<TInput, TSelect>(projector(val, nextResult.Value), nextResult.Rest);
                };
        }

        public static ParserDelegate<TInput, TValue> Or<TInput, TValue>(this ParserDelegate<TInput, TValue> first,
            ParserDelegate<TInput, TValue> second)
        {
            return input => first(input) ?? second(input);
        }

        /// <summary>
        /// 选择第一个匹配的
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TResultValue"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ParserDelegate<TInput, TResultValue> FirstMatch<TInput, TResultValue>(this IEnumerable<ParserDelegate<TInput, TResultValue>> options)
        {
            return input =>
                {
                    return options
                        .Select(option => option(input))
                        .Where(result => result != null)
                        .FirstOrDefault();
                };
        }

        public static ParserDelegate<TInput, TSecondValue> And<TInput, TFirstValue, TSecondValue>(
            this ParserDelegate<TInput, TFirstValue> first,
            ParserDelegate<TInput, TSecondValue> second)
        {
            return input => second(first(input).Rest);
        }
    }
}
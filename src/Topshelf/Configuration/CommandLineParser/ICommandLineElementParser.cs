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

    /// <summary>
    ///   Used to configure the command line element parser
    ///   用于配置命令行元素解析器
    /// </summary>
    /// <typeparam name="TResultValue"> The type of object returned as a result of the parse </typeparam>
    interface ICommandLineElementParser<TResultValue>
    {
        /// <summary>
        ///   Adds a new pattern to the parser
        ///   向解析器添加新模式
        /// </summary>
        /// <param name="parser"> The pattern to match and return the resulting object </param>
        void Add(ParserDelegate<IEnumerable<ICommandLineElement>, TResultValue> parser);
        
        ParserDelegate<IEnumerable<ICommandLineElement>, IArgumentElement> Argument();
        ParserDelegate<IEnumerable<ICommandLineElement>, IArgumentElement> Argument(string value);
        ParserDelegate<IEnumerable<ICommandLineElement>, IArgumentElement> Argument(Predicate<IArgumentElement> pred);

        ParserDelegate<IEnumerable<ICommandLineElement>, IDefinitionElement> Definition();
        ParserDelegate<IEnumerable<ICommandLineElement>, IDefinitionElement> Definition(string key);
        ParserDelegate<IEnumerable<ICommandLineElement>, IDefinitionElement> Definitions(params string[] keys);

        ParserDelegate<IEnumerable<ICommandLineElement>, ISwitchElement> Switch();
        ParserDelegate<IEnumerable<ICommandLineElement>, ISwitchElement> Switch(string key);
        ParserDelegate<IEnumerable<ICommandLineElement>, ISwitchElement> Switches(params string[] keys);

        ParserDelegate<IEnumerable<ICommandLineElement>, IArgumentElement> ValidPath();
    }
}
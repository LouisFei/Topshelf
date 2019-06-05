using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Topshelf.Tests2
{
    [TestClass]
    public class StringCommandLineParserTest
    {
        [TestMethod]
        public void TestStringCommandLineParser()
        {
            var parser = new StringCommandLineParser();
            //var result1 = parser.Whitespace("123");
            //var result2 = parser.Whitespace(" 23");
            //var result3 = parser.Whitespace("1 3");

            var result = parser.Char(' ')("123");
            Assert.IsNull(result);

            result = parser.Char(' ')(" 23");
            Assert.IsTrue(result.Rest.Equals("23"));

            result = parser.Char('\t')("\t23");
            Assert.IsTrue(result.Rest.Equals("23"));

            //-------------------
            result = (parser.Char(' ').Or(parser.Char('\t')))("123");
            Assert.IsNull(result);

            result = (parser.Char(' ').Or(parser.Char('\t')))("\t23");
            Assert.IsTrue(result.Rest.Equals("23"));

            result = (parser.Char(' ').Or(parser.Char('\t')))("\t23");
            Assert.IsTrue(result.Rest.Equals("23"));

            //-------------------
            var result2 = parser.Rep(parser.Char('\t'))("\t23");
            Assert.IsTrue(result2.Rest.Equals("23"));

            result2 = parser.Rep(parser.Char(' ').Or(parser.Char('\t')))(" \t123");

            Assert.IsTrue(true);
        }
    }


    class StringCommandLineParser
    {
        public StringCommandLineParser()
        {
            Whitespace = Rep(Char(' ').Or(Char('\t')));
        }

        public ParserDelegate<char[]> Whitespace { get; private set; }

        public ParserDelegate<char> Char(char ch)
        {
            return from a in AnyChar where a == ch select a;
        }

        ParserDelegate<char> AnyChar
        {
            get
            {
                return input => input.Length > 0
                                    ? new Result<char>(input[0], input.Substring(1)) //只会判断第一个字符啊！！！
                                    : null;
            }
        }

        //Result<char> AnyCharFunc(string input)
        //{
        //    return input.Length > 0
        //                    ? new Result<char>(input[0], input.Substring(1))
        //                    : null;
        //}

        /// <summary>
        /// 构造了一个指定了Value值的匿名函数（Lambda表达式）
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public ParserDelegate<TValue> Succeed<TValue>(TValue value)
        {
            //return (string input) => { return new Result<string, TValue>(value, input); };
            //简写
            return input => new Result<TValue>(value, input);
        }

        public ParserDelegate<TValue[]> Rep <TValue>(ParserDelegate<TValue> parser)
        {
            //后面跟了一个空数组，是什么鬼？
            return Rep1(parser).Or(Succeed(new TValue[0]));
        }

        public ParserDelegate<TValue[]> Rep1<TValue>(ParserDelegate<TValue> parser)
        {
            return from a in parser
                   from b in Rep(parser)
                   select (new[] { a }).Concat(b).ToArray();
        }
    }

    static class ParserExtensions
    {
        public static ParserDelegate<TValue> Where<TValue>(this ParserDelegate<TValue> parser,
            Func<TValue, bool> pred)
        {
            return input => {
                Result<TValue> result = parser(input);
                if (result == null || !pred(result.Value))
                {
                    return null;
                }

                return result;
            };
        }

        public static ParserDelegate<TSelect> Select<TValue, TSelect>(this ParserDelegate<TValue> parser,
            Func<TValue, TSelect> selector)
        {
            return input =>
            {
                Result<TValue> result = parser(input);
                if (result == null)
                {
                    return null;
                }
                else
                {
                    return new Result<TSelect>(selector(result.Value), result.Rest);
                }
            };
        }

        public static ParserDelegate<TSelect> SelectMany<TValue, TIntermediate, TSelect>(this ParserDelegate<TValue> parser,
            Func<TValue, ParserDelegate<TIntermediate>> selector,
            Func<TValue, TIntermediate, TSelect> projector)
        {
            return input =>
            {
                Result<TValue> result = parser(input);
                if (result == null)
                {
                    return null;
                }

                TValue val = result.Value;
                Result<TIntermediate> nextResult = selector(val)(result.Rest);
                if (nextResult == null)
                {
                    return null;
                }

                return new Result<TSelect>(projector(val, nextResult.Value), nextResult.Rest);
            };
        }

        public static ParserDelegate<TValue> Or<TValue>(this ParserDelegate<TValue> first,
            ParserDelegate<TValue> second)
        {
            //return (input) => { return first(input) ?? second(input); };
            //简化
            return input => first(input) ?? second(input);
        }
    }

    delegate Result<TValue> ParserDelegate<TValue>(string input);

    class Result<TValue>
    {
        public Result(TValue value, string rest)
        {
            Value = value;
            Rest = rest;
        }

        public TValue Value { get; private set; }

        public string Rest { get; private set; }
    }
}

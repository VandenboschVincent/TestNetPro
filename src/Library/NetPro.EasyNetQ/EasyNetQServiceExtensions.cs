﻿/*
 *  MIT License
 *  
 *  Copyright (c) 2021 LeonKou
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */

using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace System.NetPro
{
    /// <summary>
    /// 
    /// </summary>
    public static class EasyNetQServiceExtensions
    {
        /// <summary>
        /// 启动EasyNetQ
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddEasyNetQ(this IServiceCollection services, IConfiguration configuration)
        {
            // connectionstring format "host=192.168.18.129;virtualHost=/;username=admin;password=123456;timeout=60"
            //1、The first way
            services.AddSingleton(EasyNetQMulti.Instance);
            //2、The second way
            var option = new EasyNetQOption(configuration);
            services.AddSingleton(option);

            var idleBus = new IdleBus<IBus>(TimeSpan.FromSeconds(option.Idle == 0 ? 60 : option.Idle));
            foreach (var item in option.ConnectionString)
            {
                idleBus.Register(item.Key, () =>
                {
                    try
                    {
                        var bus = RabbitHutch.CreateBus(item.Value);
                        return bus;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"{ex}");
                    }
                });
            }
            services.AddSingleton(idleBus);

            return services;
        }
    }
}

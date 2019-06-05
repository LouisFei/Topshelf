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
namespace Topshelf.ServiceConfigurators
{
    using System;
    using Runtime;

    /// <summary>
    /// �����������������
    /// </summary>
    public abstract class ServiceConfiguratorBase
    {
        /// <summary>
        /// �����¼���
        /// </summary>
        protected readonly ServiceEventsImpl ServiceEvents;

        protected ServiceConfiguratorBase()
        {
            ServiceEvents = new ServiceEventsImpl();
        }

        /// <summary>
        /// ��ӷ�������ǰ����
        /// </summary>
        /// <param name="callback"></param>
        public void BeforeStartingService(Action<HostStartContext> callback)
        {
            ServiceEvents.AddBeforeStart(callback);
        }

        /// <summary>
        /// ��ӷ������������
        /// </summary>
        /// <param name="callback"></param>
        public void AfterStartingService(Action<HostStartedContext> callback)
        {
            ServiceEvents.AddAfterStart(callback);
        }

        /// <summary>
        /// ��ӷ���ֹͣǰ����
        /// </summary>
        /// <param name="callback"></param>
        public void BeforeStoppingService(Action<HostStopContext> callback)
        {
            ServiceEvents.AddBeforeStop(callback);
        }

        /// <summary>
        /// ��ӷ���ֹͣ�����
        /// </summary>
        /// <param name="callback"></param>
        public void AfterStoppingService(Action<HostStoppedContext> callback)
        {
            ServiceEvents.AddAfterStop(callback);
        }
    }
}
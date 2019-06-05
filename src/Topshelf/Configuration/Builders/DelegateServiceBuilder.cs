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
namespace Topshelf.Builders
{
    using System;
    using Runtime;

    public class DelegateServiceBuilder<T> :
        IServiceBuilder
        where T : class
    {
        readonly Func<T, IHostControl, bool> _continue;
        readonly Func<T, IHostControl, bool> _pause;
        readonly ServiceEvents _serviceEvents;
        readonly ServiceFactory<T> _serviceFactory;
        readonly Action<T, IHostControl> _shutdown;
        readonly Func<T, IHostControl, bool> _start;
        readonly Func<T, IHostControl, bool> _stop;
        readonly Action<T, IHostControl, ISessionChangedArguments> _sessionChanged;
        readonly Func<T, IHostControl, IPowerEventArguments, bool> _powerEvent;
        readonly Action<T, IHostControl, int> _customCommand;

        public DelegateServiceBuilder(ServiceFactory<T> serviceFactory, Func<T, IHostControl, bool> start,
            Func<T, IHostControl, bool> stop, Func<T, IHostControl, bool> pause, Func<T, IHostControl, bool> @continue,
            Action<T, IHostControl> shutdown, Action<T, IHostControl, ISessionChangedArguments> sessionChanged,
            Func<T, IHostControl, IPowerEventArguments, bool> powerEvent,
            Action<T, IHostControl, int> customCommand, ServiceEvents serviceEvents)
        {
            _serviceFactory = serviceFactory;
            _start = start;
            _stop = stop;
            _pause = pause;
            _continue = @continue;
            _shutdown = shutdown;
            _sessionChanged = sessionChanged;
            _powerEvent = powerEvent;
            _customCommand = customCommand;
            _serviceEvents = serviceEvents;
        }

        public IServiceHandle Build(IHostSettings settings)
        {
            try
            {
                T service = _serviceFactory(settings);

                return new DelegateServiceHandle(service, _start, _stop, _pause, _continue, _shutdown, _sessionChanged, _powerEvent, _customCommand, _serviceEvents);
            }
            catch (Exception ex)
            {
                throw new ServiceBuilderException("An exception occurred creating the service: " + typeof(T).Name, ex);
            }
        }

        class DelegateServiceHandle :
            IServiceHandle
        {
            readonly Func<T, IHostControl, bool> _continue;
            readonly Func<T, IHostControl, bool> _pause;
            readonly T _service;
            readonly ServiceEvents _serviceEvents;
            readonly Action<T, IHostControl> _shutdown;
            readonly Func<T, IHostControl, bool> _start;
            readonly Func<T, IHostControl, bool> _stop;
            readonly Action<T, IHostControl, ISessionChangedArguments> _sessionChanged;
            readonly Func<T, IHostControl, IPowerEventArguments, bool> _powerEvent;
            readonly Action<T, IHostControl, int> _customCommand;

            public DelegateServiceHandle(T service, Func<T, IHostControl, bool> start, Func<T, IHostControl, bool> stop,
                Func<T, IHostControl, bool> pause, Func<T, IHostControl, bool> @continue, Action<T, IHostControl> shutdown,
                Action<T, IHostControl, ISessionChangedArguments> sessionChanged, Func<T, IHostControl, IPowerEventArguments, bool> powerEvent,
                Action<T, IHostControl, int> customCommand, ServiceEvents serviceEvents)
            {
                _service = service;
                _start = start;
                _stop = stop;
                _pause = pause;
                _continue = @continue;
                _shutdown = shutdown;
                _sessionChanged = sessionChanged;
                _powerEvent = powerEvent;
                _customCommand = customCommand;
                _serviceEvents = serviceEvents;
            }

            public void Dispose()
            {
                var disposable = _service as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            public bool Start(IHostControl hostControl)
            {
                _serviceEvents.BeforeStart(hostControl);

                bool started = _start(_service, hostControl);
                if (started)
                {
                    _serviceEvents.AfterStart(hostControl);
                }
                return started;
            }

            public bool Pause(IHostControl hostControl)
            {
                return _pause != null && _pause(_service, hostControl);
            }

            public bool Continue(IHostControl hostControl)
            {
                return _continue != null && _continue(_service, hostControl);
            }

            public bool Stop(IHostControl hostControl)
            {
                _serviceEvents.BeforeStop(hostControl);

                bool stopped = _stop(_service, hostControl);
                if (stopped)
                {
                    _serviceEvents.AfterStop(hostControl);
                }
                return stopped;
            }

            public void Shutdown(IHostControl hostControl)
            {
                if(_shutdown != null)
                    _shutdown(_service, hostControl);
            }

            public void SessionChanged(IHostControl hostControl, ISessionChangedArguments arguments)
            {
                if (_sessionChanged != null)
                    _sessionChanged(_service, hostControl, arguments);
            }

            public bool PowerEvent(IHostControl hostControl, IPowerEventArguments arguments)
            {
                if (_powerEvent != null)
                    return _powerEvent(_service, hostControl, arguments);

                return false;
            }

            public void CustomCommand(IHostControl hostControl, int command)
            {
                if (_customCommand != null)
                    _customCommand(_service, hostControl, command);
            }
        }
    }
}
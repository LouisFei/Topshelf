// Copyright 2007-2010 The Apache Software Foundation.
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
namespace Topshelf.Shelving
{
	using System;
	using System.Reflection;
	using log4net;
	using Magnum.Extensions;
	using Messages;
	using Model;


	public class ShelfServiceController :
		ServiceStateMachine
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(ShelfServiceController));

		readonly AssemblyName[] _assemblyNames;
		readonly Type _bootstrapperType;
		readonly ShelfType _shelfType;

		ShelfReference _reference;

		public ShelfServiceController(string name, ServiceChannel eventChannel, ShelfType shelfType, Type bootstrapperType,
		                              AssemblyName[] assemblyNames)
			: base(name, eventChannel)
		{
			_shelfType = shelfType;
			_bootstrapperType = bootstrapperType;
			_assemblyNames = assemblyNames;
		}


		void Send<T>(T message)
		{
			if (_reference == null || _reference.ShelfChannel == null)
			{
				_log.WarnFormat("Unable to send service message due to null channel, service = {0}, message type = {1}",
				                Name, typeof(T).ToShortTypeName());
				return;
			}

			_reference.ShelfChannel.Send(message);
		}


		protected override void Create(CreateService message)
		{
			Create();
		}

		protected override void Create()
		{
			_log.DebugFormat("[{0}] Creating shelf service", Name);

			_reference = new ShelfReference(Name, _shelfType);

			if (_assemblyNames != null)
				_assemblyNames.Each(_reference.LoadAssembly);

			if (_bootstrapperType != null)
				_reference.Create(_bootstrapperType);
			else
				_reference.Create();
		}

		protected override void ServiceCreated(ServiceCreated message)
		{
			_log.DebugFormat("[{0}] Shelf created at {1} ({2})", Name, message.Address, message.PipeName);

			_reference.CreateShelfChannel(message.Address, message.PipeName);
		}

		protected override void ServiceFaulted(ServiceFault message)
		{
			_log.ErrorFormat("[{0}] Shelf Service Faulted: {1}", Name, message.ExceptionMessage);
		}

		protected override void Start()
		{
			_log.DebugFormat("[{0}] Start", Name);

			Send(new StartService(Name));
		}

		protected override void Stop()
		{
			_log.DebugFormat("[{0}] Stop", Name);

			Send(new StopService(Name));
		}

		protected override void Pause()
		{
			_log.DebugFormat("[{0}] Pause", Name);

			Send(new PauseService(Name));
		}

		protected override void Continue()
		{
			_log.DebugFormat("[{0}] Continue", Name);

			Send(new ContinueService(Name));
		}

		protected override void Unload()
		{
			_log.DebugFormat("[{0}] {1}", Name, "Unload");

			if (_reference != null)
			{
				_reference.Dispose();
				_reference = null;
			}

			Publish<ServiceUnloaded>();
		}
	}
}
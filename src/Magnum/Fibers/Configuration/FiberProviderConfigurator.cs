// Copyright 2007-2008 The Apache Software Foundation.
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
namespace Magnum.Fibers.Configuration
{
	using System;
	using Channels.Configuration.Internal;


	public class FiberProviderConfigurator<T, TKey>
		where T : class
	{
		Func<FiberProvider<TKey>> _configuredProvider;

		public FiberProvider<TKey> GetConfiguredProvider(ChannelConfiguratorConnection connection)
		{
			if (_configuredProvider == null)
				throw new FiberConfigurationException("No provider specified for FiberProvider");

			FiberProvider<TKey> configuredProvider = _configuredProvider();
			connection.AddDisposable(configuredProvider);

			return configuredProvider;
		}

		public FiberProvider<TKey> GetConfiguredProvider<TChannel>(ChannelConfiguratorConnection<TChannel> connection)
		{
			if (_configuredProvider == null)
				throw new FiberConfigurationException("No provider specified for FiberProvider");

			FiberProvider<TKey> configuredProvider = _configuredProvider();
			connection.AddDisposable(configuredProvider);

			return configuredProvider;
		}

		public T ExecuteOnProducerThread()
		{
			_configuredProvider = () => new SharedFiberProvider<TKey>(new SynchronousFiber());

			return this as T;
		}

		public T ExecuteOnThreadPoolFiber()
		{
			_configuredProvider = () => new FiberCache<TKey>(() => new ThreadPoolFiber());

			return this as T;
		}

		public T ExecuteOnSharedFiber()
		{
			_configuredProvider = () => new SharedFiberProvider<TKey>(new ThreadPoolFiber());

			return this as T;
		}

		public T ExecuteOnSharedThread()
		{
			_configuredProvider = () => new SharedFiberProvider<TKey>(new ThreadFiber());

			return this as T;
		}

		public T UseFiberProvider(FiberProvider<TKey> fiberProvider)
		{
			_configuredProvider = () => fiberProvider;

			return this as T;
		}
	}
}
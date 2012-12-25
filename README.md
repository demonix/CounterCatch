Counter Catch
=============

Version: 0.1 Alpha

Introduction
------------

Counter Catch is a tool that can collect and export Windows Performance Counter data. You can easily configure performance counters to monitor and choose the prefered export method (currently only CSV file is supported).
Counter Catch can be used to monitor performance counter data from various counters, instances and hosts. You can then easily aggregate the exported data and perform any custom analysis.

You can write your custom observers with just a few lines of code to export data on different format or to perform real time calculation.

Getting started
---------------

To use CounterCatch you must get the latest source code and compile it using Visual Studio 2012. Then you can configure CounterCatch.Console.config file (see an example below) and run CounterCatch.Console.exe.
Counter Catch will automatically start to monitoring performance counters. To stop it just press Enter on the console.

Configuration
---------------

Configuration is quite self explanatory, basically you must configure the counters, hosts and observers.
Here an example of a configuration file (CounterCatch.Console.config):

	<?xml version="1.0"?>
	<configuration>
	  <configSections>
		<section name="counterCatch" type="CounterCatch.Configurations.CounterCatchSection, CounterCatch.Configurations"/>
		<section name="castle" type="Castle.Windsor.Configuration.AppDomain.CastleSectionHandler, Castle.Windsor" />
	  </configSections>

	  <counterCatch>
		<counters>
		  <add id="CPU" category="Processor Information" name="% Processor Time" instance="*" hostGroup="local"/>
		</counters>
		<hostGroups>
		  <add id="local">
			<hosts>
			  <add name="localhost" />
			</hosts>
		  </add>
		</hostGroups>
	  </counterCatch>
	  <castle>
		<using assembly="CounterCatch.Core" />
		<components>
		  <component id="consoleObserver"
					 service="CounterCatch.Observers.CounterObserver"
					 type="CounterCatch.Observers.CounterConsoleObserver" />
		  <component id="csvObserver"
					 service="CounterCatch.Observers.CounterObserver"
					 type="CounterCatch.Observers.CounterCSVObserver">
			<parameters>
			  <destinationFile>out.csv</destinationFile>
			</parameters>
		  </component>
		</components>
	  </castle>
	</configuration>

Available observers
-------------------

Currently these observers are suported:

- CounterCatch.Observers.CounterConsoleObserver, CounterCatch.Core
- CounterCatch.Observers.CounterCSVObserver, CounterCatch.Core
- CounterCatch.Observers.CounterTraceObserver, CounterCatch.Core

Here the code of the console observer:

    public class CounterConsoleObserver : CounterObserver
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(CounterValue value)
        {
            Console.WriteLine(string.Format("{0} {1} {2}", value.Time, value.Counter, value.Value));
        }
    }

Notes for developers
--------------------

Counter Catch internally use a library, CounterCatch.Core.dll, that you can directly use in your code if you want to self-host the monitoring app. 

Code is written with C# (.NET 4.0) and events are elaborated using .NET Reactive Extensions (Rx).

License
-------

*[MIT License]* 

Copyright (c) 2012 Davide Icardi

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
- The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



[MIT License]: http://opensource.org/licenses/mit-license.php

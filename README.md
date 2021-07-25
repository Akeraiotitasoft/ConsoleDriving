# ConsoleDriving
The Driver pattern for .NET Console Applications\
\
This supports configuration, logging, and dependency injection.\
\
Usage:\
\
Add Akeraiotitasoft.ConsoleDriving.Abstractions to your library class where you implement IDriver.  (If you separate program entry point from Driver).\
Add Akeraiotitasoft.ConsoleDriving to your program entry point e.g. Program.Main(string[] args).


	class Program
	{
		static int Main(string[] args)
		{
			return CreateConsoleDriverBuilder(args).Build().Run();
		}
	
		static IConsoleDriverBuilder CreateConsoleDriverBuilder(string[] args) =>
			ConsoleDriver.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((drivingContext, configurationBuilder) =>
				{
					// base path
					string basePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
	
					configurationBuilder.SetBasePath(basePath);
					configurationBuilder.AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false);
					if (!string.IsNullOrEmpty(drivingContext.ConsoleDrivingEnvironment.EnvironmentName))
					{
						configurationBuilder.AddJsonFile(
							path: $"appsettings.{drivingContext.ConsoleDrivingEnvironment.EnvironmentName}.json",
							optional: true, reloadOnChange: false);
					}
					configurationBuilder.AddEnvironmentVariables("MYPREFIX_");
					configurationBuilder.AddCommandLine(args);
				})
				.ConfigureLogging( logging =>
				{
					logging.AddConsole();
				})
				.ConfigureServices(services =>
				{
					services.AddDriver<Driver>();
				});
	}
	
	class Driver : IDriver
	{
		public async Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken = default)
		{
			return await Task.Run(() =>
			{
				Console.WriteLine("Hello World");
			});
   		}
	}

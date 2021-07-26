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

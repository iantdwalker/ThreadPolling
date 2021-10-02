using System;
using System.Threading.Tasks;

namespace ThreadPolling
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var progress = new Progress<string>();
			progress.ProgressChanged += Progress_ProgressChanged;
			var taskRunner = new TaskRunner(progress);
			Console.WriteLine("Starting a TaskRunner...press any key to cancel.");
			
			await taskRunner.StartAsync();
			
			Console.ReadKey();
			taskRunner.Cancel();
			Console.WriteLine("TaskRunner was cancelled.");
		}

		private static void Progress_ProgressChanged(object sender, string e)
		{
			Console.WriteLine(e);
		}
	}
}

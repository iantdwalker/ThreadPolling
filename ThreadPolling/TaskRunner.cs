using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPolling
{
	public class TaskRunner
	{
		private IProgress<string> _progress;
		private CancellationTokenSource _cancellationTokenSource;
		private int _taskPollingInterval = 5000;
 		
		public TaskRunner(IProgress<string> progress)
		{
			_progress = progress;
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public async Task StartAsync()
		{
			await Task.Factory.StartNew(ProcessAsync, _cancellationTokenSource.Token);
		}

		public void Cancel()
		{
			_cancellationTokenSource.Cancel();
		}

		private async Task ProcessAsync(object taskState)
		{
			var token = (CancellationToken)taskState;
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			while (!token.IsCancellationRequested)
			{
				_progress.Report($"{stopWatch.ElapsedMilliseconds / 1000} seconds have elapsed.");

				// Passing token here allows the Delay to be cancelled if your task gets cancelled.
				await Task.Delay(_taskPollingInterval, token);
			}

			token.ThrowIfCancellationRequested();
		}
	}
}

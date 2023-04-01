namespace Api.Services
{
    public class IntervalTaskHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new(SaveFile, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        public void SaveFile(object state)
        {
            string fileName = "a" + new Random().Next(10000) + ".txt";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", fileName);
            File.Create(path);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

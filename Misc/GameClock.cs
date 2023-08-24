using System.Collections.Concurrent;
using System.Diagnostics;
namespace Agarme_Server.Misc
{
    public class GameClock
    {
        private readonly ConcurrentDictionary<Action, byte> callbacks = new ConcurrentDictionary<Action, byte>();
        private Stopwatch stopwatch;
        private readonly int interval;
        private long lastTickTime;
        private object lockObject = new object();
        private Thread tickThread;
        private CancellationTokenSource cancellationTokenSource;

        public GameClock(int intervalInMilliseconds)
        {
            this.interval = intervalInMilliseconds;
        }

        public void AddCallback(Action callback)
        {
            callbacks.TryAdd(callback, 0);
        }

        public void RemoveCallback(Action callback)
        {
            callbacks.TryRemove(callback, out _);
        }

        public void Start()
        {
            if (stopwatch != null)
            {
                throw new InvalidOperationException("The game clock has already been started.");
            }

            cancellationTokenSource = new CancellationTokenSource();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            lastTickTime = stopwatch.ElapsedMilliseconds;
            tickThread = new Thread(TickThread);
            tickThread.Start();
        }

        public void Stop()
        {
            if (stopwatch == null)
            {
                throw new InvalidOperationException("The game clock has not been started.");
            }

            cancellationTokenSource.Cancel();
            tickThread.Join(); // Wait for the tickThread to finish
            tickThread = null;
            stopwatch.Stop();
            stopwatch = null;
        }

        private void TickThread()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                var currentTickTime = stopwatch.ElapsedMilliseconds;
                if (currentTickTime - lastTickTime >= interval)
                {
                    foreach (var callback in callbacks.Keys)
                    {
                        lock (lockObject)
                        {
                            callback();
                        }
                    }

                    var tickDuration = currentTickTime - lastTickTime;
                    lastTickTime = currentTickTime;

                    // This is where you could add code to keep track of tick durations for performance monitoring.
                    // For example, you could store them in a list or compute an ongoing average.
                    ComputeAverageTickDuration(tickDuration);
                }
                else
                {
                    Thread.Sleep(1); // Sleep for a very short time to prevent high CPU usage
                }
            }
        }

        private void ComputeAverageTickDuration(long tickDuration)
        {
            // Add your implementation here.
        }
    }
}

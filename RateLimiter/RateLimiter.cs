using System;
using System.Threading.Tasks;

public class RateLimiter
{
    private readonly Func<object, Task> _callbackFunc;
    private int _maxCalls;
    private double _period;
    private Queue<DateTime> _historyRequest;
    private object _lock = new object();

    public const int SECOND = 1;
    public const int MINUTE = 60 * SECOND;
    public const int HOUR = 60 * MINUTE;
    public const int DAY = 24 * HOUR;
    
    public RateLimiter(Func<object, Task> func,int maxCalls, double period)
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func), "The function cannot be null.");
        }
        if (maxCalls <= 0){
            throw new ArgumentOutOfRangeException("maxCalls must be greater then 0");

        }
        if (period <= 0) {
            throw new ArgumentOutOfRangeException("period arg must be greater then 0");
        }
        _callbackFunc = func;
        _maxCalls = maxCalls;
        _period = period;
        _historyRequest = new Queue<DateTime>();

    }

    private bool IsRequestExpired ()
    {
        var now = DateTime.UtcNow;
        return (now - _historyRequest.Peek()).TotalSeconds > _period;
    }

    public bool CanExecute()
    {
        lock(_lock)
        {
             while (_historyRequest.Count > 0 && IsRequestExpired())
            {
                _historyRequest.Dequeue();
            }
            Console.WriteLine($"maxCalls : {_maxCalls} and historyCount : {_historyRequest.Count}");
            return _maxCalls > _historyRequest.Count;
        }
        
    }

    private async Task WaitIfNeeded()
    {
        while(true)
        {
            lock(_lock)
            {
                if (CanExecute())
                {
                    // The Task allow execution.
                    break; 
                }
            }
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Need to waiting for rate limit to be available...");
            
            //  The duration of the delay in milliseconds
            await Task.Delay(1000 * SECOND); 
        }
    }

    public async Task Perform(object argument){
        await WaitIfNeeded();
        lock(_lock){
            _historyRequest.Enqueue(DateTime.UtcNow);
        }

        await _callbackFunc(argument);
    }

}
# RateLimiter Class

A C# class to limit how many times a function can be called in a certain period. It works in a thread-safe way, meaning multiple threads can use it at the same time.

## Features
- Limits the number of calls in a given time (e.g., 10 calls per minute).
- Waits if the rate limit is reached.
- Works with multiple threads.

## Constants
- `SECOND`: 1 second.
- `MINUTE`: 60 seconds.
- `HOUR`: 60 minutes.
- `DAY`: 24 hours.

## Constructor
```csharp
public RateLimiter(Func<object, Task> func, int maxCalls, double period)
```

- func: The function you want to limit.
- maxCalls: Max number of calls allowed in the time period.
- period: The time period (in seconds).


Example: 
```csharp
public static async Task SendGetRequestAsync(object url)
{
    // Send GET request to URL
}

// Create a RateLimiter (10 calls per minute)
var rateLimiter = new RateLimiter(SendGetRequestAsync, 10, 60);

// Call the function
await rateLimiter.Perform("https://api.example.com");
```


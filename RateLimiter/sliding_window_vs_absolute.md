
Pros 
1. Control – Sliding window rate limiting helps control requests better over a time frame (like 24 hours). It stops users from sending too many requests just before a reset period.
2.Spread Out Requests – Sliding window avoids the problem in the absolute approach, where users send lots of requests right before and after the reset. Instead, it spreads requests out more evenly over time.
3.Stops Too Many Requests at Once – Sliding window spaces out requests by checking how many were made in the past time frame. This helps stop the system from getting too many requests all at once, keeping things more stable.
4.Good for Real-Time Systems – Sliding window works well for systems like streaming platforms, messaging apps, or live APIs where people are using them all the time.


Cons 
1. Higher Resource Usage – Sliding window rate limiting needs to keep track of timestamps for each request and check them to see if the current request is allowed. This uses a lot of memory and can slow things down. 
2.Inconsistent User Experience – Because the limit is based on a sliding time window, the number of requests allowed can change slightly depending on when the last requests were made. This might confuse users who expect a clear, fixed limit.
3. Concurrency Challenges – Using a sliding window in a multithreaded or distributed system is hard. Developers need to make sure the system is thread-safe while handling timestamps and calculating request limits.



using System.Collections.Concurrent;

namespace Yarp.Extensions.Resilience.RateLimiter;

public class DefaultRateLimiterPolicyProvider : IRateLimiterPolicyProvider
{
    private readonly ConcurrentDictionary<string, RateLimiterPolicy> _policies = new();

    public RateLimiterPolicy GetPolicy(string key)
    {
        if (_policies.TryGetValue(key, out var policy))
        {
            return policy;
        }

        return null;
    }
    
}
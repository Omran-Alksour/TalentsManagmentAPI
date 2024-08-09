﻿namespace Application.Abstractions.Caching;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key, Func<Task<T>> func, CancellationToken cancellationToken) where T : class;

    Task SetAsync<T>(string key,T value, CancellationToken cancellationToken = default) where T : class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);



}


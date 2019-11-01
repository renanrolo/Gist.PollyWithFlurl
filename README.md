# Gist.PollyWithFlurl

```
using Flurl;
using Flurl.Http;
using Polly;
```

### Básic Flurl (without Polly)
```
IEnumerable<WeatherForecast> collection = await _baseUrl.AppendPathSegment("Success")
                                                        .GetJsonAsync<IEnumerable<WeatherForecast>>();
```

### Using Flurl with Polly policy
```
var collection = await Policy.Handle<FlurlHttpException>(ShouldTryAgain)
                             .WaitAndRetryAsync(2, i =>
                             {
                                 Console.WriteLine($"Error on request. Try number '{i}'");
                                 return TimeSpan.FromSeconds(5);
                             })
                             .ExecuteAsync(() =>
                                 _baseUrl.AppendPathSegment("SuccessOnThirdTry")
                                    .GetJsonAsync<IEnumerable<WeatherForecast>>()
                             );
```


```
private static bool ShouldTryAgain(FlurlHttpException flurlHttpException)
{
    switch ((int)flurlHttpException.Call.Response.StatusCode)
    {
        case 500:
            return true;
        default:
            return false;
    }
}
```

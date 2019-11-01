using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Polly;

namespace Gist.PollyWithFlurl
{
    class Program
    {
        private const string _baseUrl = "http://localhost:54409/WeatherForecast/";

        static async Task Main(string[] args)
        {
            await FirstExemple();

            await SecondExemple();
            
            Console.WriteLine();
            Console.WriteLine("#####################################################");
            Console.WriteLine("##################### END ###########################");
            Console.WriteLine("#####################################################");

            Console.ReadKey();
        }

        private static async Task FirstExemple()
        {
            Console.WriteLine("#####################################################");
            Console.WriteLine("#FirstExemple: Simple request (with success) without polly.");

            try
            {
                var collection = await _baseUrl.AppendPathSegment("Success")
                                               .GetJsonAsync<IEnumerable<WeatherForecast>>();

                foreach (var item in collection)
                {
                    WriteAsJson(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task SecondExemple()
        {
            Console.WriteLine("#####################################################");
            Console.WriteLine("#SecondExemple: Simple request (with error TimeOut) with polly.");

            try
            {
                var collection = await Policy.Handle<FlurlHttpException>(ShouldTryAgain)
                                            .WaitAndRetryAsync(2, i =>
                                            {
                                                Console.WriteLine($"Erro on request. Try number '{i}'");
                                                return TimeSpan.FromSeconds(5);
                                            })
                                            .ExecuteAsync(() =>
                                                _baseUrl.AppendPathSegment("SuccessOnThirdTry")
                                                   .GetJsonAsync<IEnumerable<WeatherForecast>>()
                                            );

                foreach (var item in collection)
                {
                    WriteAsJson(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

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

        private static void WriteAsJson(object obj)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj));

        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InStoreApp
{
    public class Authentication
    {
        public static readonly string FetchTokenUri = "https://api.cognitive.microsoft.com/sts/v1.0";
        private string subscriptionKey;
        private string token;
        private Timer accessTokenRenewer;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        public Authentication(string subscriptionKey)
        {
            this.subscriptionKey = subscriptionKey;

            Debug.WriteLine("Authentication: Retrieving token");


            this.token = FetchToken(FetchTokenUri, subscriptionKey).Result;

            Debug.WriteLine("Authentication: Able to retrieve token: " + this.token);

            // renew the token every specfied minutes
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback),
                                           this,
                                           TimeSpan.FromMinutes(RefreshTokenDuration),
                                           TimeSpan.FromMilliseconds(-1));
        }


        public string GetAccessToken()
        {
            return this.token;
        }

        private void RenewAccessToken()
        {
            this.token = FetchToken(FetchTokenUri, this.subscriptionKey).Result;
            Debug.WriteLine("Renewed token.");
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        private async Task<string> FetchToken(string fetchUri, string subscriptionKey)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                try
                {

                }
                catch (Exception e)
                {
                }


                UriBuilder uriBuilder = new UriBuilder(fetchUri);
                uriBuilder.Path += "/issueToken";

                //Debug.WriteLine("First Stop");
                //Stopwatch sw = Stopwatch.StartNew();
                //var delay = Task.Delay(1000).ContinueWith(_ => { sw.Stop(); return sw.ElapsedMilliseconds; });

                HttpResponseMessage result = await client.PostAsync("https://api.cognitive.microsoft.com/sts/v1.0/issueToken", null).ConfigureAwait(false);
                Debug.WriteLine("Status code: " + result.StatusCode);

                //sw = Stopwatch.StartNew();
                //delay = Task.Delay(1000).ContinueWith(_ => { sw.Stop(); return sw.ElapsedMilliseconds; });

                return await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

    }

}

using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using SpotifyWebApi.Model.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace idkApp
{
    class Connection
    {


        private static string _clientId = "c10ef1543b7f42faa52b333a17ac9f24"; //"";
        private static string _secretId = "02adce548aa64ad994ebda25dbad0832"; //"";
        string endURL = "http://localhost:4002";
        Token token;
        public SpotifyWebApi.SpotifyWebApi SpotifyWebApi;

        public async void StartAuth()
        {
            string startURL = "https://accounts.spotify.com/authorize?client_id=" + _clientId + "&response_type=code&redirect_uri=http://localhost:4002&scope=user-read-private user-read-email user-read-birthdate playlist-read-private playlist-modify-private playlist-read-collaborative";
            //Création des URI
            System.Uri startURI = new System.Uri(startURL);
            System.Uri endURI = new System.Uri(endURL);
            //Resultat de la connexion
            string result;

            //Début Web Authentication Broker
            var webAuthenticationResult =
                  await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(
                  Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                  startURI,
                  endURI);

            switch (webAuthenticationResult.ResponseStatus)
            {
                case Windows.Security.Authentication.Web.WebAuthenticationStatus.Success:
                    // Successful authentication. 
                    result = webAuthenticationResult.ResponseData.ToString();

                    Debug.WriteLine("MARCHE");
                    var token = await DoLastLayers(result);
                    this.token = token;
                    SpotifyWebApi = new SpotifyWebApi.SpotifyWebApi(token);
                    Debug.WriteLine("Token Stocké Fin Connection");
                    break;
                case Windows.Security.Authentication.Web.WebAuthenticationStatus.ErrorHttp:
                    // HTTP error. 
                    result = webAuthenticationResult.ResponseErrorDetail.ToString();
                    break;
                default:
                    // Other error.
                    result = webAuthenticationResult.ResponseData.ToString();
                    break;
            }


        }


        private async Task<Token> DoLastLayers(string Data)
        {

            var t = Data.Split('?')[1].Split('&')[0].Split('=')[1];

            Debug.WriteLine(t);

            var req = Utils.ApiUtils.CreateRequest(new Uri("https://accounts.spotify.com/api/token"));


            req.Headers = new WebHeaderCollection
            {
                ["Authorization"] = "Basic " + Utils.ApiUtils.Base64Encode($"{_clientId}:{_secretId}"),
            };

            var postData = "grant_type=authorization_code" +
                          $"&code={t}" +
                          $"&redirect_uri=" + endURL;

            var data = Encoding.ASCII.GetBytes(postData);

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";


            using (var stream = await req.GetRequestStreamAsync())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)await req.GetResponseAsync();

            // Get the stream containing content returned by the server.
            var dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            var reader = new StreamReader(dataStream ?? throw new InvalidOperationException());

            // Read the content.
            var json = reader.ReadToEnd();
            //Création d'un objet Token pour notre token access
            var result = JsonConvert.DeserializeObject<Token>(json);
            //type d'authorisationy
            Debug.WriteLine(result.ToString());
            return result;
        }
    }

}

using SpotifyWebApi.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace idkApp.Utils
{
    class ApiUtils
    {
        //Encode en Bas64 Pour l'envoie dans le header
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //Envoie les données sur l'api
        public static HttpWebRequest CreateRequest(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }
        //Création du HttpClient
        private static HttpClient MakeHttpClient(Token token = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                MediaTypeWithQualityHeaderValue.Parse("application/x-www-form-urlencoded"));

            if (token != null)
                client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token.ToHeaderString());

            return client;
        }
    }
}

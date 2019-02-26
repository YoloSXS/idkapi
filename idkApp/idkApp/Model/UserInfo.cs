using SpotifyWebApi.Model;
using SpotifyWebApi.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idkApp.Model
{
    class UserInfo
    {
        public async Task<PrivateUser> getUserInfo(Token token)
        {
            var api = new SpotifyWebApi.SpotifyWebApi(token);
            var me = await api.UserProfile.GetMe();
            return me;
        }
    }
}

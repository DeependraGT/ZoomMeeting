using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ZoomAPI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        const string ClientId = "Ay9EcDT6TkOxIiNT0MfFuw";
        const string SecretId = "uoXTKxL6ZLGJbUPTN1FEe38ZOaD02Znl";

        public IActionResult OnGet(string code = "")
        {
            string redirectUrl = "https://localhost:44360/Index";
            if (!string.IsNullOrEmpty(code))
            {
                string postUrl = string.Format("/oauth/token?grant_type=authorization_code&code={0}&redirect_uri={1}", code, redirectUrl);

                var client = new RestClient("https://zoom.us");
                var request = new RestRequest(postUrl, Method.POST);
                request.AddHeader("Authorization", "Basic QXk5RWNEVDZUa094SWlOVDBNZkZ1dzp1b1hUS3hMNlpMR0piVVBUTjFGRWUzOFpPYUQwMlpubA==");

                var response = client.Execute<ResponseObj>(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var client2 = new RestClient("https://api.zoom.us");
                    var request2 = new RestRequest("/v2/users/me/meetings", Method.POST);
                    request2.AddHeader("Authorization", "Bearer " + response.Data.access_token);

                    string body = @"{
  "created_at": "2019 - 09 - 05T16: 54:14Z",
  "duration": 60,
  "host_id": "AbcDefGHi",
  "id": 1100000,
  "join_url": "https://zoom.us/j/1100000",
  "settings": {
                        "alternative_hosts": "",
    "approval_type": 2,
    "audio": "both",
    "auto_recording": "local",
    "close_registration": false,
    "cn_meeting": false,
    "enforce_login": false,
    "enforce_login_domains": "",
    
    
    "host_video": true,
    "in_meeting": false,
    "join_before_host": true,
    "mute_upon_entry": false,
    "participant_video": false,
    "registrants_confirmation_email": true,
    "use_pmi": false,
    "waiting_room": false,
    "watermark": false,
    "registrants_email_notification": true
  },
  "start_time": "2019-08-30T22:00:00Z",
  "start_url": "https://zoom.us/s/1100000?iIifQ.wfY2ldlb82SWo3TsR77lBiJjR53TNeFUiKbLyCvZZjw",
  "status": "waiting",
  "timezone": "America/New_York",
  "topic": "API Test",
  "type": 2,
  "uuid": "ng1MzyWNQaObxcf3+Gfm6A=="
}
                ";

                    var finalResponse = client2.Execute(request2);
                }

                return null;
            }
            else
            {

                string url = string.Format("https://zoom.us/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}", ClientId, redirectUrl);
                return Redirect(url);
            }
        }
    }



    public class ResponseObj
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}

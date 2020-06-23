using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Script;
using System.Collections;
using System.Web.Script.Serialization;
using GoogleTranslate.Models;

namespace GoogleTranslate.Controllers
{
    [RoutePrefix("api/Translate")]
    public class GoogleTranslateController : ApiController
    {
        /// <summary>
        /// To convert message from one language to another
        /// </summary>
        /// <param name="message"></param>
        /// <param name="fromCulture"></param>
        /// <param name="toCulture"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Translate")]
        public IHttpActionResult Translate(string message, string fromCulture, string toCulture)
        {
            return Ok(TranslateText(message, fromCulture, toCulture));
        }

        public APIResponse<string> TranslateText(string input, string fromCulture, string toCulture)
        {
            APIResponse<string> response = new APIResponse<string>();

            try
            {
                string url = String.Format(
                    "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                    "en",
                    "ar",
                    Uri.EscapeUriString(input));

                HttpClient httpClient = new HttpClient();

                string result = httpClient.GetStringAsync(url).Result;

                var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);
                var translationItems = jsonData[0];
                string translation = "";

                foreach (object item in translationItems)
                {
                    IEnumerable translationLineObject = item as IEnumerable;
                    IEnumerator translationLineString = translationLineObject.GetEnumerator();
                    translationLineString.MoveNext();
                    translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
                }

                if (translation.Length > 1)
                {
                    translation = translation.Substring(1);
                }

                response.Data = translation;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Data = "";
                response.Message = "Error: " + ex.StackTrace;
            }

            return response;

        }
    }
}
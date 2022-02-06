using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace RazorpaySampleApp
{
    public partial class Payment : System.Web.UI.Page
    {
        public string orderId; // your key ID
        public string paymentLink;
        protected void Page_Load(object sender, EventArgs e)
        {

            string key = "rzp_test_xxxxx";
            string secret = "xxxxxxxxxxx";
            //your order creation request
            var uniqnumber = Guid.NewGuid().ToString("n").Substring(0, 8);
            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", 100); // this amount should be same as transaction amount 
            input.Add("currency", "INR");
            input.Add("first_min_partial_amount", 100);
            input.Add("expire_by", 1691097057);
            input.Add("reference_id", uniqnumber);
            input.Add("description", "Payment for policy no #23456");
            input.Add("callback_url", "http://127.0.0.1:8080/Charge.aspx");
            input.Add("callback_method", "get");
            string jsonData= JsonConvert.SerializeObject(input);
            var output = CreatePaymentLink(key, secret, jsonData);

            JObject responseObject = JObject.Parse(output);
            paymentLink = responseObject["short_url"].ToString();

            try
            {
                orderId = "1234";
            } catch (WebException ex)
            {

                using (WebResponse response = ex.Response)
                {

                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                    using (Stream data = response.GetResponseStream())
                    {

                        string ErrorText = new StreamReader(data).ReadToEnd();
                        Debug.WriteLine(ErrorText);
                    }
                }
            }
        }
        public string CreatePaymentLink(string key, string secret, string jsonData ) {
            string output = string.Empty;
            string message = string.Empty;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string json = string.Empty;
                // Adding Headers for the request
                // Authorizations based on Client API Id and Client Secret Key
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.razorpay.com/v1/payment_links");
                httpWebRequest.ContentType = "application/json";
                //Basic Authorization
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(key + ":" + secret);
                var encodedkey = System.Convert.ToBase64String(plainTextBytes);
                httpWebRequest.Headers["Authorization"] = "Basic " + encodedkey;
                httpWebRequest.Method = "POST";

                //Adding Body in the request
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    json = jsonData;
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                //Receiving a response from the POST API request
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                // Reading the response using a Stream and pusing it into a string variable
                Stream dataStream = httpResponse.GetResponseStream();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    output = result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return output;
        }
    }
}

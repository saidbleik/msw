using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }

        static string InvokeRequestResponseService(string inputSurname) //////////////////////////////////////
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"Surname"},
                                Values = new string[,] {  { inputSurname },  { "value" },  } /////////////////////////////////////
                            }
                        },
                                        },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = ""; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("");



                HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result; ///////////////////
                string result = "";
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = response.Content.ReadAsStringAsync().Result;

                    Console.WriteLine(responseContent);
                }

                return (result);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            textBox2.Text = "";
            string json = InvokeRequestResponseService(textBox1.Text);
            JObject jo = JObject.Parse(json);
            textBox2.Text = jo["Results"]["output1"]["value"]["Values"].First.First.ToString(Newtonsoft.Json.Formatting.None).Replace("\"","");
            this.Cursor = Cursors.Default;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

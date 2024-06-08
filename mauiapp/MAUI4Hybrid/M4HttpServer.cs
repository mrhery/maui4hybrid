using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MAUI4Hybrid
{
    public class M4HttpServer
    {
        static HttpListener server;
        public static Dictionary<string, string> Data = new Dictionary<string, string>();

        public static string Dir = "www";
        static string names = typeof(App).Namespace;
        public static dynamic configure = new ExpandoObject();

        public static bool IsLitening
        {
            get
            {
                return server.IsListening;
            }
        }

        public static bool IsRunning = false;
        public static string Port = "45100";

        public static void Init(string port = "45100")
        {
            server = new HttpListener();
            server.Prefixes.Add("http://127.0.0.1:" + port + @"/");
            server.Prefixes.Add("http://127.0.0.1:" + port + @"/");
            Port = port;

            ReadConfig();
        }

        public static async void Start()
        {
            server.Start();

            var assembly = Assembly.GetExecutingAssembly();

            IsRunning = true;

            while (server.IsListening)
            {
                HttpListenerContext context = server.GetContext();
                HttpListenerResponse response = context.Response;
                HttpListenerRequest request = context.Request;

                string page = context.Request.Url.LocalPath;

                try
                {
                    string[] route = page.Split(new char[] { '/' });
                    string filename = route[route.Length - 1];
                    string[] fpart = filename.Split(new char[] { '.' });
                    string extension = fpart[fpart.Length - 1];

                    page = names + "." + Dir + "." + page.Replace("/", ".");
                    page = page.Replace("..", ".");

                    Stream streeam = assembly.GetManifestResourceStream(page);

                    if (streeam == null)
                    {
                        string n = "File you are looking for is not available. Resource: " + page;
                        byte[] b = Encoding.UTF8.GetBytes(n);

                        response.ContentLength64 = b.Length;
                        Stream ns = response.OutputStream;
                        ns.Write(b, 0, b.Length);
                    }
                    else
                    {
                        response.AddHeader("Access-Control-Allow-Origin", "*");
                        response.AddHeader("Access-Control-Allow-Methods", "*");

                        if (route.Length > 0)
                        {
                            if (route[0] == "device-api")
                            {
                                if (route.Length > 1)
                                {
                                    switch (route[1])
                                    {
                                        case "all-contacts":

                                            break;

                                        case "get-contact":

                                            break;

                                        case "add-contact":

                                            break;
                                    }
                                }
                                else
                                {
                                    string ss1 = "not-found";

                                    byte[] b1 = Encoding.UTF8.GetBytes(ss1);
                                    response.ContentLength64 = b1.Length;

                                    Stream st1 = response.OutputStream;
                                    st1.Write(b1, 0, b1.Length);
                                }
                            }
                            else
                            {
                                switch (extension)
                                {
                                    case "png":
                                    case "jpg":
                                    case "jpeg":
                                    case "eot":
                                    case "svg":
                                    case "woff":
                                    case "woff2":
                                    case "ttf":
                                    case "otf":
                                        //Debug.
                                        byte[] buffer = new byte[8192];
                                        int bytesRead = 1;
                                        List<byte> arrayList = new List<byte>();

                                        while (bytesRead > 0)
                                        {
                                            bytesRead = streeam.Read(buffer, 0, buffer.Length);
                                            arrayList.AddRange(new ArraySegment<byte>(buffer, 0, bytesRead).Array);
                                        }

                                        byte[] buffer2 = arrayList.ToArray();
                                        response.ContentLength64 = buffer2.Length;

                                        Stream output_stream = response.OutputStream;
                                        output_stream.Write(buffer2, 0, buffer2.Length);
                                        break;

                                    default:
                                        Stream s1 = assembly.GetManifestResourceStream(page);
                                        StreamReader r1 = new StreamReader(s1);
                                        string ss1 = RenderText(RenderHTML(r1.ReadToEnd()));

                                        byte[] b1 = Encoding.UTF8.GetBytes(ss1);
                                        response.ContentLength64 = b1.Length;

                                        Stream st1 = response.OutputStream;
                                        st1.Write(b1, 0, b1.Length);
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    string n = e.Message;
                    byte[] b = Encoding.UTF8.GetBytes(n);

                    response.ContentLength64 = b.Length;
                    Stream ns = response.OutputStream;
                    ns.Write(b, 0, b.Length);
                }

                context.Response.Close();
            }
        }

        static string RenderText(string input)
        {
            return Regex.Replace(input, @"\{\{(\w+)\}\}", match =>
            {
                string key = match.Groups[1].Value;
                string value;

                if(Data.TryGetValue(key, out value))
                {
                    return value;
                }

                value = FetchConfigData(key);

                return value ?? match.Value;
            });
        }

        static string RenderHTML(string input)
        {
            return Regex.Replace(input, @"\[\[include:(.+?)\]\]", match =>
            {
                string resourceName = $"{names}.{Dir}.{match.Groups[1].Value.Replace('/', '.').Replace('\\', '.')}";
                string resourceContent = GetEmbeddedResourceContent(resourceName);
                return resourceContent ?? match.Value;
            });
        }

        static string FetchConfigData(string propertyName)
        {
            try
            {
                string[] properties = propertyName.Split('.');
                foreach (var prop in properties)
                {
                    if (configure is ExpandoObject expando && ((IDictionary<string, object>)expando).ContainsKey(prop))
                    {
                        configure = configure[prop];
                    }
                    else
                    {
                        return null;
                    }
                }
                return configure.ToString();
            }
            catch
            {
                return null;
            }
        }

        static void ReadConfig()
        {
            string jsonContent = GetEmbeddedResourceContent(names + "." + Dir + ".configure.json");

            if(jsonContent != null)
            {
                configure = JsonConvert.DeserializeObject<ExpandoObject>(jsonContent);
            }
        }

        static string GetEmbeddedResourceContent(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static void Stop()
        {
            server.Stop();
        }

        public static void Reload()
        {
            Stop();
            Start();
        }
    }
}

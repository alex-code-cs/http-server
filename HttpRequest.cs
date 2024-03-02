using System.Collections.Generic;
using System;

namespace HttpServer{

    class HttpRequest{
        public string url;
        public HttpMethod method;
        public Dictionary<string,string> headers = new Dictionary<string, string>();
        
        public HttpRequest(string request){
            Console.WriteLine(request);
            string[] lines = request.Split('\n');
            method = StringMethodToEnum(lines[0].Split(' ')[0]);
            url = lines[0].Split(' ')[1];
            string httpVersion = lines[0].Split(' ')[2].Trim();
            if (httpVersion != "HTTP/1.1"){
                throw new Exception($"Неподдерживаемая версия HTTP протокола: <{lines[0].Split(' ')[2]}>");
            }
            
            for (int i = 1; i < lines.Length; i++){
                string line = lines[i].Trim();
                if (line.Length != 0) {
                    headers[line.Split(':')[0]] = line.Split(':')[1];
                }
            }
        }

        private HttpMethod StringMethodToEnum(string method){
            Console.WriteLine($"<{method}>");
            switch (method){
                case "DELETE": return HttpMethod.Delete;
                case "GET": return HttpMethod.Get; 
                case "HEAD": return  HttpMethod.Head; 
                case "OPTIONS": return HttpMethod.Options; 
                case "PATCH": return HttpMethod.Patch; 
                case "POST": return HttpMethod.Post; 
                case "PUT": return HttpMethod.Put; 
                case "TRACE": return HttpMethod.Trace; 
                default: throw new Exception("Неизвестный метод");
            }
        }
    }

    enum HttpMethod{
        Delete,
        Get,
        Head,
        Options,
        Patch,
        Post,
        Put,
        Trace
    }
    
}

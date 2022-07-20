using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using FlueFlame.AspNetCore.Common;
using FlueFlame.AspNetCore.Deserialization;
using Microsoft.AspNetCore.Http;

namespace FlueFlame.AspNetCore.Services;

internal class HttpService
{
    private readonly IList<Action<HttpContext>> _setups = new List<Action<HttpContext>>();
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IXmlSerializer _xmlSerializer;
    
    public HttpService(IJsonSerializer jsonSerializer, IXmlSerializer xmlSerializer)
    {
        _jsonSerializer = jsonSerializer;
        _xmlSerializer = xmlSerializer;
    }

    public void ConfigureHttpContext(Action<HttpContext> configure)
    {
        _setups.Add(configure);
    }
    internal void SetupHttpContext(HttpContext context)
    {
        foreach (var setup in _setups) setup(context);
    }
    internal void SetMethod(HttpMethod method)
    {
        ConfigureHttpContext(x=>x.Request.Method = method.ToString());
    }
    
    public void AddXml(object target)
    {
        var xml = _xmlSerializer.SerializeObject(target);
        var bytes = Encoding.UTF8.GetBytes(xml);

        ConfigureHttpContext(context =>
        {
            var stream = context.Request.Body = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            context.Request.ContentType = MimeType.Xml.Value;
            context.Request.Headers.Accept = MimeType.Xml.Value;
            context.Request.ContentLength = xml.Length;
        });
    }
    
    public void AddJson(object target)
    {
        var json = _jsonSerializer.SerializeObject(target);
        var bytes = Encoding.UTF8.GetBytes(json);

        ConfigureHttpContext(context =>
        {
            var stream = context.Request.Body = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            context.Request.ContentType = MimeType.Json.Value;
            context.Request.Headers.Accept = MimeType.Json.Value;
            context.Request.ContentLength = json.Length;
        });
    }

    public void AddText(string text)
    {
        ConfigureHttpContext(context =>
        {
            var stream = context.Request.Body = new MemoryStream();

            var writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();

            stream.Position = 0;

            context.Request.ContentLength = stream.Length;
        });
    }
}
public class HttpResponseBodyHelper
{
        public HttpResponseBodyHelper(HttpResponse response)
        {
            Response = response;
        }
        
        public HttpResponse Response { get; }
        
        public T Read<T>(Func<Stream, T> read)
        {
            if (Response.Body.CanSeek || Response.Body is MemoryStream)
            {
                Response.Body.Position = 0;
            }
            else
            {
                var stream = new MemoryStream();
                Response.Body.CopyTo(stream);
                stream.Position = 0;
                Response.Body = stream;
            }
            
            return read(Response.Body);
        }
        
        public string ReadAsText()
        {
            return Read(x => new StreamReader(x).ReadToEnd());
        }
}
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Specialized;
using System.Net.Http.Headers;

namespace Soda.Ice.Client.Services;

public interface IIceHttpClient
{
    void SetToken(string token);

    Task<T> GetAsync<T>();

    Task<T> PostAsync<T>();

    Task<T> PutAsync<T>();

    Task<T> DeleteAsync<T>();

    IIceHttpClient Body(object body);

    IIceHttpClient Body(string body);

    IIceHttpClient Url(string url);

    IIceHttpClient Create();

    IIceHttpClient Authentication(AuthenticationHeaderValue authenticationHeaderValue);

    IIceHttpClient Authentication(string scheme, string parameter);

    IIceHttpClient Accept(string[] accept);

    IIceHttpClient ContentType(string mediaType, string charSet = "");

    IIceHttpClient File(string path, string name, string fileName);

    IIceHttpClient File(string content, string name);

    IIceHttpClient Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection);

    IIceHttpClient Headers(NameValueCollection headers);

    IIceHttpClient Header(string key, string value);

    IIceHttpClient Query(IEnumerable<KeyValuePair<string, string>> nameValueCollection);

    IIceHttpClient File(IBrowserFile file);
}

#pragma warning restore // 可能返回 null 引用。
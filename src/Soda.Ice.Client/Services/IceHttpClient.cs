using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Soda.Ice.Common.Extensions;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.IO.Compression;
using Soda.Ice.Common;

namespace Soda.Ice.Client.Services;

public class CompressedContent : HttpContent
{
    public enum CompressionMethod
    {
        GZip = 1,
        Deflate = 2
    }

    private readonly HttpContent _originalContent;
    private readonly CompressionMethod _compressionMethod;

    public CompressedContent(HttpContent content, CompressionMethod compressionMethod)
    {
        _originalContent = content ?? throw new ArgumentNullException(nameof(content));
        _compressionMethod = compressionMethod;

        foreach (KeyValuePair<string, IEnumerable<string>> header in _originalContent.Headers)
        {
            Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        Headers.ContentEncoding.Add(_compressionMethod.ToString().ToLowerInvariant());
    }

    protected override bool TryComputeLength(out long length)
    {
        length = -1;
        return false;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        if (_compressionMethod == CompressionMethod.GZip)
        {
            using (var gzipStream = new GZipStream(stream, CompressionMode.Compress, leaveOpen: true))
            {
                await _originalContent.CopyToAsync(gzipStream);
            }
        }
        else if (_compressionMethod == CompressionMethod.Deflate)
        {
            using (var deflateStream = new DeflateStream(stream, CompressionMode.Compress, leaveOpen: true))
            {
                await _originalContent.CopyToAsync(deflateStream);
            }
        }
    }
}

#pragma warning disable // 可能返回 null 引用。

public class IceHttpClient : IIceHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient _client = null!;

    private string[] _accept;
    private NameValueCollection _headers;
    private MediaTypeHeaderValue _mediaType;
    private string _url;
    private AuthenticationHeaderValue _authenticationHeaderValue;
    private HttpContent _httpContent;

    private string Token = string.Empty;

    public void SetToken(string token)
    {
        Token = token;
    }

    //private readonly ILocalStorageService _localStorage;
    public IceHttpClient()
    {
        //Token = SyncLocalStorageService.GetItem<string>(GlobalVars.ClientTokenKey);
        //_localStorage = localStorageService;
    }

    private static readonly string[] DEFAULT_ACCEPT =
    {
            "application/json",
            "text/plain",
            "*/*"
    };

    private static readonly string[] DEFAULT_ACCEPT_ENCODING = { "gzip", "deflate" };

    private static readonly MediaTypeHeaderValue DEFAULT_MEDIATYPE = new MediaTypeHeaderValue("application/json");

    /// <summary>
    /// 启用请求内容gzip压缩 自动使用gzip压缩body并设置Content-Encoding为gzip
    /// </summary>
    public static bool EnableCompress { get; set; } = false;

    /// <summary>
    /// </summary>
    /// <param name="httpClientFactory"> </param>
    public IceHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        _mediaType = DEFAULT_MEDIATYPE;
        _accept = DEFAULT_ACCEPT;
    }

    public IIceHttpClient Accept(string[] accept)
    {
        _accept = accept;
        return this;
    }

    public IIceHttpClient Authentication(AuthenticationHeaderValue authenticationHeaderValue)
    {
        _authenticationHeaderValue = authenticationHeaderValue;
        return this;
    }

    public IIceHttpClient Authentication(string scheme, string parameter)
    {
        _authenticationHeaderValue = new AuthenticationHeaderValue(scheme, parameter);

        return this;
    }

    public IIceHttpClient Body(object body)
    {
        return Body(body.ToJson());
    }

    public IIceHttpClient Body(string body)
    {
        if (string.IsNullOrEmpty(body))
        {
            return this;
        }

        var sc = new StringContent(body);

        if (EnableCompress)
        {
            _httpContent = new CompressedContent(sc, CompressedContent.CompressionMethod.GZip);
            sc.Headers.ContentEncoding.Add("gzip");
        }
        else
        {
            _httpContent = sc;
        }

        //sc.Headers.ContentLength = sc..Length;
        sc.Headers.ContentType = _mediaType;

        return this;
    }

    public IIceHttpClient ContentType(string mediaType, string charSet = "")
    {
        _mediaType = new MediaTypeHeaderValue(mediaType);
        if (!string.IsNullOrEmpty(charSet))
        {
            _mediaType.CharSet = charSet;
        }

        return this;
    }

    public IIceHttpClient Create()
    {
        _client = _httpClientFactory.CreateClient(GlobalVars.ApiBase);

        _httpContent = null;

        return this;
    }

    public async Task<T> DeleteAsync<T>()
    {
        return await RequestAsync<T>(HttpMethod.Delete);
    }

    public IIceHttpClient File(string path, string name, string fileName)
    {
        if (_httpContent == null)
        {
            _httpContent = new MultipartFormDataContent();
        }

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var bac = new StreamContent(stream);
        ((MultipartFormDataContent)_httpContent).Add(bac, name, fileName);

        return this;
    }

    public IIceHttpClient File(IBrowserFile file)
    {
        if (_httpContent == null)
        {
            _httpContent = new MultipartFormDataContent();
        }
        //_httpContent = new MultipartFormDataContent();

        var stream = file.OpenReadStream(maxAllowedSize: 50_000_000L);
        var bac = new StreamContent(stream);
        ((MultipartFormDataContent)_httpContent).Add(bac, "file", file.Name);

        return this;
    }

    public IIceHttpClient File(string content, string name)
    {
        if (_httpContent == null)
        {
            _httpContent = new MultipartFormDataContent();
        }

        ((MultipartFormDataContent)_httpContent).Add(new StringContent(content), name);
        return this;
    }

    public IIceHttpClient Form(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
    {
        if (_httpContent == null)
        {
            _httpContent = new FormUrlEncodedContent(nameValueCollection);
        }

        return this;
    }

    protected virtual async Task<HttpResponseMessage> GetOriginHttpResponse(HttpMethod method)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
        var sw = new Stopwatch();
        try
        {
            using (var requestMessage = new HttpRequestMessage(method, _url))
            {
                switch (method.Method)
                {
                    case "PUT":
                    case "POST":
                        if (_httpContent != null)
                        {
                            requestMessage.Content = _httpContent;
                        }
                        break;
                }

                foreach (var acc in _accept)
                {
                    requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acc));
                }

                foreach (var accenc in DEFAULT_ACCEPT_ENCODING)
                {
                    requestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(accenc));
                }

                if (_authenticationHeaderValue != null)
                {
                    requestMessage.Headers.Authorization = _authenticationHeaderValue;
                }

                if (_headers != null)
                {
                    foreach (string header in _headers.AllKeys)
                    {
                        requestMessage.Headers.Add(header, _headers.Get(header));
                    }
                }

                sw.Start();
                HttpResponseMessage res = null;
                res = await _client.SendAsync(requestMessage);
                httpStatusCode = res.StatusCode;
                sw.Stop();
                return res;
            }
        }
        catch (Exception e)
        {
            //Logger.Error(e.Message);
            throw;
        }
        finally
        {
            //Logger.Info($"{method.Method} {_url} {httpStatusCode} {sw.ElapsedMilliseconds}ms");
        }
    }

    private async Task<T> GetData<T>(HttpResponseMessage res)
    {
        var str = await res.Content.ReadAsStringAsync();

        if (IsStringOrDecimalOrPrimitiveType(typeof(T)))
        {
            return (T)Convert.ChangeType(str, typeof(T));
        }

        return JsonConvert.DeserializeObject<T>(str);
    }

    protected async Task<T> RequestAsync<T>(HttpMethod method)
    {
        using (var res = await GetOriginHttpResponse(method))
        {
            return await GetData<T>(res);
        }
    }

    private bool IsStringOrDecimalOrPrimitiveType(Type t)
    {
        var typename = t.Name;
        return t.IsPrimitive || typename == "String" || typename == "Decimal";
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public async Task<HttpResponseMessage> GetHttpResponseMessageAsync()
    {
        return await GetOriginHttpResponse(HttpMethod.Get);
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public HttpResponseMessage PostHttpResponseMessage()
    {
        return GetOriginHttpResponse(HttpMethod.Post).Result;
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public async Task<HttpResponseMessage> PostHttpResponseMessageAsync()
    {
        return await GetOriginHttpResponse(HttpMethod.Post);
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public HttpResponseMessage PutHttpResponseMessage()
    {
        return GetOriginHttpResponse(HttpMethod.Put).Result;
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public async Task<HttpResponseMessage> PutHttpResponseMessageAsync()
    {
        return await GetOriginHttpResponse(HttpMethod.Put);
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public HttpResponseMessage DeleteHttpResponseMessage()
    {
        return GetOriginHttpResponse(HttpMethod.Delete).Result;
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public async Task<HttpResponseMessage> DeleteHttpResponseMessageAsync()
    {
        return await GetOriginHttpResponse(HttpMethod.Delete);
    }

    /// <summary>
    /// 获取请求的 <see cref="HttpResponseMessage" /> 结果
    /// </summary>
    /// <returns> </returns>
    public HttpResponseMessage GetHttpResponseMessage()
    {
        return GetOriginHttpResponse(HttpMethod.Get).Result;
    }

    public async Task<T> GetAsync<T>()
    {
        return await RequestAsync<T>(HttpMethod.Get);
    }

    public IIceHttpClient Header(string key, string value)
    {
        CheckHeaderIsNull();
        _headers.Add(key, value);
        return this;
    }

    private void CheckHeaderIsNull()
    {
        if (_headers == null)
        {
            _headers = new NameValueCollection();
        }
    }

    public IIceHttpClient Headers(NameValueCollection headers)
    {
        CheckHeaderIsNull();

        foreach (string key in headers.Keys)
        {
            _headers.Add(key, headers.Get(key));
        }

        return this;
    }

    public async Task<T> PostAsync<T>()
    {
        return await RequestAsync<T>(HttpMethod.Post);
    }

    public async Task<T> PutAsync<T>()
    {
        return await RequestAsync<T>(HttpMethod.Put);
    }

    public IIceHttpClient Url(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        _url = url;
        //Token = (_localStorage.GetItemAsStringAsync(GlobalVars.ClientTokenKey)).Result ?? "";
        this.Authentication("Bearer", Token);

        return this;
    }

    public IIceHttpClient Query(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
    {
        if (nameValueCollection.Any())
        {
            _url = _url + "?" + string.Join("&", nameValueCollection.Select(x => x.Key + "=" + x.Value));
        }

        return this;
    }
}

#pragma warning restore // 可能返回 null 引用。
namespace SIM.Base.Services
{
  using System;
  using System.Web;
  using JetBrains.Annotations;
  using Sitecore.Diagnostics.Base;

  public sealed class WebServerConnectionString : ConnectionString
  {
    public WebServerConnectionString([NotNull] string value)
      : base(value)
    {
      var uri = new Uri(value);
      var scheme = uri.Scheme;      
      var isHttp = string.Equals(scheme, "http", StringComparison.OrdinalIgnoreCase);
      var isHttps = string.Equals(scheme, "https", StringComparison.OrdinalIgnoreCase);
      Assert.ArgumentCondition(isHttp || isHttps, nameof(value), $"The {nameof(value)} argument Uri schema is '{scheme}' which does not match supported 'http' and 'https'");
      
      var url = uri.GetLeftPart(UriPartial.Path).TrimEnd('/') + "/";
      var port = uri.Port;
      Assert.ArgumentCondition(url.EndsWith($":{port}/"), nameof(value), $"The {nameof(value)} argument Url port is missing");

      var query = uri.Query;
      Assert.ArgumentNotNullOrEmpty(query, $"{nameof(value)}.{nameof(query)}");

      var queryString = HttpUtility.ParseQueryString(query);
      var token = queryString["token"];
      Assert.ArgumentNotNullOrEmpty(token, $"{nameof(value)}.{nameof(token)}");

      Url = url.TrimEnd('/');
      Token = token;
    }

    public string Token { get; }

    public string Url { get; }
  }
}
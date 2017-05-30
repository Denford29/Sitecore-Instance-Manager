namespace SIM.Adapters
{
  using System;
  using System.Collections.Generic;
  using System.Net;
  using System.Net.Http;
  using System.Threading.Tasks;
  using JetBrains.Annotations;
  using Newtonsoft.Json;
  using Sitecore.Diagnostics.Base;
  using SIM.Base.FileSystem;
  using SIM.Base.Services;

  public sealed class WebServerAdapter
  {
    [NotNull]
    private WebServerConnectionString ConnectionString { get; }

    [NotNull]
    private HttpClient Client { get; }

    public WebServerAdapter([NotNull] WebServerConnectionString connectionString)
    {
      var client = new HttpClient(new HttpClientHandler()
      {
        Proxy = new WebProxy("http://localhost:8888", false),
        UseProxy = true
      });

      var headers = client.DefaultRequestHeaders;
      Assert.IsNotNull(headers);

      // save token to default headers
      headers.Add("Access-Token", $"Bearer CQIv9XTgHcNsNdWvRDJcBhsV9xlJ1kTy5Gkg2iCBzde-nkQK7uK63A");
      headers.Add("Accept-Encoding", "gzip, deflate, sdch, br");
      headers.Add("accept", "application/json");
      headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

      ConnectionString = connectionString;
      Client = client;
    }

    public async Task CreateWebSite([NotNull] string websiteName, [NotNull] FilePath websiteRootDirectoryPath)
    {
      try
      {
        var newSite = new
        {
          name = websiteName,
          physical_path = websiteRootDirectoryPath.FullName,
          bindings = new[]
          {
            new
            {
              //host_name = websiteName,
              port = 810,
              is_https = false,
              ip_address = "*"
            }
          }
        };

        var content = new StringContent(JsonConvert.SerializeObject(newSite));
        var res = await Client.PostAsync($"https://sitecore.australiasoutheast.cloudapp.azure.com:55539/api/webserver/websites", content);
        Assert.IsNotNull(res);

        if (res.StatusCode != HttpStatusCode.Created)
        {
          throw new WebServerAdapterException(res);
        }
      }
      catch (WebServerAdapterException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new WebServerAdapterException(ex);
      }
    }

    public async Task DeleteWebSite([NotNull] string websiteName)
    {
      try
      {
        var res = await Client.GetAsync($"{ConnectionString.Url}/api/webserver/websites?fields=name");
        Assert.IsNotNull(res);

        if (res.StatusCode != HttpStatusCode.OK)
        {
          throw new WebServerAdapterException(res);
        }

        var json = res.Content;
        var obj = JsonConvert.DeserializeObject(json.ToString());
      }
      catch (WebServerAdapterException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new WebServerAdapterException(ex);
      }
    }

    [NotNull]
    public FilePath GetWebSiteRootDirectoryPath([NotNull] string websiteName)
    {
      try
      {
        throw new WebSiteDoesNotExistException(websiteName);
      }
      catch (WebServerAdapterException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new WebServerAdapterException(ex);
      }
    }

    public bool WebSiteExists([NotNull] string websiteName)
    {
      try
      {
        return false;
      }
      catch (Exception ex)
      {
        throw new WebServerAdapterException(ex);
      }
    }

    [NotNull]
    public IReadOnlyList<string> GetWebSites()
    {
      try
      {
        return new string[0];
      }
      catch (Exception ex)
      {
        throw new WebServerAdapterException(ex);
      }
    }
  }
}

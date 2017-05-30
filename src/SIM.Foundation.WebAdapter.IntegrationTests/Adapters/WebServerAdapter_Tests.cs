namespace SIM.Adapters
{
  using System;
  using System.Linq;
  using JetBrains.Annotations;
  using Microsoft.VisualStudio.TestTools.UnitTesting;
  using SIM.Base.FileSystem;
  using SIM.Base.Services;

  [TestClass]
  public class WebServerAdapter_Tests
  {
    private const string LocalConnectionString = "https://localhost:55539/?token=Pfg-3Vod38C7Brh6LtwvQTXcOVcFUw0kT1rwLuMQ3_HGcgnjC2o5cQ";

    [NotNull]
    private WebServerAdapter Adapter { get; } = new WebServerAdapter(new WebServerConnectionString(LocalConnectionString));
    
    public WebServerAdapter_Tests()
    {
      System.Net.ServicePointManager.ServerCertificateValidationCallback =
        ((sender, certificate, chain, sslPolicyErrors) => true);
    }

    [TestMethod]
    public void DeleteWebSite_MissingWebSite()
    {
      Adapter.DeleteWebSite(GetRandomWebSiteName());
    }

    [TestMethod]
    public void GetWebSiteFilePath_MissingWebSite()
    {
      var WebSiteName = GetRandomWebSiteName();

      try
      {
        Adapter.GetWebSiteRootDirectoryPath(WebSiteName);
      }
      catch (WebSiteDoesNotExistException ex)
      {
        Assert.AreEqual(WebSiteName, ex.WebSiteName);
        Assert.AreEqual($"Failed to perform an operation with SqlServer. The requested '{WebSiteName}' WebSite does not exist", ex.Message);

        return;
      }

      Assert.Fail();
    }

    [TestMethod]
    public void Deploy_Check_Delete_Check()
    {
      var websiteName = GetRandomWebSiteName();
      var websiteRootDirectoryPath = new FilePath("C:\\inetpub\\wwroot");



      Adapter.CreateWebSite(websiteName, websiteRootDirectoryPath).Wait();


      int count;
      try
      {
        Adapter.CreateWebSite(websiteName, websiteRootDirectoryPath).Wait();
        Assert.AreEqual(true, Adapter.WebSiteExists(websiteName));
        Assert.AreEqual(websiteRootDirectoryPath.FullName, Adapter.GetWebSiteRootDirectoryPath(websiteName).FullName);

        var WebSites = Adapter.GetWebSites();
        Assert.AreEqual(true, WebSites.Contains(websiteName));

        count = WebSites.Count;
        Assert.AreEqual(true, count >= 1);
      }
      finally
      {
        Adapter.DeleteWebSite(websiteName);
      }

      Assert.AreEqual(false, Adapter.WebSiteExists(websiteName));

      var newCount = Adapter.GetWebSites().Count;
      Assert.AreEqual(count - 1, newCount);

    }

    [NotNull]
    private static string GetRandomWebSiteName()
    {
      return Guid.NewGuid().ToString("N");
    }
  }
}

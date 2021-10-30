﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PnP.Core.Admin.Test.Utilities;
using PnP.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PnP.Core.Admin.Test.SharePoint
{
    [TestClass]
    public class SiteCollectionAppManagerTests
    {
        private const string packageName = "pnp-alm-app.sppkg";
        private const string appTitle = "pnp-alm-app-client-side-solution";
        private string packagePath = $"TestAssets/{packageName}";

        [ClassInitialize]
        public static void TestFixtureSetup(TestContext context)
        {
            // Configure mocking default for all tests in this class, unless override by a specific test
            //TestCommon.Instance.Mocking = false;
        }

        [TestMethod]
        public async Task AddTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);

                Assert.IsNotNull(app);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddThrowsArgumentExceptionTitleNullTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(null, "filename", true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public async Task AddThrowsExceptionNoneExistingPathTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add("none-existing.sppkg", true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddThrowsArgumentExceptionPathNullTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(null, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddThrowsArgumentExceptionFilenameNullTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var bytes = File.ReadAllBytes(packagePath);
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(bytes, null, true);
            }
        }

        [TestMethod]
        public async Task GetAppByTitleTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                appManager.Add(packagePath, true);
                var app = appManager.GetAvailable(appTitle);

                Assert.IsNotNull(app);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAppByTitleThrowsArgumentExceptionTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.GetAvailable(null);
            }
        }

        [TestMethod]
        public async Task GetAppByTitleReturnsNullTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                appManager.Add(packagePath, true);
                var app = appManager.GetAvailable("none-existing");

                Assert.IsNull(app);
            }
        }

        [TestMethod]
        public async Task GetAppByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                appManager.Add(packagePath, true);
                var app = appManager.GetAvailable(appTitle);
                app = appManager.GetAvailable(app.Id);

                Assert.IsNotNull(app);
            }
        }

        [TestMethod]
        public async Task DeployTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                var result = app.Deploy(false);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task DeployByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                var result = appManager.Deploy(app.Id, false);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task GetAvailableAppsTests()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                appManager.Add(packagePath, true);
                var apps = appManager.GetAvailable();

                Assert.IsTrue(apps.Count > 0);
            }
        }

        [TestMethod]
        public async Task InstallTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);
                if (app.InstalledVersion != null)
                {
                    app.Uninstall();
                    if (!TestCommon.Instance.Mocking)
                    {
                        // uninstall takes some time
                        await Task.Delay(10 * 1000);
                    }
                }
                var result = app.Install();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task InstallByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);
                if (app.InstalledVersion != null)
                {
                    app.Uninstall();
                    if (!TestCommon.Instance.Mocking)
                    {
                        // uninstall takes some time
                        await Task.Delay(10 * 1000);
                    }
                }

                var result = appManager.Install(app.Id);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task UpgradeTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);

                if (app.InstalledVersion == null)
                {
                    app.Install();
                }

                var result = app.Upgrade();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task UpgradeByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);

                if (app.InstalledVersion == null)
                {
                    app.Install();
                }

                var result = appManager.Upgrade(app.Id);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task UninstallTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);

                if (app.InstalledVersion == null)
                {
                    app.Install();
                }

                var result = app.Uninstall();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task UninstallByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);

                if (app.InstalledVersion == null)
                {
                    app.Install();
                }

                var result = appManager.Uninstall(app.Id);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task RetractTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);
                var result = app.Retract();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task RetractByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                app.Deploy(false);
                var result = appManager.Retract(app.Id);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task RemoveTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                var result = app.Remove();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public async Task RemoveByIdTest()
        {
            //TestCommon.Instance.Mocking = false;
            using (var context = await TestCommon.Instance.GetContextAsync(TestCommon.SiteCollectionAppCatalogSite))
            {
                var appManager = context.GetSiteCollectionAppManager();
                var app = appManager.Add(packagePath, true);
                var result = appManager.Remove(app.Id);

                Assert.IsTrue(result);
            }
        }
    }
}

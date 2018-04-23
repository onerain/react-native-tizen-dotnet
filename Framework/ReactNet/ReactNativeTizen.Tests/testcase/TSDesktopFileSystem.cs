using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tizen.Applications;
using NUnit.Framework;
using PCLStorage;

namespace PCLStorage.Tests
{
    [TestFixture]
    public class DesktopFileSystemTests
    {
        private static DesktopFileSystem desktopFileSystem = new DesktopFileSystem();
        [Test]
        [Description("TC DesktopFileSystem Ctor")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.DesktopFileSystem C")]
        public static void DesktopFileSystem_Return()
        {
            DesktopFileSystem FileSystem = new DesktopFileSystem();
        }

        [Test]
        [Description("TC DesktopFileSystem GetFileFromPathAsync")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.GetFileFromPathAsync M")]
        public static async void GetFileFromPathAsync_PositiveReturn()
        {
            var storageFile = await desktopFileSystem.GetFileFromPathAsync(Application.Current.DirectoryInfo.Resource+ "/../tizen-manifest.xml", CancellationToken.None);
            Assert.NotNull(storageFile);
        }

        [Test]
        [Description("TC DesktopFileSystem GetFileFromPathAsync")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.GetFileFromPathAsync M")]
        public static async void GetFileFromPathAsync_NegativeReturn()
        {
            var storageFile = await desktopFileSystem.GetFileFromPathAsync("/usr/lib/noexistfile.not", CancellationToken.None);
            Assert.Null(storageFile);
        }

        [Test]
        [Description("TC DesktopFileSystem GetFolderFromPathAsync")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.GetFolderFromPathAsync M")]
        public static async void GetFolderFromPathAsync_PositiveReturn()
        {
            var folder = await desktopFileSystem.GetFolderFromPathAsync(Application.Current.DirectoryInfo.Resource, CancellationToken.None);
            Assert.NotNull(folder);
        }

        [Test]
        [Description("TC DesktopFileSystem GetFolderFromPathAsync")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.GetFolderFromPathAsync M")]
        public static async void GetFolderFromPathAsync_NegativeReturn()
        {
            var folder = await desktopFileSystem.GetFolderFromPathAsync("/usr/lib/noexistfile.not", CancellationToken.None);
            Assert.Null(folder);
        }

        [Test]
        [Description("TC DesktopFileSystem LocalStorage")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.LocalStorage A")]
        public static void LocalStorage_Return()
        {
            var folder = desktopFileSystem.LocalStorage;
            Assert.NotNull(folder);
        }

        [Test]
        [Description("TC DesktopFileSystem RoamingStorage")]
        [Property("SPEC", "PCLStorage.DesktopFileSystem.RoamingStorage A")]
        public static void RoamingStorage_Return()
        {
            var folder = desktopFileSystem.RoamingStorage;
            Assert.Null(folder);
        }
    }
}
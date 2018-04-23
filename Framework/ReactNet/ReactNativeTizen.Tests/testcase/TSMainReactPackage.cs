using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tizen.Applications;
using NUnit.Framework;
using ReactNative.Bridge;

namespace ReactNative.Shell.Tests
{
    [TestFixture]
    public class MainReactPackageTests
    {
        [Test]
        [Description("TC MainReactPackage Ctor")]
        [Property("SPEC", "ReactNative.Shell.MainReactPackage.MainReactPackage C")]
        public static void MainReactPackage_Return()
        {
            var mainReactPackage = new MainReactPackage();
        }

        [Test]
        [Description("TC MainReactPackage CreateJavaScriptModulesConfig")]
        [Property("SPEC", "ReactNative.Shell.MainReactPackage.CreateJavaScriptModulesConfig M")]
        public static void CreateJavaScriptModulesConfig_Return()
        {
            var mainReactPackage = new MainReactPackage();
            var list = mainReactPackage.CreateJavaScriptModulesConfig();
        }

        [Test]
        [Description("TC MainReactPackage CreateNativeModules")]
        [Property("SPEC", "ReactNative.Shell.MainReactPackage.CreateNativeModules M")]
        public static void CreateNativeModules_Return()
        {
            var mainReactPackage = new MainReactPackage();
            ReactContext _context = new ReactContext();
            var list = mainReactPackage.CreateNativeModules(_context);
        }

        [Test]
        [Description("TC MainReactPackage CreateViewManagers")]
        [Property("SPEC", "ReactNative.Shell.MainReactPackage.CreateViewManagers M")]
        public static void CreateViewManagers_Return()
        {
            var mainReactPackage = new MainReactPackage();
            ReactContext _context = new ReactContext();
            var list = mainReactPackage.CreateViewManagers(_context);
        }

    }
}
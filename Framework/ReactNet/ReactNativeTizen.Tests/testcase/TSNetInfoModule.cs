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


namespace ReactNative.Modules.NetInfo
{

    class Callback : ICallback
    {
        public void Invoke(params object[] arguments)
        {

        }
    }

        [TestFixture]
    public class NetInfoModuleTests
    {
        [Test]
        [Description("TC NetInfoModule Ctor1")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.NetInfoModule C")]
        [Property("COVPARAM", "ReactNative.Bridge.ReactContext")]
        public static void NetInfoModule_ReturnA()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(_context);
            Assert.NotNull(netInfo);
        }

        [Test]
        [Description("TC NetInfoModule Ctor2")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.NetInfoModule C")]
        [Property("COVPARAM", "ReactNative.Modules.NetInfo.INetworkInformation,ReactNative.Bridge.ReactContext")]
        public static void NetInfoModule_ReturnB()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            Assert.NotNull(netInfo);
        }

        [Test]
        [Description("TC NetInfoModule Name")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.Name M")]
        public static void Name_Return()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            var name = netInfo.Name;
            Assert.NotNull(name);
        }

        [Test]
        [Description("TC NetInfoModule Initialize")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.Initialize M")]
        public static void Initialize_Return()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            netInfo.Initialize();
        }

        [Test]
        [Description("TC NetInfoModule OnSuspend")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.OnSuspend M")]
        public static void OnSuspend_Return()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            netInfo.OnSuspend();
        }

        [Test]
        [Description("TC NetInfoModule OnResume")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.OnResume M")]
        public static void OnResume_Return()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            netInfo.OnResume();
        }

        [Test]
        [Description("TC NetInfoModule OnDestroy")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.OnDestroy M")]
        public static void OnDestroy_Return()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            netInfo.OnDestroy();
        }

        [Test]
        [Description("TC NetInfoModule getCurrentConnectivity")]
        [Property("SPEC", "ReactNative.Modules.NetInfo.NetInfoModule.getCurrentConnectivity M")]
        public static void getCurrentConnectivity_Return()
        {
            ReactContext _context = new ReactContext();
            var netInfo = new NetInfoModule(new DefaultNetworkInformation(), _context);
            netInfo.getCurrentConnectivity(new Promise(new Callback(), new Callback()));
        }
    }
}
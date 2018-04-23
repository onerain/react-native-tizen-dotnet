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

namespace ReactNative.Modules.Network.Tests
{
    [TestFixture]
    public class NetworkingModuleTests
    {
        [Test]
        [Description("TC NetworkingModule Name")]
        [Property("SPEC", "ReactNative.Modules.Network.NetworkingModule.Name M")]
        public static void Name_Return()
        {
            //NetworkingModule  construct is internal, this function could not be tested
        }

        [Test]
        [Description("TC NetworkingModule OnReactInstanceDispose")]
        [Property("SPEC", "ReactNative.Modules.Network.NetworkingModule.OnReactInstanceDispose M")]
        public static void OnReactInstanceDispose_Return()
        {
            //NetworkingModule  construct is internal, this function could not be tested
        }


        [Test]
        [Description("TC NetworkingModule abortRequest")]
        [Property("SPEC", "ReactNative.Modules.Network.NetworkingModule.abortRequest M")]
        public static void abortRequest_Return()
        {
            //NetworkingModule  construct is internal, this function could not be tested
        }
    }
}
using System;
using System.Collections.Generic;

using ReactNative;
using ReactNative.Common;
using ReactNative.Tracing;
using ReactNative.Shell;
using ReactNative.Modules.Core;

using Tizen.Applications;

namespace TestApp
{
    class TestApp : ReactProgram
    {
        public override string MainComponentName
        {
            get
            {
                return "react_native_testsuite.tizen";
            }
        }

        public override string JavaScriptMainModuleName
        {
            get
            {
                return "index.tizen";
            }
        }

#if BUNDLE
        public override string JavaScriptBundleFile
        {
            get
            {
                return Application.Current.DirectoryInfo.SharedResource + MainComponentName + ".bundle";
            }
        }
#endif

        public override List<IReactPackage> Packages
        {
            get
            {
                return new List<IReactPackage>
                {
                    new MainReactPackage(),
                };
            }
        }

        public override bool UseDeveloperSupport
        {
            get
            {
#if !BUNDLE// || DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        static void Main(string[] args)
        {
            try
            {
                TestApp app = new TestApp();
                

                app.Run(args);
            }
            catch (Exception e)
            {
                Tracer.Error(ReactConstants.Tag, e.ToString());
            }
        }
    }
}
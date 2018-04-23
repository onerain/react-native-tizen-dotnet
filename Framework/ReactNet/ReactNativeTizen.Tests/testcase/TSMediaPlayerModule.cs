using NUnit.Framework;

using ReactNative.Bridge;
using ReactNative.Modules.MediaPlayer;

namespace ReactNative.Modules.MediaPlayer.Tests
{
    [TestFixture]
    class MediaPlayerModuleTests
    {
        public static readonly MediaPlayerModule _module = new MediaPlayerModule(new ReactContext());

        [SetUp]
        public static void Init()
        {
            _module.init("/opt/baisuzhen.mp3");
        }

        [TearDown]
        public static void Destroy()
        {
            _module.deInit();
        }

        [Test]
        [Description("MediaPlayerModule")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.MediaPlayerModule M")]
        //[Property("COVPARAM", " ")]
        public static void MediaPlayerModule_ReturnPositive()
        {
            // TODO: Add your test code here
            Assert.AreNotEqual(new MediaPlayerModule(new ReactContext()), null);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("Name")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.Name M")]
        //[Property("COVPARAM", " ")]
        public static void Name_ReturnPositive()
        {
            // TODO: Add your test code here
            Assert.AreEqual(_module.Name, "MediaPlayer");

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("init Player")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.init M")]
        //[Property("COVPARAM", " ")]
        public static void init_ReturnPositive()
        {
            // return 'void'
            _module.init("/opt/test.mp3");

            Assert.Pass("Your init passing Postive test");
        }

        [Test]
        [Description("play")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.play M")]
        //[Property("COVPARAM", " ")]
        public static void play_ReturnPositive()
        {
            // return 'void'
            _module.play();

            Assert.Pass("Your play passing Postive test");
        }

        [Test]
        [Description("pause")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.pause M")]
        //[Property("COVPARAM", " ")]
        public static void pause_ReturnPositive()
        {
            // return 'void'
            _module.pause();

            Assert.Pass("Your pause passing Postive test");
        }

        [Test]
        [Description("seekTo")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.seekTo M")]
        //[Property("COVPARAM", " ")]
        public static void seekTo_ReturnPositive()
        {
            // return 'void'
            _module.seekTo(1000*10);

            Assert.Pass("Your seekTo passing Postive test");
        }

        [Test]
        [Description("stop")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.stop M")]
        //[Property("COVPARAM", " ")]
        public static void stop_ReturnPositive()
        {
            // return 'void'
            _module.pause();

            Assert.Pass("Your stop passing Postive test");
        }

        [Test]
        [Description("deInit")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.deInit M")]
        //[Property("COVPARAM", " ")]
        public static void deInit_ReturnPositive()
        {
            // return 'void'
            _module.deInit();

            Assert.Pass("Your deInit passing Postive test");
        }

        [Test]
        [Description("setSubtitle")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.setSubtitle M")]
        //[Property("COVPARAM", " ")]
        public static void setSubtitle_ReturnPositive()
        {
            // return 'void'
            _module.setSubtitle("/opt/test1.srt");

            Assert.Pass("Your setSubtitle passing Postive test");
        }

        [Test]
        [Description("setVolume")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.setVolume M")]
        //[Property("COVPARAM", " ")]
        public static void setVolume_ReturnPositive()
        {
            // return 'void'
            _module.setVolume(0.2f);

            Assert.Pass("Your setVolume passing Postive test");
        }

        [Test]
        [Description("setLooping")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.setLooping M")]
        //[Property("COVPARAM", " ")]
        public static void setLooping_ReturnPositive()
        {
            // return 'void'
            _module.setLooping(true);

            Assert.Pass("Your setLooping passing Postive test");
        }

        [Test]
        [Description("setMute")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.setMute M")]
        //[Property("COVPARAM", " ")]
        public static void setMute_ReturnPositive()
        {
            // return 'void'
            _module.setMute(true);
            Assert.Pass("Your setLooping passing Postive test");
        }
        
        // LifeCycle Begin
        [Test]
        [Description("OnDestroy")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.OnDestroy M")]
        //[Property("COVPARAM", " ")]
        public static void OnDestroy_ReturnPositive()
        {
            // return 'void'
            _module.OnDestroy();
            Assert.Pass("Your OnDestroy passing Postive test");
        }

        [Test]
        [Description("OnPause")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.OnPause M")]
        //[Property("COVPARAM", " ")]
        public static void OnPause_ReturnPositive()
        {
            // return 'void'
            _module.OnPause();
            Assert.Pass("Your OnPause passing Postive test");
        }

        [Test]
        [Description("OnResume")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.OnResume M")]
        //[Property("COVPARAM", " ")]
        public static void OnResume_ReturnPositive()
        {
            // return 'void'
            _module.OnResume();
            Assert.Pass("Your OnResume passing Postive test");
        }

        [Test]
        [Description("OnSuspend")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.MediaPlayerModule.OnSuspend M")]
        //[Property("COVPARAM", " ")]
        public static void OnSuspend_ReturnPositive()
        {
            // return 'void'
            _module.OnSuspend();
            Assert.Pass("Your OnResume passing Postive test");
        }

        // LifeCycle End
    }
}
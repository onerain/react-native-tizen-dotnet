using NUnit.Framework;
using ReactNative.Modules.MediaPlayer;

using Tizen;

namespace ReactNative.Modules.MediaPlayer.Tests
{
    [TestFixture]
    class DantePlayerTests
    {
        [SetUp]
        public static void Init()
        {
            Assert.AreEqual(DantePlayer.instance.state, State.Idle);
            DantePlayer.instance.Prepare("/opt/baisuzhen.mp3");
            Assert.AreEqual(DantePlayer.instance.state, State.Started);
        }

        [TearDown]
        public static void Destroy()
        {
            DantePlayer.instance.UnPrepare();
            Assert.AreEqual(DantePlayer.instance.state, State.Idle);
        }

        [Test]
        [Description("instance")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.instance M")]
        //[Property("COVPARAM", " ")]
        public static void instance_ReturnPositive()
        {
            Assert.AreNotEqual(DantePlayer.instance, null);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("Init Player")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.Prepare M")]
        //[Property("COVPARAM", " ")]
        public static void Prepare_ReturnPositive()
        {
            // TODO: Add your test code here
            Assert.AreEqual(DantePlayer.instance.state, State.Idle);
            DantePlayer.instance.Prepare("/opt/baisuzhen.mp3");
            Assert.AreEqual(DantePlayer.instance.state, State.Started);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("Init Player negative test")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.Prepare M")]
        //[Property("COVPARAM", " ")]
        public static void Prepare_ReturnNegative()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp3");
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);
            Assert.AreEqual(DantePlayer.instance.state, State.Started);

            Assert.Pass("Your first passing Negative test");
        }

        [Test]
        [Description("DantePlayer Play")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.Play M")]
        //[Property("COVPARAM", " ")]
        public static void Play_ReturnPositive()
        {
            // TODO: Add your test code here
            Assert.AreEqual(DantePlayer.instance.state, State.Prepared);
            RET_CODE ret = DantePlayer.instance.Play();
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);
            Assert.AreEqual(DantePlayer.instance.state, State.Started);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("DantePlayer Pause")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.Pause M")]
        //[Property("COVPARAM", " ")]
        public static void Pause_ReturnPositive()
        {
            // TODO: Add your test code here
            //Assert.AreEqual(DantePlayer.instance.state, State.Ready);
            RET_CODE ret = DantePlayer.instance.Pause();
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);
            Assert.AreEqual(DantePlayer.instance.state, State.Paused);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("DantePlayer Stop")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.Stop M")]
        //[Property("COVPARAM", " ")]
        public static void Stop_ReturnPositive()
        {
            // TODO: Add your test code here
            //Assert.AreEqual(DantePlayer.instance.state, State.Ready);
            RET_CODE ret = DantePlayer.instance.Stop();
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);
            Assert.AreEqual(DantePlayer.instance.state, State.Started);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("DantePlayer UnPrepare")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.UnPrepare M")]
        //[Property("COVPARAM", " ")]
        public static void UnPrepare_ReturnPositive()
        {
            // TODO: Add your test code here
            //Assert.AreEqual(DantePlayer.instance.state, State.Ready);
            RET_CODE ret = DantePlayer.instance.UnPrepare();
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);
            Assert.AreEqual(DantePlayer.instance.state, State.Idle);

            Assert.Pass("Your first passing Postive test");
        }

        [Test]
        [Description("DantePlayer SetSubtile")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetSubtile M")]
        //[Property("COVPARAM", " ")]
        public static void SetSubtile_ReturnPositive()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp4");
            ret = DantePlayer.instance.SetSubtile("/opt/test1.sub");
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);

            ret = DantePlayer.instance.SetSubtile("/opt/test2.smi");
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);

            ret = DantePlayer.instance.SetSubtile("/opt/test3.srt");
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);

            DantePlayer.instance.Play();
            Assert.Pass("Your SetSubtile passing Postive test");
        }

        [Test]
        [Description("SetSubtile")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetSubtile M")]
        //[Property("COVPARAM", " ")]
        public static void SetSubtile_ReturnNegative()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp4");

            ret = DantePlayer.instance.SetSubtile("/opt/123.mp3");
            Assert.AreEqual(ret, RET_CODE.PLAYER_SUBTITLE_FORMAT_NOT_SUPPORTED);

            ret = DantePlayer.instance.SetSubtile(null);
            Assert.AreEqual(ret, RET_CODE.PLAYER_SUBTITLE_NULL_PATH);

            ret = DantePlayer.instance.SetSubtile("");
            Assert.AreEqual(ret, RET_CODE.PLAYER_SUBTITLE_EMPTY_PATH);

            DantePlayer.instance.Play();

            Assert.Pass("Your SetSubtile Positive test");
        }


        [Test]
        [Description("ClearSubtile")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.ClearSubtile M")]
        //[Property("COVPARAM", " ")]
        public static void ClearSubtile_ReturnPositive()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp4");
            ret = DantePlayer.instance.SetSubtile("/opt/test3.srt");
            ret = DantePlayer.instance.Play();

            DantePlayer.instance.ClearSubtile();

            Assert.Pass("Your SetSubtile Positive test");
        }

        [Test]
        [Description("GetDownlaodProgress")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.GetDownlaodProgress M")]
        //[Property("COVPARAM", " ")]
        public static void GetDownlaodProgress_ReturnPositive()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp4");
            ret = DantePlayer.instance.SetSubtile("/opt/test3.srt");
            ret = DantePlayer.instance.Play();

            int start = 0;
            int current = 0;
            DantePlayer.instance.GetDownlaodProgress(out start, out current);

            Assert.Pass("Your SetSubtile Positive test");
        }

        [Test]
        [Description("AudioCodecFormat")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.AudioCodecFormat A")]
        //[Property("COVPARAM", " ")]
        public static void AudioCodecFormat_ReturnPositive()
        {
            // TODO: Add your test code here
            Assert.AreEqual(DantePlayer.instance.AudioCodecFormat, "mp3");

            Assert.Pass("Your SetSubtile Positive test");
        }

        [Test]
        [Description("AudioCodecFormat")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.AudioCodecFormat A")]
        //[Property("COVPARAM", " ")]
        public static void AudioCodecFormat_ReturnNegative()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.wav"); 
            Assert.AreEqual(DantePlayer.instance.AudioCodecFormat, "");

            Assert.Pass("Your SetSubtile Positive test");
        }

        [Test]
        [Description("VideoCodecFormat")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.VideoCodecFormat A")]
        //[Property("COVPARAM", " ")]
        public static void VideoCodecFormat_ReturnPositive()
        {
            // TODO: Add your test code here
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp4");
            Assert.AreEqual(DantePlayer.instance.VideoCodecFormat, "h.264");

            Assert.Pass("Your SetSubtile Positive test");
        }

        [Test]
        [Description("VideoCodecFormat")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.VideoCodecFormat A")]
        //[Property("COVPARAM", " ")]
        public static void VideoCodecFormat_ReturnNegative()
        {
            // TODO: Add your test code here
            Assert.AreEqual(DantePlayer.instance.VideoCodecFormat, "");

            Assert.Pass("Your SetSubtile Negative test");
        }

        [Test]
        [Description("State")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.state A")]
        //[Property("COVPARAM", " ")]
        public static void state_ReturnPositive()
        {
            // TODO: Add your test code here
            Assert.AreEqual(DantePlayer.instance.state, State.Idle);

            Assert.Pass("Your SetSubtile Positive test");
        }

        [Test]
        [Description("position")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.position A")]
        //[Property("COVPARAM", " ")]
        public static void position_ReturnPositive()
        {
            RET_CODE ret = DantePlayer.instance.Prepare("/opt/test.mp3");
            Assert.AreEqual(ret, RET_CODE.PLAYER_OK);

            Assert.AreEqual(DantePlayer.instance.position, 0);

            ret = DantePlayer.instance.Play();
            Assert.AreNotEqual(DantePlayer.instance.position, 0);

            Assert.Pass("Your Position Positive test");
        }

        [Test]
        [Description("duration")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.duration A")]
        //[Property("COVPARAM", " ")]
        public static void duration_ReturnPositive()
        {
            Assert.AreNotEqual(DantePlayer.instance.duration, 0);

            Assert.Pass("Your Duration Positive test");
        }

        [Test]
        [Description("Album Info")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.AlbumInfo A")]
        //[Property("COVPARAM", " ")]
        public static void AlbumInfo_ReturnPositive()
        {
            Assert.AreEqual(DantePlayer.instance.AlbumInfo, "");
            Assert.AreNotEqual(DantePlayer.instance.AlbumInfo, "");

            Assert.Pass("Your AlbumInfo Positive test");
        }

        [Test]
        [Description("looping")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.looping A")]
        //[Property("COVPARAM", " ")]
        public static void looping_ReturnPositive()
        {
            DantePlayer.instance.looping = true;
            Assert.AreEqual(DantePlayer.instance.looping, true);

            DantePlayer.instance.looping = false;
            Assert.AreEqual(DantePlayer.instance.looping, false);

            Assert.Pass("Your Looping Positive test");
        }

        [Test]
        [Description("Muted")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.muted A")]
        //[Property("COVPARAM", " ")]
        public static void muted_ReturnPositive()
        {
            DantePlayer.instance.muted = true;
            Assert.AreEqual(DantePlayer.instance.muted, true);

            DantePlayer.instance.muted = false;
            Assert.AreEqual(DantePlayer.instance.muted, false);

            Assert.Pass("Your Mute Positive test");
        }

        [Test]
        [Description("volume")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.volume A")]
        //[Property("COVPARAM", " ")]
        public static void volume_ReturnPositive()
        {
            DantePlayer.instance.volume = 0.5f;
            Assert.AreEqual(DantePlayer.instance.volume, 0.5f);

            DantePlayer.instance.volume = 0.0f;
            Assert.AreEqual(DantePlayer.instance.volume, 0.0f);
            Assert.AreEqual(DantePlayer.instance.muted, true);

            Assert.Pass("Your volume Positive test");
        }

        [Test]
        [Description("currentMediaUri")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.currentMediaUri A")]
        //[Property("COVPARAM", " ")]
        public static void currentMediaUri_ReturnPositive()
        {
            Assert.AreNotEqual(DantePlayer.instance.currentMediaUri, "");
            Assert.AreEqual(DantePlayer.instance.currentMediaUri, "/opt/baisuzhen.mp3");
            Assert.Pass("Your currentMediaUri Positive test");
        }

        [Test]
        [Description("cookie")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.cookie A")]
        //[Property("COVPARAM", " ")]
        public static void cookie_ReturnPositive()
        {
            Assert.AreNotEqual(DantePlayer.instance.cookie, "");

            Assert.Pass("Your cookie Positive test");
        }

        [Test]
        [Description("volume")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.volume A")]
        //[Property("COVPARAM", " ")]
        public static void volume_ReturnNegative()
        {
            DantePlayer.instance.volume = 1.5f;
            Assert.AreEqual(DantePlayer.instance.volume, 0.2f);

            DantePlayer.instance.volume = -0.5f;
            Assert.AreEqual(DantePlayer.instance.volume, 0.2f);

            Assert.Pass("Your volume Negative test");
        }


        [Test]
        [Description("playbackRate")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.playbackRate A")]
        //[Property("COVPARAM", " ")]
        public static void playbackRate_ReturnPositive()
        {
            DantePlayer.instance.playbackRate = 3.0f;

            Assert.Pass("Your playbackRate Positive test");
        }

        [Test]
        [Description("playbackRate")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.playbackRate A")]
        //[Property("COVPARAM", " ")]
        public static void playbackRate_ReturnNegative()
        {
            DantePlayer.instance.playbackRate = 6.0f;

            DantePlayer.instance.playbackRate = 0.0f;

            DantePlayer.instance.playbackRate = -6.0f;

            Assert.Pass("Your playbackRate Negative test");
        }

        [Test]
        [Description("SeekTo")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SeekTo M")]
        //[Property("COVPARAM", " ")]
        public static void SeekTo_ReturnPositive()
        {
            DantePlayer.instance.SeekTo(8 * 1000);

            Assert.Pass("Your playbackRate Negative test");
        }


        // Delegate Begin
        private static void OnErrorOcurred(int ret_code)
        {
            Log.Debug("RNUT", $"[OnErrorOcurred] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnErrorOccurred")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnErrorOccurred M")]
        public static void SetOnErrorOccurred_Return()
        {
            DantePlayer.instance.SetOnErrorOccurred(OnErrorOcurred);
            Assert.Pass("Your SetOnErrorOccured Negative test");
        }

        private static void OnIdle(int ret_code)
        {
            Log.Debug("RNUT", $"[OnIdle] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnIdle")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnIdle M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnIdle_ReturnPositive()
        {
            DantePlayer.instance.SetOnIdle(OnIdle);
            Assert.Pass("Your SetOnIdle Negative test");
        }

        private static void OnPaused(int ret_code)
        {
            Log.Debug("RNUT", $"[OnPaused] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnPaused")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnPaused M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnPaused_ReturnPositive()
        {
            DantePlayer.instance.SetOnPaused(OnPaused);
            Assert.Pass("Your SetOnPaused Negative test");
        }

        private static void OnPlaybackComplete(int ret_code)
        {
            Log.Debug("RNUT", $"[OnPlaybackComplete] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnPlaybackComplete")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnPlaybackComplete M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnPlaybackComplete_ReturnPositive()
        {
            DantePlayer.instance.SetOnPlaybackComplete(OnPlaybackComplete);
            Assert.Pass("Your SetOnPaused Negative test");
        }

        private static void OnPlaybackInterrupted(int ret_code)
        {
            Log.Debug("RNUT", $"[OnPlaybackInterrupted] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnPlaybackInterrupted")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnPlaybackInterrupted M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnPlaybackInterrupted_ReturnPositive()
        {
            DantePlayer.instance.SetOnPlaybackInterrupted(OnPlaybackInterrupted);
            Assert.Pass("Your SetOnPlaybackInterrupted Negative test");
        }

        private static void OnPlaying(int ret_code)
        {
            Log.Debug("RNUT", $"[OnPlaying] ret_code={ret_code}");
        }

        private static void OnPrepared(int ret_code)
        {
            Log.Debug("RNUT", $"[OnPrepared] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnPrepared")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnPrepared M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnPrepared_ReturnPositive()
        {
            DantePlayer.instance.SetOnPrepared(OnPrepared);
            Assert.Pass("Your OnPlaying Negative test");
        }


        private static void OnPreparing(int ret_code)
        {
            Log.Debug("RNUT", $"[OnPreparing] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnPreparing")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnPreparing M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnPreparing_ReturnPositive()
        {
            DantePlayer.instance.SetOnPreparing(OnPreparing);
            Assert.Pass("Your OnPlaying Negative test");
        }

        private static void OnException(int ret_code)
        {
            Log.Debug("RNUT", $"[OnException] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnThrowException")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnThrowException M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnThrowException_ReturnPositive()
        {
            DantePlayer.instance.SetOnThrowException(OnException);
            Assert.Pass("Your OnPlaying Negative test");
        }

        private static void OnUpdatePlayInfo(int ret_code)
        {
            Log.Debug("RNUT", $"[OnUpdatePlayInfo] ret_code={ret_code}");
        }

        [Test]
        [Description("SetOnThrowException")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnUpdatePlayInfo M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnUpdatePlayInfo_ReturnPositive()
        {
            DantePlayer.instance.SetOnUpdatePlayInfo(OnUpdatePlayInfo);
            Assert.Pass("Your OnPlaying Negative test");
        }
        // Delegate End


        [Test]
        [Description("GetCurrentState")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.GetCurrentState M")]
        //[Property("COVPARAM", " ")]
        public static void GetCurrentState_ReturnPositive()
        {
            DantePlayer.instance.GetCurrentState();
            Assert.Pass("Your OnPlaybackComplete Negative test");
        }

        [Test]
        [Description("SetOnSeeked")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnSeeked M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnSeeked_ReturnPositive()
        {
            DantePlayer.instance.SetOnSeeked(null);
            Assert.Pass("Your OnPlaybackComplete Negative test");
        }

        [Test]
        [Description("SetOnSeeking")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnSeeking M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnSeeking_ReturnPositive()
        {
            DantePlayer.instance.SetOnSeeking(null);
            Assert.Pass("Your OnPlaybackComplete Negative test");
        }

        [Test]
        [Description("SetOnStarted")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.SetOnStarted M")]
        //[Property("COVPARAM", " ")]
        public static void SetOnStarted_ReturnPositive()
        {
            DantePlayer.instance.SetOnStarted(null);
            Assert.Pass("Your OnPlaybackComplete Negative test");
        }


        [Test]
        [Description("PlayerCBList")]
        [Property("SPEC", "ReactNative.Modules.MediaPlayer.DantePlayer.PlayerCBList A")]
        //[Property("COVPARAM", " ")]
        public static void PlayerCBList_ReturnPositive()
        {
            var ret = DantePlayer.PlayerCBList;
            Assert.Pass("Your OnPlaybackComplete Negative test");
        }
    }
}      

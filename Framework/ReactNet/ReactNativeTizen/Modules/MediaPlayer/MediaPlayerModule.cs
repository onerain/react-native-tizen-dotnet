using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using ReactNative.Modules.Core;
using System.Collections.Generic;
using Tizen;
using ElmSharp;

namespace ReactNative.Modules.MediaPlayer
{
    /// <summary>
    /// Native module for playing media files.  2017-06-27 BY YB
    /// </summary>
    public class MediaPlayerModule : ReactContextNativeModuleBase, ILifecycleEventListener
    {   
        public const string MEDIA_PALYER_MODULE = "MPM";
        
        private bool _bInit = false;

        private MediaInfo _mediaInfo = new MediaInfo();

        public MediaPlayerModule(ReactContext reactContext)
            : base(reactContext)
        {
            // Log.Info(MEDIA_PALYER_MODULE, "[Constructor] MediaPlayerModule ");
            //Context.AddLifecycleEventListener(this);
            reactContext.AddLifecycleEventListener(this);
        }

        /// <summary>
        /// Gets the name of the native module.
        /// </summary>
        public override string Name
        {
            get
            {
                return "MediaPlayer";
            }
        }

        private void OnIdle(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[idle] Data:[null] ");
            SendEvent("idle", new JObject
            {
                {"ret_code", ret_code},
            }); 
        }        

        private void OnPreparing(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[preparing] Data:[null] ");
            SendEvent("preparing", new JObject
            {
                {"ret_code", ret_code},
            }); 
        }

        private void OnPrepared(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, $" Event:[prepared] Data:[state={DantePlayer.instance.state}, duration={DantePlayer.instance.duration}ms, position={DantePlayer.instance.position}ms] ");

            // Cache 'duration'
            _mediaInfo.duration = DantePlayer.instance.duration;
            _mediaInfo.ablum = DantePlayer.instance.AlbumInfo;

            // TODO: emit event to JS (gather some necessary media info for JS)
            SendEvent("prepared", new JObject
            {
                {"duration", _mediaInfo.duration},
                {"album", _mediaInfo.ablum },
                {"state", (int)DantePlayer.instance.state}
            });
        } 
        private void OnStarted(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[started] Data:[null] ");

            // TODO: drived by requirements
            SendEvent("started", new JObject
            {
                {"ret_code", ret_code},
            });            
        }

        private void OnSeeking(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[seeking] Data:[null] ");
            SendEvent("seeking", new JObject
            {
                {"ret_code", ret_code},
            }); 
        }
        private void OnSeeked(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[seeked] Data:[null] ");
            SendEvent("seeked", new JObject
            {
                {"ret_code", ret_code},
            }); 
        }

        private void OnPaused(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[paused] Data:[null] ");

            // TODO: drived by requirements
            SendEvent("paused", new JObject
            {
                {"ret_code", ret_code},
            });            
        }

        private void OnUpdatePlayInfo(int ret_code)
        {
            // check
            //if ( State.Started == DantePlayer.instance.state && _mediaInfo.position != DantePlayer.instance.position )
            if ( State.STARTED == DantePlayer.instance.state )
            {
                Log.Info(MEDIA_PALYER_MODULE, $" Event:[updatePlayInfo] Data:[duration={DantePlayer.instance.duration}ms, position={DantePlayer.instance.position}ms] ");

                // Cache
                _mediaInfo.position = DantePlayer.instance.position;  

                // TODO: emit event to JS (gather some necessary media info for JS)
                SendEvent("updatePlayInfo", new JObject
                {
                    {"duration", _mediaInfo.duration},
                    {"position", _mediaInfo.position},
                });
                  
            }
        }

        private void OnThrowException(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[OnThrowException] Data:[null] ");

            // TODO: drived by requirements
            SendEvent("exceptionHappened", new JObject
            {
                {"ret_code", ret_code},
            });
        }

        private void OnErrorOccurred(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[errorOccurred] Data:[null] ");

            // reset flag
            _bInit = false;

            // TODO: drived by requirements
            SendEvent("errorOccurred", new JObject
            {
                {"ret_code", ret_code},
            });
        }

        private void OnPlaybackInterrupted(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, " Event:[playbackInterrupted] Data:[null] ");

            // reset flag
            _bInit = false;

            // TODO: drived by requirements
            //SendEvent("playbackInterrupted", null);
        }

        private void OnPlaybackComplete(int ret_code)
        {
            Log.Info(MEDIA_PALYER_MODULE, $" Event:[playbackComplete] Data:[state={(State)DantePlayer.instance.state}, position={DantePlayer.instance.position}ms, source={DantePlayer.instance.currentMediaUri}] ");

            // reset flag
            _bInit = false;

            // TODO: drived by requirements
            SendEvent("playbackComplete", new JObject{{"state", (int)DantePlayer.instance.state}});
        }

        #region "React Methods"
        [ReactMethod]
        public void init(string url)
        {
            if ( false == _bInit )  // First time register
            {
                DantePlayer.instance.SetOnIdle(OnIdle);
                DantePlayer.instance.SetOnPreparing(OnPreparing);
                DantePlayer.instance.SetOnPrepared(OnPrepared);
                DantePlayer.instance.SetOnStarted(OnStarted);
                DantePlayer.instance.SetOnPaused(OnPaused);
                DantePlayer.instance.SetOnSeeking(OnSeeking);
                DantePlayer.instance.SetOnSeeked(OnSeeked);
                DantePlayer.instance.SetOnUpdatePlayInfo(OnUpdatePlayInfo);
                DantePlayer.instance.SetOnPlaybackComplete(OnPlaybackComplete);
                DantePlayer.instance.SetOnPlaybackInterrupted(OnPlaybackInterrupted);
                DantePlayer.instance.SetOnErrorOccurred(OnErrorOccurred);
                DantePlayer.instance.SetOnThrowException(OnThrowException);
            }

            // Init
            DantePlayer.instance.Prepare(url);

            // reset flag
            _bInit = true;
        }

        [ReactMethod]
        public void play()
        {
            //Log.Info(MEDIA_PALYER_MODULE, "[BGN] Play ## ");
            DantePlayer.instance.Play();
            //Log.Info(MEDIA_PALYER_MODULE, "[END] Play ##");
        }

        [ReactMethod]
        public void seekTo(int millisec)
        {
            //Log.Info(MEDIA_PALYER_MODULE, $"[BGN] seekTo = {millisec}ms ## ");
            DantePlayer.instance.SeekTo(millisec);
            //Log.Info(MEDIA_PALYER_MODULE, $"[END] seekTo = {millisec}ms ## ");
        }

        [ReactMethod]
        public void pause()
        {
            //Log.Info(MEDIA_PALYER_MODULE, "[BGN] Pause ## ");
            DantePlayer.instance.Pause();
            //Log.Info(MEDIA_PALYER_MODULE, "[END] Pause ## ");
        }

        [ReactMethod]
        public void stop()
        {
            //Log.Info(MEDIA_PALYER_MODULE, "[BGN] Stop ## ");
            DantePlayer.instance.Stop();
            //Log.Info(MEDIA_PALYER_MODULE, "[END] Stop ## ");
        }
        
        [ReactMethod]
        public void deInit()
        {
            //Log.Info(MEDIA_PALYER_MODULE, "[BGN] DeInit ## ");
            DantePlayer.instance.UnPrepare();
            //Log.Info(MEDIA_PALYER_MODULE, "[END] DeInit ## ");

            // reset flag
            _bInit = false;
        }

        [ReactMethod]
        public void setSubtitle(string path)
        {
            DantePlayer.instance.SetSubtile(path);
        }

        [ReactMethod]
        public void setMute(bool muted)
        {
            DantePlayer.instance.muted = muted;
        }

        [ReactMethod]
        public void setVolume(float v)
        {
            DantePlayer.instance.volume = v;
        }

        [ReactMethod]
        public void setLooping(bool loop)
        {
            DantePlayer.instance.looping = loop;
        }

        #endregion

        /// <summary>
        /// Called when the application host is destroyed.
        /// </summary>
        public void OnDestroy()
        {
            // TODO: destroy player instance
            Log.Info(MEDIA_PALYER_MODULE, "[INFO] App State -> Destroy");
            //DantePlayer.instance.UnPrepare();
        }

        /// <summary>
        /// Called when the application host is resumed.
        /// </summary>
        public void OnResume()
        {
            Log.Info(MEDIA_PALYER_MODULE, "[INFO] App State -> Resume");
            //DantePlayer.instance.Play();
        }

        /// <summary>
        /// Called when the application host is paused.
        /// </summary>
        public void OnPause()
        {
            Log.Info(MEDIA_PALYER_MODULE, "[INFO] App State -> Pause");
            //DantePlayer.instance.Pause();            
        }

        /// <summary>
        /// Called when the application host is suspended.
        /// </summary>
        public void OnSuspend()
        {
            Log.Info(MEDIA_PALYER_MODULE, "[INFO] App State -> Suspend");
            //DantePlayer.instance.Pause(); 
        }

        private void SendEvent(string eventName, JObject parameters)
        {
            //Log.Info(MEDIA_PALYER_MODULE, $"[INFO] Location:[{Context.IsOnDispatcherQueueThread()},{Context.IsOnJavaScriptQueueThread()},{Context.IsOnNativeModulesQueueThread()}]");
            Context.GetJavaScriptModule<RCTDeviceEventEmitter>()
                .emit(eventName, parameters);
        }
    }
}
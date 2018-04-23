using System;
//using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.UIManager.Events;

using ElmSharp;
using Tizen;
using ReactNative.Common;

//using System.Threading;

namespace ReactNative.Views.ActivityIndicator
{
    public class ReactActivityIndicatorManager : SimpleViewManager<ProgressBar>
    {
        private const string ReactClass = "RCTActivityIndicator";

        /// <summary>
        /// The name of this view manager. This will be the name used to 
        /// reference this view manager from JavaScript.
        /// </summary>
        public override string Name
        {
            get
            {
                return ReactClass;
            }
        }

        /// <summary>
        /// The view manager event constants.
        /// </summary>
        public override IReadOnlyDictionary<string, object> ExportedCustomDirectEventTypeConstants
        {
            get
            {
                return new Dictionary<string, object>
                {
                    {
                        "topSelectedChange",
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onSelectedChange" }
                        }
                    },
                };
            }
        }
/* 
        [ReactProp("IsPulseMode")]
        public void IsPulseMode(ProgressBar view, bool bPulseMode)
        {
            Log.Info(ReactConstants.Tag, $"[Views::AI] set PulseMode:'{bPulseMode}'");

            view.IsPulseMode = bPulseMode;
        }

        [ReactProp("playPulse")]
        public void PlayPulse(ProgressBar view, bool bPulse)
        {
            Log.Info(ReactConstants.Tag, $"[Views::AI] Set playPulse:'{bPulse}'");
            
            view.PlayPulse();  
        }

        [ReactProp("stopPulse")]
        public void StopPulse(ProgressBar view, bool bPulse)
        {
            Log.Info(ReactConstants.Tag, $"[Views::AI] Set stopPulse:'{bPulse}'");
            
            view.StopPulse();
        }
*/
        /// <summary>
        /// Sets the font color for the node.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(ProgressBar view, uint? color)
        {
            if (color.HasValue)
            {
                var c = ColorHelpers.Parse(color.Value);
                view.Color = c;

                Tracing.RNTracer.Write(Common.ReactConstants.Tag, "SetColor = " + c.ToString() + ", from " + color.Value);
            }
        }

        [ReactProp("animating")]
        public void SetAnimating(ProgressBar view, bool bAnimating)
        {
            Log.Info(ReactConstants.Tag, $"[Views::AI] Set animating:'{bAnimating}'");
            
            view.IsPulseMode = bAnimating;

            if(bAnimating)
            {
                view.PlayPulse();
            }
            else
            {
                view.StopPulse();
            }
        }

        [ReactProp("display")]
        public void SetDisplay(ProgressBar view, bool bDisplay)
        {
            Log.Info(ReactConstants.Tag, $"[Views::AI] Set display:'{bDisplay}'");            

            if(bDisplay)
            {
                view.Show();
            }
            else
            {
                view.Hide();
            }
        }

        // NOT SUPPORTED
        /*
        [ReactProp("size")]
        public void SetSize(ProgressBar view, int size)
        {
            Log.Info(ReactConstants.Tag, "[Views::Grid] Size: " + size );
        }
        */

        /// <summary>
        /// Receive events/commands directly from JavaScript through the 
        /// <see cref="UIManagerModule"/>.
        /// </summary>
        /// <param name="view">
        /// The view instance that should receive the command.
        /// </param>
        /// <param name="commandId">Identifer for the command.</param>
        /// <param name="args">Optional arguments for the command.</param>
        public override void ReceiveCommand(ProgressBar view, int commandId, JArray args)
        {
            // TODO: parse command & change view
            Log.Info(ReactConstants.Tag, "[Views] ReactActivityIndicatorManager::ReceiveCommand");
        }
        


        /// <summary>
        /// Creates a new view instance of type <see cref="activityIndicator"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected override ProgressBar CreateViewInstance(ThemedReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[Views] ReactActivityIndicatorManager::CreateViewInstance BGN ");

            // create view component & set basic prop
            ProgressBar activityIndicator = new ProgressBar(ReactProgram.RctWindow)
            {
                Style = "process",
            };            
            
            activityIndicator.PlayPulse();

            activityIndicator.Show();            

            return activityIndicator;
        }

        /// <summary>
        /// Subclasses can override this method to install custom event 
        /// emitters on the given view.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <param name="view">The view instance.</param>
        protected override void AddEventEmitters(ThemedReactContext reactContext, ProgressBar view)
        {
            Log.Info(ReactConstants.Tag, "[Views] Register custom event , view:" + view );
            
        }

        /// <summary>
        /// Selection changed event handler.
        /// </summary>
        /// <param name="sender">an event sender.</param>
        /// <param name="e">the event.</param>
        
    }

    /* Event for ReactActivityIndicator */
    class ReactActivityIndicatorEvent : Event
    {
        private readonly int _index;

        public ReactActivityIndicatorEvent(int viewTag, int index)
            : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
        {
            _index = index;
        }

        public override string EventName
        {
            get
            {
                return "topSelectedChange";
            }
        }

        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            var eventData = new JObject
            {
                { "target", ViewTag },
                { "value", _index },
            };

            Log.Info(ReactConstants.Tag, "[Views] Dispatch Event >> name:Select, viewTag:" + ViewTag + ", cur postion:" + _index);
            eventEmitter.receiveEvent(ViewTag, EventName, eventData);
        }
    }
}

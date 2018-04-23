using System;
//using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

using ReactNative.UIManager;
using ReactNative.UIManager.Annotations;
using ReactNative.UIManager.Events;

using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;
using Tizen;
using ReactNative.Common;

//using System.Threading;

namespace ReactNative.Views.ReactProgressBar
{
    public class ReactProgressBarManager : SimpleViewManager<ProgressBar>
    {
        private const string ReactClass = "RCTProgressBar";

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
                        "topValueChange",
                        new Dictionary<string, object>
                        {
                            { "registrationName", "onValueChange" }
                        }
                    },
                };
            }
        }

        [ReactProp("isPulseMode")]
        public void IsPulseMode(ProgressBar view, bool bPulseMode)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] set PulseMode:'{bPulseMode}'");

            view.IsPulseMode = bPulseMode;

            if(bPulseMode)
            {
                view.PlayPulse();
            }
            else
            {
                view.StopPulse();
            }
        }

        [ReactProp("playPulse")]
        public void PlayPulse(ProgressBar view, bool bPulse)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] Set playPulse:'{bPulse}'");
            
            view.PlayPulse();  
        }

        [ReactProp("stopPulse")]
        public void StopPulse(ProgressBar view, bool bPulse)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] Set stopPulse:'{bPulse}'");
            
            view.StopPulse();
        }


        [ReactProp("horizontal")]
        public void IsHorizontal(ProgressBar view, bool bHorizontal)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] IsHorizontal:'{bHorizontal}'");

            view.IsHorizontal = bHorizontal;
        }

        [ReactProp("inverted")]
        public void IsInverted(ProgressBar view, bool bInverted)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] IsInverted:'{bInverted}'");

            view.IsInverted = bInverted;
        }

        [ReactProp("value")]
        public void SetProgressValue(ProgressBar view, double value)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] Value:'{value}'");

            view.Value = value;
        }

		/*
        [ReactProp("SpanSize")]
        public void SetSpanSize(ProgressBar view, int spanSize)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] SpanSize:'{spanSize}'");

            view.SpanSize = spanSize;
        }

        [ReactProp("UnitFormat")]
        public void SetUnitFormat(ProgressBar view, string unitFormat)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] UnitFormat:'{unitFormat}'");

            view.UnitFormat = unitFormat;
        }
		*/

        /// <summary>
        /// Sets the front color of the view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(ViewProps.Color, CustomType = "Color")]
        public void SetColor(ProgressBar view, uint? color)
        {
            Log.Info(ReactConstants.Tag, $"[Views::PB] Color is:'{color.HasValue}'");
            if (color.HasValue)
            {
                var c = ColorHelpers.Parse(color.Value);
                view.Color = c;

                Tracing.RNTracer.Write(Common.ReactConstants.Tag, "SetColor = " + c.ToString() + ", from " + color.Value);
            }
        }

        /// <summary>
        /// Sets the background color of the view.
        /// </summary>
        /// <param name="view">The view instance.</param>
        /// <param name="color">The masked color value.</param>
        [ReactProp(
            ViewProps.BackgroundColor,
            CustomType = "Color",
            DefaultUInt32 = ColorHelpers.Transparent)]
        public void SetBackgroundColor(ProgressBar view, uint color)
        {
            view.BackgroundColor = Color.FromUint(color);
        }
        

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
            Log.Info(ReactConstants.Tag, "[Views] ReactProgressBarManager::ReceiveCommand");
        }

        /// <summary>
        /// Creates a new view instance of type <see cref="ProgressBar"/>.
        /// </summary>
        /// <param name="reactContext">The react context.</param>
        /// <returns>The view instance.</returns>
        protected override ProgressBar CreateViewInstance(ThemedReactContext reactContext)
        {
            Log.Info(ReactConstants.Tag, "[Views] ReactProgressBarManager::CreateViewInstance BGN ");

            // create view component & set basic prop
            ProgressBar pb = new ProgressBar(ReactProgram.RctWindow)
            {
                //Style = "default",
                //Color = Color.Orange,
                //SpanSize = 400,
                //Value = 0.5
            };            

            pb.Show();            

            return pb;
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
            view.ValueChanged += OnValueChanged;
        }

        /// <param name="reactContext">The React context.</param>
        /// <param name="view">The view.</param>
        public override void OnDropViewInstance(ThemedReactContext reactContext, ProgressBar view)
        {
            Log.Info(ReactConstants.Tag, "[Views] OnDropViewInstance PB:" + view);
            view.ValueChanged -= OnValueChanged;
        }


        /// <summary>
        /// Selection changed event handler.
        /// </summary>
        /// <param name="sender">an event sender.</param>
        /// <param name="e">the event.</param>
        private void OnValueChanged(object sender, EventArgs e)
        {
            var btn = (ProgressBar)sender;
            Log.Info(ReactConstants.Tag, "[Views] # You change the value. Notify to JS # ");
            
            btn.GetReactContext()
                .GetNativeModule<UIManagerModule>()
                .EventDispatcher
                .DispatchEvent(
                    new ReactPbValueChangedEvent(btn.GetTag()));
        }

        
    }


    class ReactPbValueChangedEvent : Event
    {
        public ReactPbValueChangedEvent(int viewTag)
            : base(viewTag, TimeSpan.FromTicks(Environment.TickCount))
        {
        }

        /// <summary>
        /// The event name.
        /// </summary>
        public override string EventName
        {
            get
            {
                return "topValueChange";
            }
        }

        /// <summary>
        /// Disabling event coalescing.
        /// </summary>
        /// <remarks>
        /// Return false if the event can never be coalesced.
        /// </remarks>
        public override bool CanCoalesce
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Dispatch this event to JavaScript using the given event emitter.
        /// </summary>
        /// <param name="eventEmitter">The event emitter.</param>
        public override void Dispatch(RCTEventEmitter eventEmitter)
        {
            var eventData = new JObject()
            {
                { "target", ViewTag },
            };

            eventEmitter.receiveEvent(ViewTag, EventName, eventData);
        }
    }

    
}

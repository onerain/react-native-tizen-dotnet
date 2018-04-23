using System;

using ReactNative.UIManager.Events;
using Newtonsoft.Json.Linq;

namespace ReactNative.Views.ReactButton
{
    class ReactButtonClickEvent : Event
    {
        public ReactButtonClickEvent(int viewTag) 
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
                return "topPress";
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

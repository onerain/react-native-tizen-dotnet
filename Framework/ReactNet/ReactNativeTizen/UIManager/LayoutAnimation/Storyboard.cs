using System;
using ElmSharp;

namespace ReactNative.UIManager.LayoutAnimation
{

/// Wrapper for story board , which following WPF
public class Storyboard
{
    public EventHandler Completed;

    public TimelineCollection Children { get; set; }

    public Storyboard()
    {
        // TODO:
    }

    public static void SetTarget(Timeline tl, Widget widget)
    {
        // TODO:
    }

    public void Begin()
    {
        // TODO:
    }

    public void Stop()
    {
        // TODO:
    }


}
}
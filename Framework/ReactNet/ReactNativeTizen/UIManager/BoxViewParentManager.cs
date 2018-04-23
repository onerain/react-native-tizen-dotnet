using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;


using Tizen;
using ReactNative.Common;

namespace ReactNative.UIManager
{
    /// <summary>
    /// Class providing child management API for view managers of classes
    /// extending <see cref="TBox"/>.
    /// </summary>
    /// <typeparam name="TBox">Type of panel.</typeparam>
    public abstract class BoxViewParentManager<TBox> : ViewParentManager<TBox>
        where TBox : ReactBox
    {
        /// <summary>
        /// Gets the number of children for the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <returns>The number of children.</returns>
        public override int GetChildCount(TBox parent)
        {
            //Log.Info(ReactConstants.Tag, "[BoxViewParentManager::GetChildCount]->  parent: " + parent + ", child_count:" + parent.GetChildrenCount());
            return parent.GetChildrenCount();
        }

        /// <summary>
        /// Gets the child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="index">The index.</param>
        /// <returns>The child view.</returns>
        public override Widget GetChildAt(TBox parent, int index)
        {
            //Log.Info(ReactConstants.Tag, "[BoxViewParentManager::GetChildAt]->  parent: " + parent + ", index:" + index);
            return (Widget)parent.GetChildAt(index) ;
        }

        /// <summary>
        /// Adds a child at the given index.
        /// </summary>
        /// <param name="parent">The parent view.</param>
        /// <param name="child">The child view.</param>
        /// <param name="index">The index.</param>
        public sealed override void AddView(TBox parent, Widget child, int index)
        {
            // check whether 'index' is occupied
            //if (null != parent.GetChildAt(index))
            //{
            //    parent.RemoveChildAt(index);   // If 'Directory' is almost same as which in C++, I need to do this.  ^ ^
            //    return;
            //}
            //else
            {
                //Log.Info(ReactConstants.Tag, "[BoxViewParentManager::AddView]->  parent: " + parent + ", index:" + index);

                // Add to Box view
                parent.PackEnd(child);

                // Saved in MAP DB
                parent.AddChild(child, index);
            }
        }

        /// <summary>
        /// Removes the child at the given index.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        /// <param name="index">The index.</param>
        public override void RemoveChildAt(TBox parent, int index)
        {
            //Log.Info(ReactConstants.Tag, "[BoxViewParentManager::RemoveChildAt]->  parent: " + parent + ", index:" + index);
            // 1. Get view via index, then remove it
            EvasObject content = parent.GetChildAt(index);
            parent.UnPack(content);

            // 2. Remove from MAP
            parent.RemoveChildAt(index);
        }

        /// <summary>
        /// Removes all children from the view parent.
        /// </summary>
        /// <param name="parent">The view parent.</param>
        public override void RemoveAllChildren(TBox parent)
        {
            //Log.Info(ReactConstants.Tag, "[BoxViewParentManager::RemoveChildAt]->  parent: " + parent);

            // 1. Remove all child in current box
            parent.UnPackAll();

            // 2. Clean MAP
            parent.ClearAllChildren();
        }
    }
}

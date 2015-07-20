using System.Windows;
using System.Windows.Interactivity;

namespace Bookie
{
    public class Behaviours : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.LostFocus += (s, e) => AssociatedObject.Topmost = true;
        }
    }
}
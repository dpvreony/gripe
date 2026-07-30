namespace Gripe.Testing.Winforms
{
    /// <summary>
    /// Simple WPF Window for testing purposes.
    /// </summary>
    public sealed class SimpleWindow : System.Windows.Window
    {
        /// <summary>
        /// Shows another window instance. Used to ensure GR0052 is NOT triggered for WPF Window.Show().
        /// </summary>
        /// <example>
        /// <code>
        /// var window = new SimpleWindow();
        /// window.ShowChildWindow();
        /// </code>
        /// </example>
        public void ShowChildWindow()
        {
            var child = new SimpleWindow();
            child.Show();
        }
    }
}

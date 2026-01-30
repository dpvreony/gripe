namespace Gripe.Testing.Winforms
{
    /// <summary>
    /// Simple Windows Form for testing purposes.
    /// </summary>
    public sealed class SimpleForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Calls a message box from a non-member method. Used to ensure GR0052 is NOT triggered.
        /// </summary>
        /// <example>
        /// <code>
        /// var form = new SimpleForm();
        /// form.CallMessageBoxAsMember();
        /// </code>
        /// </example>
        public void CallMessageBoxAsMember()
        {
            System.Windows.Forms.MessageBox.Show("This is a message box NOT called from a UI element member.");
        }

        /// <summary>
        /// Calls a message box from a non-member method. Used to ensure GR0052 is triggered.
        /// </summary>
        /// <example>
        /// <code>
        /// SimpleForm.CallMessageBoxAsNonMember();
        /// </code>
        /// </example>
        public static void CallMessageBoxAsNonMember()
        {
            System.Windows.Forms.MessageBox.Show("This is a message box called from a UI element member.");
        }
    }
}

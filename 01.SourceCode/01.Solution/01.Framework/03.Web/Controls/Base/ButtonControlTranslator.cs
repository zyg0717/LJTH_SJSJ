using System.Web.UI.WebControls;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonControlTranslator : ControlTranslatorGenericBase<IButtonControl>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public ButtonControlTranslator(IButtonControl control)
            : base(control)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Translate()
        {
            if (CategoryDefined)
                Control.Text = Translate(Control.Text);
        }
    }
}

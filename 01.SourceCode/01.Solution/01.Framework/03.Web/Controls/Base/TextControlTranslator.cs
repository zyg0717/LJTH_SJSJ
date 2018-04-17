using System.Web.UI;

namespace Framework.Web.Controls
{
    /// <summary>
    /// For Label, TextBox
    /// </summary>
    public class TextControlTranslator : ControlTranslatorGenericBase<ITextControl>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public TextControlTranslator(ITextControl control)
            : base(control)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Translate()
        {
            if (CategoryDefined)
            {
                this.Control.Text = Translate(this.Control.Text);
            }
        }
    }
}

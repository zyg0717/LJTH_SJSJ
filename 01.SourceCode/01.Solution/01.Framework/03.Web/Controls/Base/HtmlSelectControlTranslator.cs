using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlSelectControlTranslator : ControlTranslatorGenericBase<HtmlSelect>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public HtmlSelectControlTranslator(HtmlSelect control) :
            base(control)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Translate()
        {
            if (CategoryDefined)
            {
                foreach (ListItem item in this.Control.Items)
                {
                    item.Text = Translate(item.Text);
                }
            }
        }
    }
}

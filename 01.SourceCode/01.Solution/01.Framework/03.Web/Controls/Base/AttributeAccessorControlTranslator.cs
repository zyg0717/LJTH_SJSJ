using System.Web.UI;
using Framework.Core;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class AttributeAccessorControlTranslator : ControlTranslatorGenericBase<IAttributeAccessor>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public AttributeAccessorControlTranslator(IAttributeAccessor control)
            : base(control)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Translate()
        {
            TranslateAttribute("title");
        }

        /// <summary>
        /// 
        /// </summary>
        private void TranslateAttribute(string attrName)
        {
            ExceptionHelper.CheckStringIsNullOrEmpty(attrName, "attrName");

            if (CategoryDefined)
            {
                string text = this.AttributeAccessor.GetAttribute(attrName);

                if (string.IsNullOrEmpty(text) == false)
                {
                    this.AttributeAccessor.SetAttribute(attrName, Translate(text));
                }
            }
        }
    }
}

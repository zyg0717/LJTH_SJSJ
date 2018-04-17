using System;
using System.Web.UI;

namespace Framework.Web.Controls
{
    /// <summary>
    /// Describes an object that can be used to resolve references to a control by its ID
    /// </summary>
    public interface IControlResolver
    {
        /// <summary>
        /// Resolves a reference to a control by its ID
        /// </summary>
        /// <param name="controlId">控件ID</param>
        /// <returns></returns>
        Control ResolveControl(string controlId);
    }
}

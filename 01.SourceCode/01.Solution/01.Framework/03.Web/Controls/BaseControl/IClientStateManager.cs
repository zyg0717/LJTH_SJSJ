using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Web.Controls
{
    /// <summary>
    /// Describes an object which supports ClientState
    /// </summary>
    public interface IClientStateManager
    {
        /// <summary>
        /// Whether ClientState is supported by the object instance
        /// </summary>
        bool SupportsClientState { get; }
        /// <summary>
        /// Loads the client state for the object
        /// </summary>
        /// <param name="clientState"></param>
        void LoadClientState(string clientState);
        /// <summary>
        /// Saves the client state for the object
        /// </summary>
        /// <returns></returns>
        string SaveClientState();
    }
}

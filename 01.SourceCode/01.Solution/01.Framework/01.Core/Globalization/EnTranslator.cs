using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Framework.Core.Globalization
{
    public class EnTranslator : ITranslator
    {
        public string Translate(string category, CultureInfo sourceCulture, string sourceText, CultureInfo targetCulture, params object[] objParams)
        {
            throw new NotImplementedException();
        }
    }
}

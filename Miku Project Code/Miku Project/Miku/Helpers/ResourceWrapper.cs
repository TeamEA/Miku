using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Miku.Client.Assets.Resources;

namespace Miku.Client.Helpers
{
    class ResourceWrapper
    {
        private ApplicationStrings applicationStrings = new ApplicationStrings();
        /// <summary>
        /// Gets the <see cref="ApplicationStrings"/>.
        /// </summary>
        public ApplicationStrings ApplicationStrings
        {
            get { return applicationStrings; }
        }
    }
}

using System.Collections.Generic;

namespace Vip.ComboBox
{
    /// <summary>
    ///     The result of a match
    ///     There are the items we store in the suggestion listbox
    /// </summary>
    public class StringMatch
    {
        /// <summary>
        ///     The original source
        /// </summary>
        public string Text { get; internal set; }

        /// <summary>
        ///     The source decomposed on match/non matches against the pattern
        /// </summary>
        public List<string> Segments { get; internal set; }

        /// <summary>
        ///     Is the first segment a match?
        /// </summary>
        public bool StartsOnMatch { get; internal set; }
    }
}
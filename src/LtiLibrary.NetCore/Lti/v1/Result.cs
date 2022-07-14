namespace LtiLibrary.NetCore.Lti.v1
{
    /// <summary>
    /// Represents an Outcomes 1.0 result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Get or set the score for this result. Can be null.
        /// </summary>
        public double? Score { get; set; }

        #region Optional submission details supported by Canvas 

        /// <summary>
        /// Get or set optional submission detail text. Can contain HTML. Supported by Canvas.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set optional submission detail URL. Supported by Canvas.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Get or set optional submission detail LTI Launch URL. Supported by Canvas.
        /// </summary>
        public string LtiLaunchUrl { get; set; }

        #endregion

        /// <summary>
        /// Get or set the SourcedId for this result.
        /// </summary>
        public string SourcedId { get; set; }
    }
}


using Sitecore.Analytics.Pipelines.CommitSession;
using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using System;
using System.Linq;

namespace Sitecore.Support.Sitecore.Analytics.Pipelines.CommitSession
{
    public class UpdateEndDateTime : CommitSessionProcessor
    {
        #region Public methods

        /// <summary>
        /// The pipeline process method that updates the EndDateTime data.
        /// </summary>
        /// <param name="args">
        /// The UpdateEndDateTime argument instance.
        /// </param>
        public override void Process([NotNull] CommitSessionPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            Assert.IsNotNull(args.Session.Interaction, "args.Session.Interaction");

            if (args.Session.Interaction.EndDateTime < args.Session.Interaction.GetPages().Last<IPageContext>().DateTime)
            {
                // Attempt to update end date time if not done earlier in the visit.
                int num = args.Session.Interaction.GetPages().Sum<IPageContext>((Func<IPageContext, int>)(d => d.Duration));
                args.Session.Interaction.EndDateTime = args.Session.Interaction.StartDateTime.AddMilliseconds((double)num);
            }
        }

        #endregion
    }
}
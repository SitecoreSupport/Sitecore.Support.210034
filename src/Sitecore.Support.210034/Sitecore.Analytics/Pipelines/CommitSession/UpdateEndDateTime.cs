
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
            var lastPage = args.Session.Interaction.GetPages().Last<IPageContext>();
            if (args.Session.Interaction.EndDateTime < lastPage.DateTime)
            {
                // Attempt to update end date time if not done earlier in the visit.
                SetDuration(lastPage, args.Session.Interaction);
                int num = args.Session.Interaction.GetPages().Sum<IPageContext>((Func<IPageContext, int>)(d => d.Duration));
                args.Session.Interaction.EndDateTime = args.Session.Interaction.StartDateTime.AddMilliseconds((double)num);
            }
        }


        private void SetDuration(IPageContext page, CurrentInteraction interation)
        {
            if (page.CanSetDuration)
            {
                IPageContext previousPage = interation.PreviousPage;
                if (previousPage != null)
                {
                    TimeSpan span = (TimeSpan)(page.DateTime - previousPage.DateTime);
                    previousPage.Duration = (int)span.TotalMilliseconds;
                }
            }
        }
        #endregion
    }
}
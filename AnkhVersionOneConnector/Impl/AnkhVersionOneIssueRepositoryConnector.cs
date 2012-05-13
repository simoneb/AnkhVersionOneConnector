using System.Runtime.InteropServices;
using Ankh.ExtensionPoints.IssueTracker;

namespace AnkhVersionOneConnector.Impl
{
    [Guid(Constants.ServiceGUID)]
    public class AnkhVersionOneIssueRepositoryConnector : IssueRepositoryConnector
    {
        public override IssueRepository Create(IssueRepositorySettings settings)
        {
            return settings != null && Name.Equals(settings.ConnectorName)
                       ? VersionOneRepository.Create(settings)
                       : null;
        }

        public override string Name
        {
            get { return Constants.ConnectorName; }
        }

        public override IssueRepositoryConfigurationPage ConfigurationPage
        {
            get { return new VersionOneConfigurationPagePresenter(); }
        }
    }
}
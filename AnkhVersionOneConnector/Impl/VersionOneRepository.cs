using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ankh.ExtensionPoints.IssueTracker;
using AnkhVersionOneConnector.Impl.Controls;
using VersionOne.SDK.ObjectModel;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace AnkhVersionOneConnector.Impl
{
    public class VersionOneRepository : IssueRepository, IWin32Window, IDisposable
    {
        public const string PropertyUsername = "username";
        public const string PropertyPassword = "passcode";
        public const string PropertyIntegratedAuthentication = "integratedAuthentication";

        private readonly Uri _repositoryUri;
        private readonly string _repositoryId;
        private readonly IDictionary<string, object> _customProperties;
        private IssuesListView _issueListView;
        private V1Instance _instance;
        private Task<WorkItemService> _getWorkItemService;

        public VersionOneRepository(Uri repositoryUri, string repositoryId, IDictionary<string, object> customProperties) : base(Constants.ConnectorName)
        {
            _repositoryUri = repositoryUri;
            _repositoryId = repositoryId;
            _customProperties = customProperties;

            _getWorkItemService = Task.Factory.StartNew<WorkItemService>(CreateWorkItemService);
        }

        private WorkItemService CreateWorkItemService()
        {
            return new WorkItemService(V1Instance);
        }

        public static IssueRepository Create(IssueRepositorySettings settings)
        {
            return settings != null ? new VersionOneRepository(settings.RepositoryUri, settings.RepositoryId, settings.CustomProperties) : null;
        }

        private V1Instance CreateVersionOneInstance()
        {
            // may want to support explicit user integrated authentication some time in the future (format of username should be user@domain)
            var instance = bool.Parse(_customProperties[PropertyIntegratedAuthentication].ToString()) ? 
                new V1Instance(_repositoryUri.ToString(), null, null, true) : 
                new V1Instance(_repositoryUri.ToString(), (string) _customProperties[PropertyUsername], (string) _customProperties[PropertyPassword]);

            try
            {
                instance.Validate();
            }
            catch (ApplicationUnavailableException e)
            {
                MessageBox.Show(string.Format("VersionOne application is unavailable ({0})", _repositoryUri), "VersionOne application unavailable", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return null;
            }
            catch(AuthenticationException e)
            {
                MessageBox.Show("Cannot authenticate to VersionOne instance with the credentials supplied", "VersionOne authentication error",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return instance;
        }

        public V1Instance V1Instance { get { return _instance ?? (_instance = CreateVersionOneInstance()); } }

        public override Uri RepositoryUri
        {
            get { return _repositoryUri; }
        }

        public override string RepositoryId
        {
            get { return _repositoryId; }
        }

        public override IDictionary<string, object> CustomProperties
        {
            get { return _customProperties; }
        }

        public override string Label
        {
            get { return RepositoryId ?? (RepositoryUri == null ? string.Empty : RepositoryUri.ToString()); }
        }

        public override string IssueIdPattern
        {
            get
            {
                return @"(?<id>\w+-\d+)(\s*?(?<id>\w+-\d+))*";
            }
        }

        public override void PreCommit(PreCommitArgs args)
        {
            base.PreCommit(args);

            if(!IssuesListView.SelectedWorkItems.Any())
            {
                MessageBox.Show("You must select at least one issue before you can commit your changes",
                                "No issue selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                args.Cancel = true;

                return;
            }

            var commitMessage = new StringBuilder();

            foreach (var issue in IssuesListView.SelectedWorkItems)
                commitMessage.Append(issue.DisplayID).Append(" ");

            commitMessage.Append(args.CommitMessage);

            args.CommitMessage = commitMessage.ToString();
        }

        public override void PostCommit(PostCommitArgs args)
        {
            _issueListView.ClearSelection();

            base.PostCommit(args);
        }

        public override void NavigateTo(string issueId)
        {
            if (!string.IsNullOrWhiteSpace(issueId))
            {
                var url = V1Instance.Get.IssueByDisplayID(issueId).URL;

                Process.Start(url);
            }
        }

        public IntPtr Handle
        {
            get { return IssuesListView.Handle; }
        }

        private IssuesListView IssuesListView
        {
            get { return _issueListView ?? (_issueListView = new IssuesListView(_getWorkItemService.Result)); }
        }

        public void Dispose()
        {
            if (_issueListView != null && !_issueListView.IsDisposed && !_issueListView.Disposing)
                _issueListView.Dispose();
            
            _issueListView = null;

            if(_getWorkItemService != null)
            {
                if (_getWorkItemService.Result != null)
                    _getWorkItemService.Result.Dispose();

                _getWorkItemService.Wait(200);
                _getWorkItemService.Dispose();
                _getWorkItemService = null;
            }
        }
    }
}
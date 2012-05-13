using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ankh.ExtensionPoints.IssueTracker;

namespace AnkhVersionOneConnector.Impl.Controls
{
	public partial class VersionOneConfigurationPage : UserControl
	{
	    IssueRepositorySettings _settings;
	    private bool _processedSettings;
	    private bool _canProcessSettings;

	    public event EventHandler<ConfigPageEventArgs> OnPageEvent = delegate{};

	    public VersionOneConfigurationPage()
		{
			InitializeComponent();
		}

	    protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _canProcessSettings = true;
            SelectSettings();
        }

	    internal IssueRepositorySettings Settings
        {
            get
            {
                return CreateRepository();
            }
            set
            {
                _settings = value;
                SelectSettings();
            }
        }

	    private VersionOneRepository CreateRepository()
	    {
	        Uri uri;

	        return Uri.TryCreate(_url.Text.Trim(), UriKind.Absolute, out uri)
	                   ? new VersionOneRepository(uri, null, new Dictionary<string, object>
	                        {
	                            { VersionOneRepository.PropertyUsername, _user.Text.Trim()},
	                            { VersionOneRepository.PropertyPassword, _password.Text.Trim() },
	                            { VersionOneRepository.PropertyIntegratedAuthentication, _integratedAuthentication.Checked}
	                        })
	                   : null;
	    }

	    private void SelectSettings()
        {
            if (_settings != null && !_processedSettings && _canProcessSettings)
                try
                {
                    _url.Text = _settings.RepositoryUri.ToString();

                    if (_settings.CustomProperties != null)
                    {
                        object value;

                        if (_settings.CustomProperties.TryGetValue(VersionOneRepository.PropertyUsername, out value) && value != null)
                            _user.Text = value.ToString();
                        if (_settings.CustomProperties.TryGetValue(VersionOneRepository.PropertyPassword, out value) && value != null)
                            _password.Text = value.ToString();
                        if (_settings.CustomProperties.TryGetValue(VersionOneRepository.PropertyIntegratedAuthentication, out value) && value != null)
                            _integratedAuthentication.Checked = bool.Parse(value.ToString());
                    }
                }
                finally
                {
                    _processedSettings = true;
                }
        }

	    private bool IsPageComplete
        {
            get
            {
                Uri _;
                return Uri.TryCreate(_url.Text.Trim(), UriKind.Absolute, out _) &&
                       (_integratedAuthentication.Checked || (!string.IsNullOrWhiteSpace(_user.Text.Trim()) && !string.IsNullOrWhiteSpace(_password.Text.Trim())));
            }
        }

	    private void RaisePageEvent()
	    {
	        var args = new ConfigPageEventArgs();

	        try
	        {
	            args.IsComplete = IsPageComplete;
	        }
	        catch (Exception exc)
	        {
	            args.IsComplete = false;
	            args.Exception = exc;
	        }

	        OnPageEvent(this, args);
	    }

	    private void UrlTextChanged(object sender, EventArgs e)
	    {
	        RaisePageEvent();
	    }

	    private void UserTextChanged(object sender, EventArgs e)
        {
            RaisePageEvent();
        }

	    private void PasswordTextChanged(object sender, EventArgs e)
        {
            RaisePageEvent();
        }

	    private void IntegratedAuthenticationCheckedChanged(object sender, EventArgs e)
	    {
	        _user.Enabled = _password.Enabled = !_integratedAuthentication.Checked;
	        RaisePageEvent();
	    }
	}
}
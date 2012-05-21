using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using AnkhVersionOneConnector.Impl;
using Microsoft.VisualStudio.Shell;

namespace AnkhVersionOneConnector
{
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideService(typeof(AnkhVersionOneIssueRepositoryConnector), ServiceName = Constants.ConnectorName)]
    [ProvideIssueRepositoryConnector(typeof(AnkhVersionOneIssueRepositoryConnector), Constants.ConnectorName, typeof(AnkhVersionOneConnectorPackage), "#110")]
    [Guid(GuidList.guidAnkhVersionOneConnectorPkgString)]
    public sealed class AnkhVersionOneConnectorPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public AnkhVersionOneConnectorPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this));
        }
        
        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this));
            base.Initialize();

            ((IServiceContainer) this).AddService(typeof(AnkhVersionOneIssueRepositoryConnector), 
                (c, type) => c == this && type == typeof (AnkhVersionOneIssueRepositoryConnector) ? new AnkhVersionOneIssueRepositoryConnector() : null, true);

        }
    }
}
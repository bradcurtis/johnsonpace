using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace JP.Shared.PermissiveXFrameHeader.Features.PERMISSIVEXFRAME
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("8353bd37-823e-48e2-b84d-7999a6e36ad5")]
    public class PERMISSIVEXFRAMEEventReceiver : SPFeatureReceiver
    {
        private SPWebConfigModification[] Modifications
        {
            get
            {
                return new SPWebConfigModification[] {
                    new SPWebConfigModification("add[@name='PermissiveXFrameHeaderModule']", "configuration/system.webServer/modules")
                        { Owner = "PERMISSIVEXFRAME", Sequence = 0, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode, Value = @"<add name=""PermissiveXFrameHeaderModule"" type=""JP.Shared.PermissiveXFrameHeader.Modules.PermissiveXFrameHeaderModule, JP.Shared.PermissiveXFrameHeader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f16c625796e052d1"" />" }
                };
            }
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            foreach (SPWebConfigModification modification in this.Modifications)
            {
                webApp.WebConfigModifications.Add(modification);
            }

            webApp.WebService.ApplyWebConfigModifications();
            webApp.Update();
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWebApplication webApp = (SPWebApplication)properties.Feature.Parent;

            foreach (SPWebConfigModification modification in this.Modifications.Reverse())
            {
                webApp.WebConfigModifications.Remove(modification);
            }

            webApp.WebService.ApplyWebConfigModifications();
            webApp.Update();
        }
    }
}

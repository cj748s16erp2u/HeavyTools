using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageAttachmentSearchTab : eProjectWeb.Framework.UI.Templates.DetailSearchTabTemplate1
    {
        public static ApprovedPackageAttachmentSearchTab New(DefaultPageSetup attach)
        {
            var t = eProjectWeb.Framework.ObjectFactory.New<ApprovedPackageAttachmentSearchTab>();
            t.Initialize("apprattachment", attach, "$noroot_apprpackage", eProjectWeb.Framework.UI.Templates.DefaultActions.None);
            return t;
        }

        protected override void CreateBase()
        {
            RefreshGridOnActivate = false;

            base.CreateBase();

            SearchResults.MergePageData = "myattachkey";

            OnPageActivate += ApprovedPackageAttachmentSearchTab_OnPageActivate;

            var btnViewAttachment = new Button("preview", 1000);
            AddCmd(btnViewAttachment);
            SetButtonAction(btnViewAttachment.ID, new ControlEvent(btnViewAttachment_OnClick));

            var btnDownloadAttachment = new Button("download", 2000);
            AddCmd(btnDownloadAttachment);
            SetButtonAction(btnDownloadAttachment.ID, new ControlEvent(btnDownloadAttachment_OnClick));
        }

        protected void ApprovedPackageAttachmentSearchTab_OnPageActivate(eProjectWeb.Framework.PageUpdateArgs args)
        {
            int? pinvId = null;
            string packageCode = "";

            if ((args.PageData?.ContainsKey(Consts.DetailEntityKey)).GetValueOrDefault())
            {
                var detailEntityKey = args.PageData[Consts.DetailEntityKey] as Dictionary<string, object>;

                if (detailEntityKey.ContainsKey("pinvid"))
                {
                    pinvId = ConvertUtils.ToInt32(detailEntityKey["pinvid"]);
                    if (!pinvId.HasValue)
                        pinvId = 0;
                }
            }
            if ((args.PageData?.ContainsKey(Consts.RootEntityKey)).GetValueOrDefault())
            {
                var rootEntityKey = args.PageData[Consts.RootEntityKey] as Dictionary<string, object>;
                if (rootEntityKey.ContainsKey("packagecode"))
                {
                    packageCode = ConvertUtils.ToString(rootEntityKey["packagecode"]);
                }
            }

            Key k = new Key();
            if (pinvId.HasValue)
            {
                k.Add("selectedpinvid", pinvId);
            }
            if (!string.IsNullOrEmpty(packageCode))
            {
                k.Add("selectedpackagecode", packageCode);
            }

            args.PageData["myattachkey"] = k;
            if (k.Count() > 0)
                SearchResults.ForceRefresh = true;
        }

        protected void btnDownloadAttachment_OnClick(PageUpdateArgs args)
        {
            Key k = SearchResults.SelectedPK;

            if (k != null)
            {
                Base.Attach.Attachment at = Base.Attach.Attachment.Load(k);

                if (at != null)
                {
                    string realFileName = StringN.ConvertToString(at.Filename);
                    string mimeType = StringN.ConvertToString(at.Mimetype);
                    string storedFileName = StringN.ConvertToString(at.Storedfilename);
                    if (mimeType == null)
                        mimeType = string.Empty;

                    var sett = new eProjectWeb.Framework.UI.Script.DownloadSettings
                    {
                        ServicePrefix = "Services/DownloadService.ashx",
                        RealFileName = realFileName,
                        StoredFileName = storedFileName,
                        ContentType = mimeType,
                        Store = "attach"
                    };

                    args.AddExecCommand(new eProjectWeb.Framework.UI.Script.DownloadStep(sett));
                }
            }
        }

        protected void btnViewAttachment_OnClick(PageUpdateArgs args)
        {
            var k = SearchResults.SelectedPK;

            if (k != null)
            {
                var at = Base.Attach.Attachment.Load(k);

                if (at != null)
                {
                    string mimeType = at.Mimetype;
                    if (mimeType == null)
                    {
                        mimeType = string.Empty;
                    }

                    var sett = new eProjectWeb.Framework.UI.Script.OpenPopupSettings
                    {
                        ServicePrefix = "Services/DownloadService.ashx",
                        RealFileName = at.Filename,
                        StoredFileName = at.Storedfilename,
                        ContentType = mimeType,
                        Store = "attach"
                    };

                    args.AddExecCommand(new eProjectWeb.Framework.UI.Script.OpenPopupStep(sett));
                }
            }
        }

    }
}

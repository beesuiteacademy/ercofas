using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NetStarter.Enumeration;
using NetStarter.Helpers;
using NetStarter.Models;

namespace NetStarter.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private DefaultDBContext _db = new DefaultDBContext();

        // GET: Dashboard
        //Only user with ViewRight = true for Dashboard module can access this
        //Empty value for AddRight, EditRight, DeleteRight parameters means no need to validate that
        [CustomAuthorizeFilter(ProjectEnum.ModuleCode.Dashboard, "true", "", "", "")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            TotalCountViewModel model = new TotalCountViewModel()
            {
                TotalRegistrations = _db.PreRegistration.Count(),
                TotalAmendments = 0,
                TotalPreFilings = _db.PreFiledCases.Count(),
                TotalLetterCorrespondents = 0,
                TotalInitiatorySubsequentPleadings = 0,
                TotalValidatioMeterSamplings = 0,
                TotalSealingRequestTestingMeters = 0,
                TotalVirtualHearings = _db.Hearings.Count(),
                TotalTransactions = 0,
                TotalRetailElectricitySuppliers = 0
            };

            if (RoleHelpers.GetMainRole() == UserTypeEnum.Admin.ToString() || RoleHelpers.GetMainRole() == UserTypeEnum.SuperAdmin.ToString())
                return View(model);
            else
                return Redirect("Stakeholder");
        }

        #region Get

        // GET: Dashboard/Stakeholder
        [AllowAnonymous]
        public ActionResult Stakeholder()
        {
            string userId = User.Identity.GetUserId();

            TotalCountViewModel model = new TotalCountViewModel()
            {
                TotalPreFilings = _db.PreFiledCases.Count(x => x.UserId == userId),
                TotalSealingRequestTestingMeters = 0,
                TotalVirtualHearings = _db.Hearings.Count(),
                TotalInServiceMeterSamplings = 0,
                TotalRetailElectricitySuppliers = 0,
                TotalTransactions = 0
            };

            return View(model);
        }

        #endregion Get

        #region Protected

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion Protected
    }
}


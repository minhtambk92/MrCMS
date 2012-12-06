﻿using System.Linq;
using System.Web.Mvc;
using MrCMS.Entities.Multisite;
using MrCMS.Entities.People;
using MrCMS.Models;
using MrCMS.Services;
using MrCMS.Website;
using NHibernate;
using MrCMS.Helpers;

namespace MrCMS.Web.Areas.Admin.Controllers
{
    public class NavigationController : AdminController
    {
        private readonly INavigationService _service;
        private readonly IUserService _userService;
        private readonly ISitesService _sitesService;

        public NavigationController(INavigationService service, IUserService userService, ISitesService sitesService)
        {
            _service = service;
            _userService = userService;
            _sitesService = sitesService;
        }

        public PartialViewResult WebSiteTree()
        {
            var sites = _sitesService.GetAllSites();
            var currentSite = _sitesService.GetCurrentSite();
            return PartialView("WebsiteTreeList",
                               new WebsiteTreeListModel(sites.BuildSelectItemList(site => site.Name,
                                                                                  site => site.Id.ToString(),
                                                                                  site => site == currentSite,
                                                                                  emptyItemText: null),
                                                        sites.Select(site => _service.GetWebsiteTree(site)).ToList()));
        }

        public PartialViewResult MediaTree()
        {
            return PartialView("MediaTree", _service.GetMediaTree());
        }

        public PartialViewResult LayoutTree()
        {
            return PartialView("LayoutTree", _service.GetLayoutList());
        }

        public PartialViewResult UserList()
        {
            return PartialView("UserList", _service.GetUserList());
        }

        [ChildActionOnly]
        public PartialViewResult LoggedInAs()
        {
            User user = _userService.GetCurrentUser(HttpContext);
            return PartialView(user);
        }
    }
}
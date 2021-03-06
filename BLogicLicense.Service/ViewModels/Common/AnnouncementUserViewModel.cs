﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLogicLicense.Service.ViewModels.Common
{
    public class AnnouncementUserViewModel
    {
        public int AnnouncementId { get; set; }

        public string UserId { get; set; }

        public bool HasRead { get; set; }

        public virtual AppUserViewModel AppUser { get; set; }

        public virtual AnnouncementViewModel Announcement { get; set; }
    }
}
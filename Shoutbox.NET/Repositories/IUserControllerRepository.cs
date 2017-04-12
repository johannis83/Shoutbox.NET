﻿using Shoutbox.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoutbox.NET.Repositories
{
    public interface IUserRepository
    {
        User CreateFromUserPrincipal(ActiveDirectoryUser activeDirectoryUserObject);
        User GetByLogonUser(string username);
        void SaveGridLayout(string logonUser, string serializedLayout);
        string GetGridLayout(string logonUser);
        void SaveNotificationSettings(string logonUser, string serializedSettings);
    }
}

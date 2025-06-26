﻿using EcommerceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceSystem.Application.Interfaces
{
    public interface IUserService
    {
        User? ValidateUser(string email);
    }
}

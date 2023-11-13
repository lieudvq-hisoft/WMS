﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model;

public class Token
{
    public string Access_token { get; set; }
    public string TokenType { get; set; }
    public string UserID { get; set; }
    public int ExpiresIn { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserAva { get; set; }
    public int CurrenNoticeCount { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}

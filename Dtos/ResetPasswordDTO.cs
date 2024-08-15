using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Dtos
{
    public class ResetPasswordDTO
    {
    public string Username { get; set; }  // This is the email

    public string NewPassword { get; set; }
    }
}
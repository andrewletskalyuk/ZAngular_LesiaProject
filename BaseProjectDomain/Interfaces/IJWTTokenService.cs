using BaseProjectDataContext.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProjectDomain.Interfaces
{
    public interface IJWTTokenService
    {
        string CreateToken(User user);
    }
}

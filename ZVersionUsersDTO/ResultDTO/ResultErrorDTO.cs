using System;
using System.Collections.Generic;
using System.Text;

namespace ZVersionUsersDTO.ResultDTO
{
    public class ResultErrorDTO : ResultDTO
    {
        public List<string> Errors { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement;

public class ApiResponse
{
    public string resultCd { get; set; }
    public List<User> resultData { get; set; }
}


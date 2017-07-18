using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProjectDemo.Models
{
    public partial class DropBoxAuthentication { public int ID { get; set; } public string Token { get; set; } public string Secret { get; set; } public string AppKey { get; set; } public string AppSecret { get; set; } public Nullable<int> DropBoxUserID { get; set; } }
}

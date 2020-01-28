using System;
using System.Web.UI;

namespace Application.Web.TestPages
{
    public partial class CrashingPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            throw new Exception("crash");
        }
    }
}
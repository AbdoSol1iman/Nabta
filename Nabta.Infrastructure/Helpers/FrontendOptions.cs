using System;
using System.Collections.Generic;
using System.Text;

namespace Nabta.Infrastructure.Helpers
{

    public class FrontendOptions
    {
        public string BaseUrl { get; set; } = string.Empty;

        public string ConfirmEmailPath { get; set; } = string.Empty;

        public string ResetPasswordPath { get; set; } = string.Empty;
    }
}

#pragma checksum "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b8b4c70be6577e6a39d4b371c31ee453515eb1a2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_PrivateInfo), @"mvc.1.0.view", @"/Views/Home/PrivateInfo.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\_ViewImports.cshtml"
using FirstProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\_ViewImports.cshtml"
using FirstProject.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b8b4c70be6577e6a39d4b371c31ee453515eb1a2", @"/Views/Home/PrivateInfo.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cfa503ef3de3887ad277480ed4d6261c659b6922", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_PrivateInfo : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml"
  
	ViewData["Title"] = "Profile information";
	var user = await UserManager.GetUserAsync(User);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>Private Information</h2>\r\n<div class=\"table-bordered\">\r\n\t<table style=\"width:100%;\">\r\n\t\t<tr>\r\n\t\t\t<th>Id</th>\r\n\t\t\t<th>Username</th>\r\n\t\t\t<th>Email</th>\r\n\t\t\t<th>Phone number</th>\r\n\t\t</tr>\r\n\t\t<tr>\r\n\t\t\t<td>");
#nullable restore
#line 19 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml"
            Write(user.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t\t\t<td>");
#nullable restore
#line 20 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml"
            Write(user.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t\t\t<td>");
#nullable restore
#line 21 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml"
            Write(user.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t\t\t<td>");
#nullable restore
#line 22 "C:\ITechArtGit\TestRep\FirstProject\FirstProject\Views\Home\PrivateInfo.cshtml"
            Write(user.PhoneNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n\t\t</tr>\r\n\t</table>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<IdentityUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public UserManager<IdentityUser> UserManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591

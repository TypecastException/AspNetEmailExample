Asp.Net Email Example
=====================

This project is an example used in an article demonstrating a basic approach to sending personalized emails to recipients selected from a list.

The Blog article to accompany this code can be found at [Send Email to Selected Recipients from your ASP.NET MVC Web Application][1]

Use Nuget Package Restore
=========================

You will need to enable Nuget Package Restore in Visual Studio in order to download and restore Nuget packages during build. If you are not sure how to do this, see [Keep Nuget Packages Out of Source Control with Nuget Package Manager Restore][2]

Run Entity Framework Migrations
===============================

You will also need to run Entity Framework Migrations `Update-Database` command per the article. The migration files are included in the repo, so you will NOT need to `Enable-Migrations` or run `Add-Migration Init`. You will likely want to edit the Admin user name and password in the seed method before doing this. 



[1]: http://typecastexception.com/post/2014/01/15/Send-Email-to-Selected-Recipients-from-your-ASPNET-MVC-Web-Application.aspx "Send Email to Selected Recipients from your ASP.NET MVC Web Application"

[2]: http://www.typecastexception.com/post/2013/11/10/Keep-Nuget-Packages-Out-of-Source-Control-with-Nuget-Package-Manager-Restore.aspx "Keep Nuget Packages Out of Source Control with Nuget Package Manager Restore"

# Google calendar synchronization with Blazor Scheduler

This sample explains about how to synchronize the google calendar with Blazor Scheduler.

## Prerequisites

* Visual Studio 2022

## How to run the project

* Checkout this project to a location in your disk.
* Open the solution file using the Visual Studio 2022.
* Restore the NuGet packages by rebuilding the solution.
* Run the project.

To run this application, we have to Turn on the Google Calendar API and DOWNLOAD CLIENT CONFIGURATION and save the file credentials.json in root directory. Refer below link to Turn on google calendar API.

**link**: https://developers.google.com/calendar/api/quickstart/js#step_1_turn_on_the

> At the first time of running sample, it will attempt to open a new browser window, in that you have to authorize it.

**Blog**: [How to Synchronize Google Calendar with Syncfusion Blazor Scheduler](https://www.syncfusion.com/blogs/post/synchronize-google-calendar-with-syncfusion-blazor-scheduler.aspx)

## FAQ

### Error 400: redirect_uri_mismatch

**Q: I keep getting "Error 400: redirect_uri_mismatch" when trying to authenticate with Google Calendar. How can I fix this?**

**A:** To resolve this error, create the OAuth Client ID as a "Desktop app" application type rather than a "Web application".

**Error Screenshot:**

![Error 400: redirect_uri_mismatch](wwwroot/faq/error_400_redirect_uri_mismatch.png)

**Solution:**

Select "Desktop app" as the application type when creating the OAuth client ID:

![Select Desktop app solution](wwwroot/faq/error_400_redirect_uri_mismatch_solution.png)
Installing: 
Identity DB:
	update-database -ConnectionStringName "DefaultConnection" -ConfigurationTypeName "Configuration"

Initialize (create) database: update-database -configurationtypename:"DALConfiguration"

Restoring SQL DB
If the DB Restore process is "stuck" in "restoring..." (with no recovery has probably been used), then run the following command after successful restoration of the DB:
"RESTORE DATABASE <database name> WITH RECOVERY" (without quotes)

EF 6 Update Database (run Code First Migrations on a remote SQL DB)
Command: update-database -targetmigration:<NAME_OF_MIGRATION> -ConnectionStringName "<NAME_OF_CONNECTION_STRING_FOR_REMOTE_SQL_DB>"

Scaffolding:
 * use x86-platform to enable scaffolding if it fails

Building:
	> use x86-platform when:
		* Generating EF View Entity Data Model (read-only)
		* (running update-database on project BootstrapModel)
	> use x64-platform when:
		* Running DB Convert (use: reg add HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\11.0\WebProjects /v Use64BitIISExpress /t REG_DWORD /d 1)
		* Enable IIS 64 bit version (see image 'IIS64bitVersion.JPG')

http://stackoverflow.com/questions/19843683/vs2013-debugger-entity-framework-runtime-has-refused-to-evaluate-the-express

Internationalization
Part1: http://afana.me/post/aspnet-mvc-internationalization.aspx
Part2: http://afana.me/post/aspnet-mvc-internationalization-part-2.aspx

Front-End:
A JavaScript library for internationalization and localization that leverage the official Unicode CLDR JSON data. The library works both for the browser and as a Node.js module
https://github.com/jquery/globalize

National Language Support (NLS) API Reference
http://www.microsoft.com/resources/msdn/goglobal/default.mspx

Code First Migrations
http://msdn.microsoft.com/en-us/data/jj591621

Software Licences
http://choosealicense.com/licenses/

Phone Number Parser Library
https://code.google.com/p/libphonenumber/

Dashboard Sidebar Bootstrap Template
License & Author Details
MIT by Bootply.com
http://bootstrapzero.com/bootstrap-template/dashboard-sidebar

Underscore.js
https://github.com/jashkenas/underscore/blob/master/LICENSE

Web API
http://www.asp.net/web-api/overview/creating-web-apis/creating-api-help-pages

Bootstrap Datepicker
http://vitalets.github.io/bootstrap-datepicker/

Problems encountered
http://stackoverflow.com/questions/21918000/mvc5-vs2012-identity-createidentityasync-value-cannot-be-null

Knockout Kendo
http://rniemeyer.github.io/knockout-kendo/

jQuery Hotkeys
https://github.com/jeresig/jquery.hotkeys

Bootstrap datepicker
https://github.com/vitalets/bootstrap-datepicker
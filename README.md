# Notification Portal - BCIT Industry Project 2017 

Table of Contents
-----------------
* [Introduction](#introduction)
* [Installation](#installation)
    * [Requirements](#requirements)
    * [Configuring SMTP Account](#configuring-smtp-account)    
    * [Configuring Twilio Account](#configuring-twilio-account)   
    * [Configuring SQL Server](#configuring-sql-server)       
    * [Additional Configurations](#additional-configurations)      
* [Functional Requirements](#functional-requirements)
    * [Overview](#overview)
    * [Specifications](#specifications)
    * [Program Features](#program-features)
    * [Tasks By Roles](#tasks-by-roles)
    * [Entity Relationship Diagram](#entity-relationship-diagram)
* [Features](#features)
* [Project Structure](#project-structure)
    * [Constants](#constants)
    * [API](#api)
    * [Reference Id](#reference-id)
    * [Centralized Status Control](#centralized-status-control)
    * [Custom Javascript Files](#custom-javascript-files)
    * [Static File Locations](#static-file-locations)
    * [Service](#service)

Introduction
============
This project is for one of our **clients** in the *Industry Project* for **BCIT's Software Systems Developer** program. The Client is a technology consulting company that specializes in the development and integration of custom software solutions for web, mobile and desktop. In addition to that, they provide managed hosting services to many of their clients. Based on the size, complexity of their hosting environment, coupled with the number of applications being hosted in this environment it was becoming increasingly difficult to manage notifications to their clients when there was a maintenance or a problem. 

We were asked to develop a platform which allows our client to manage and issue notifications by Email or SMS. This platform also allows their clients to find out the current status of their applications. 

Installation
============

### Requirements
* Microsoft SQL Server
* SMTP Account
* Twilio Account SID and Phone Number

All of these will be configured in the **Web.Config** of the project.

### Configuring SMTP Account
Open the project solution in Visual Studio and navigate to **Web.Config** file. In the appSettings, do the following:

```
    <add key="SmtpHost" value="SmtpHost Goes Here" />
    <add key="SmtpEmail" value="Smtp Email Goes Here" />
    <add key="SmtpPassword" value="Smtp Password Goes Here" />
    <add key="SmtpNoReplyEmail" value="no-reply@notification-portal.com" />
```
1. Change the **"SmtpHost Goes Here"** value to your SmtpHost
1. Change the **"SmtpEmail Goes Here"** value to your SmtpEmail
1. Change the **"SmtpPassword Goes Here"** value to your SmtpPassword
1. SmtpNoRepyEmail does not need to be changed unless you want to

### Configuring Twilio Account
Open the project solution in Visual Studio and navigate to **Web.Config** file. In the appSettings, do the following:

```
    <add key="TwilioAccountSID" value="Twilio SID Goes Here" />
    <add key="TwilioAuthToken" value="Twilio Auth Token Here" />
    <add key="TwilioFromNumber" value="Twilio Number Here" />
```
1. Go to `https://www.twilio.com/` and register for an account
2. During registration do the following settings:

![alt text](https://github.com/gurkiratbola/NotificationPortal/blob/master/docs/step1.png "Step 1 Twilio")

3. After signing up, you can retrieve your SID and Auth Token from Twilio's Console Dashboard

![alt text](https://github.com/gurkiratbola/NotificationPortal/blob/master/docs/step2.png "Step 2 Twilio")

4. Change the **"Twilio SID Goes Here"** value to your Twilio Account SID
5. Change the **"Twilio Auth Token Here"** value to your Twilio Auth Token 

6. Then go to `https://www.twilio.com/console/phone-numbers/incoming` or follow the image below

![alt text](https://github.com/gurkiratbola/NotificationPortal/blob/master/docs/step3.png "Step 3 Twilio")
![alt text](https://github.com/gurkiratbola/NotificationPortal/blob/master/docs/step4.png "Step 4 Twilio")

7. Generate a phone number
8. Change the **"Twilio Number Here"** value to your Twilio Number
9. Now, Twilio is configured.

### Configuring SQL Server
1. Set up a SQL Server Database. Make sure it's empty and full privileges are granted for it.
1. Clone the repository `git clone https://github.com/gurkiratbola/NotificationPortal`
1. Open the solution file in Visual Studio
1. Build the solution. This will download any required libraries which is required later on.
1. In the Solution Explorer, Go to Properties > Settings. Add a connection to the database by changing the connection string to the database you created in **Step 1**.
1. In the Web.config file, find connectionStrings and add name to **DefaultConnection** or the other steps will fail.
1. After adding the connection string, build the solution.
1. To populate the database with seed data, go to the /Migrations folder and delete all the files except **Configuration.cs**. Don't worry if the file does not exist.
1. Open the NuGet Package Manager Console and type `Update-Database –TargetMigration: $InitialDatabase`
1. Then type `Add-Migration Initial`
1. Lastly, type `Update-Database`
1. And that will seed the database and you're good to go!

### Additional Configurations
If you plan to host the project to a server, some other configurations needs to be adjusted. If this project will be hosted on a **regular domain** or a **sub-domain**, these changes will not apply to you, otherwise do the following:
1. Navigate to the `Scripts/Custom/script.js` within Visual Studio
1. On line 4, `subdir = '/'` add your sub-directory here like **`subdir = '/directory_path/`** 
1. After completing the above step navigate to `Service/TemplateService.cs` 
1. On line 16, `private const string SUB_DIRECTORY = "/";` add your sub-directory path similar to Step 2 like **`private const string SUB_DIRECTORY = "/directory_path/";`**
1. After completing all of these steps, you should be good to deploy the application on a sub-directory successfully.

Functional Requirements
============

### Overview
Notification Portal is a notification system that allows organizations to communicate with clients regarding application/server updates. For example, contacting client regarding hosting services when issues arise suddenly or when maintenance is scheduled to be performed in the near future. 

This system is independent from any external database. Notifications are manually managed by internal employees and are only viewable to external users. Four type of users are required:

* Internal Admin (Internal)
* Internal User (Internal)
* External Admin (Client)
* External Users (Client)

Internal roles will manage all clients, apps, users, servers, and notification delivery while external roles will manage users associated with their company. 

### Specifications
* Notification Portal is a stand-alone system which has it’s own database
* User roles are limited to the four roles listed above
* Role management is not required (without ability to edit/delete user roles)
* All notifications are sent out immediately after creation 
* Notifications are not saved as drafts and are not auto scheduled
* Notifications are sent in the form of Emails and SMS
* Notification is created with association to servers, and further association applications are optional
* Server status is manually tracked and updated by the internal users 

### Program Features
Task | Essential | Nice To Have
------------ | ------------- | -------------
Internal Send / Manage Notifications (Incidents and Maintenance) | ✓ | 
External Users can view Notifications | ✓ | 
Notifications are threaded | ✓ |
Manage Internal / External Users (CRUD) | ✓ |
Manage Clients, Applications and Servers | ✓ | 
External Registration | | ✓
Search on Client, Application | ✓ |
Notification by SMS | | ✓
Full Index / Advanced Search | | ✓

### Tasks By Roles
Task | Internal Admin | Internal User | External Admin | External User
------------ | ------------- | ------------- | ------------- | -------------
Manage Internal Users (CRUD) | ✓ | | 
Manage External Users (CRUD) | ✓ | ✓ | ✓ 
Manage External Admin (CRUD) | ✓ |
Manage Clients (CRUD) | ✓ | ✓
Manage Applications (CRUD) | ✓ | ✓
Manage Notifications | ✓ | ✓
Read Notifications (CUD) | ✓ | ✓ | ✓ | ✓
Read Server Status | ✓ | ✓ |  | 

### Entity Relationship Diagram
![alt text](https://github.com/gurkiratbola/NotificationPortal/blob/master/docs/entity_relationship_diagram.png "Entity Relationship Diagram")

Features
============
The platform was built using **ASP.NET MVC 4.6.2, Bootstrap 4, jQuery 3.1.1, Font Awesome, Summernote.js,** and **Twilio**.

The project does the following:
1. Role based authentication of Users using Identity Framework
1. Internal registration which sends automated, responsive emails for password recovery and account confirmation
1. Management of Applications, Clients, Data Centers, Servers and Statuses
1. Ability to create Notifications associated with servers and applications
1. Notification issued by the application allow Rich Text formatting and prioritization
1. Notifications are sent by both Email and SMS
1. Notifications are threaded and allow updates to previously posted notifications which are grouped
1. Fully Responsive for all screen types

Project Structure
============

### Constants
There are three types of shared constants:

1. **Models -> Key.cs**: These constants represent the reference values created in the database. 
2. **Repositories -> ConstantsRepo.cs**: These constants represent the values used in the project that are shared between views, controller and repositories.
3. **File specific constants**: These constants exist in specific files and are defined within the file.

### API
AJAX is used for loading Notification Index, Application Index and Dashboard. All related logic is hosted in API folder. For the application page, AJAX is used to check all app status on load. 

**Note**: Notification Index and Dashboard Index has inline script tags for handling the AJAX calls using constants defined in ConstantsRepo.cs and Key.cs

### Reference Id
Reference IDs are used in forms and models to be passed back and forth instead of using primary keys. In this case, GUIDs are used.

### Centralized Status Control
User/Application/Server/Notification/Client each has status and all of which are editable in the Status Controller. 

### Custom Javascript Files
Total of nine Javascript files are created to support the functionality mentioned:
1. **add-notification.js**: Customizing rich text editor plugin
1. **notification-refresh-dropdown**: Handles applications and servers list dropdown for notifications
1. **application-multiselect.js**: Handles multi-select inputs on application views 
1. **refresh-application-dropdown.js**:	Filtering application select list based on server selection 
1. **refresh-application-status.js**: Dynamically checks the status of the application
1. **refresh-index-helper.js**:	Re-populate table and pagination info on dashboard and notification index
1. **script.js**: Custom scripts for front-end purposes
1. **server-multiselect.js**: Handles applications dropdown list for servers.
1. **user-multiselect.js**: Handles applications multiselect for users
1. **user-refresh-dropdown.js**: Handles getting the application based on the client selected for users

### Static File Locations
Locations for static files:
* All CSS files are stored in Content
* Custom CSS files: sidebar.css and Site.css
* All images are stored in Content -> images. 
* Favicon is stored outside of the folder structure
* Email Templates are stored in Service -> templates
* Scripts -> Custom folder for all custom .js files
* Scripts -> Plugin folder for all plugin .js files

### Service
* **EncryptionHelper.cs**: For encrypting sensitive information in the web.config file, such as connection string and app settings
* **NotificationService.cs**: For sending Email and SMS by creating / updating notification and for user email confirmation and password reset
* **StringHelper.cs**: For stripping tags created by rich text editor
* **TemplateService.cs**: For Email and SMS templates used

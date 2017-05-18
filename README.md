# Bayleaf - BCIT Industry Project 2017 

Table of Contents
-----------------
* [Introduction](#introduction)
* [Functional Requirements](#functional-requirements)
    * [Overview](#overview)
    * [Specifications](#specifications)
* [Features](#features)
* [Project Structure](#project-structure)
    * [Constants](#constants)
    * [API](#api)
    * [Reference Id](#reference-id)
    * [Centralized Status Control](#centralized-status-control)
    * [Custom Javascript Files](#custom-javascript-files)
    * [Static File Locations](#static-file-locations)
    * [Service](#service)
* [Installation](#installation)

Introduction
============
This project is for *Bayleaf*, one of our clients in the *Industry Project* for **BCIT's Software Systems Developer** program. Bayleaf is a technology consulting company that specializes in the development and integration of custom software solutions for web, mobile and desktop. In addition to that, they provide managed hosting services to many of their clients. Based on the size, complexity of their hosting environment, coupled with the number of applications being hosted in this enviornment it was becoming increasingly difficult to manage notifications to their clients when there was a maintenance or a problem. 

We were asked to develop a platform which allows Bayleaf the management and issuing of the notification which will be implemented within their current system. 

Functional Requirements
============

### Overview
Notification Portal is a notification system that allows organizations to communicate with clients regarding application/server updates. For example, contacting client regarding hosting services when issues arise suddenly or when maintenance is scheduled to be performed in the near future. 

This system is independent from any external database. Notifications are manually managed by internal employees and are only viewable to client users. Four type of users are required:

* Internal Admin (Internal)
* Internal User (Internal)
* External Admin (Client)
* External User (Client)

Internal roles will manage all clients, apps, users, servers, and notification delivery while external roles will manage users associated with their company. 

### Specifications
* Notification Portal is a stand-alone system which has it’s own database
* User roles are limited to the four roles listed above
* Role management is not required (without ability to edit/delete user roles)
* All notifications are sent out immediately after creation 
* Notifications are not saved as drafts and are not auto scheduled
* Notifications are sent in the form of emails only
* Notification is generated either by application or by server
* Server status is manually tracked and updated by the internal users 

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

Note*: Notification Index and Dashboard Index has inline script tags for handling the AJAX calls using constants defined in ConstantsRepo.cs and Key.cs

### Reference Id
Reference IDs are used in forms and models to be passed back and forth instead of using primary keys. In this case, GUIDs are used.

### Centralized Status Control
User/Application/Server/Notification/Client each has status and all of which are editable in the Status Controller. 

### Custom Javascript Files
Total of six Javascript files are created to support the functionality mentioned:
1. **add-notification.js**: Customizing rich text editor plugin
2. **application-multiselect.js**: Handles multi-select inputs on application views 
3. **refresh-application-dropdown.js**:	Filtering application select list based on server selection 
4. **refresh-application-status.js**: Dynamically checks the status of the application
5. **refresh-index-helper.js**:	Re-populate table and pagination info on dashboard and notification index
6. **script.js**: Custom scripts for front-end purposes

### Static File Locations
Locations for static files:
* All CSS files are stored in Content
* Custom CSS files: sidebar.css and Site.css
* All images are stored in Content->images. 
* Favicon is stored outside of the folder structure
* Email Templates are stored in Service->templates
* Scripts->Custom folder for all custom .js files
* Scripts->Plugin folder for all plugin .js files

### Service
1. **EncryptionHelper.cs**: Customizing rich text editor plugin
2. **NotificationService.cs**: For sending Email and SMS by creating/updating notification
3. **StringHelper.cs**: For stripping tags created by rich text editor
4. **TemplateService.cs**: For Email and SMS templates used
Table of Contents
-----------------
* [Introduction](#introduction)
* [Features](#features)
* [Installation](#installation)
    * [Constants](#constants)


Introduction
============
This project is for *Bayleaf*, one of our clients in the *Industry Project* for **BCIT's Software Systems Developer** program. Bayleaf is a technology consulting company that specializes in the development and integration of custom software solutions for web, mobile and desktop. In addition to that, they provide managed hosting services to many of their clients. Based on the size, complexity of their hosting environment, coupled with the number of applications being hosted in this enviornment it was becoming increasingly difficult to manage notifications to their clients when there was a maintenance or a problem. 

We were asked to develop a platform which allows Bayleaf the management and issuing of the notification which will be implemented within their current system. 

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

Installation
============

### Constants
There are three types of shared constants:

1. **Models -> Key.cs**: These constants represent the reference values created in the database. 

2. **Repositories --> ConstantsRepo.cs**: These constants represent the values used in the project that are shared between views, controller and repositories 

3. **File specific constants**: These constants exist in specific files and are defined within the file

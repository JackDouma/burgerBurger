# burgerBurger

COMP-3415 Software Engineering Semester Project

Developed by Jackson Douma, Alessio Schiavi, Liam Slappended and Mike Rosanelli

Live site has been taken down after running out of credits

<h1>1. Introduction</h1>
<h3>1.1 Purpose</h3>
This document is made to describe the specifications, descriptions, and requirements of burgerBurger, our custom burger creation software by burgerBurger Ltd. It is intended to be viewed by any and all stakeholders of burgerBurger Ltd.
<h3>1.2 Scope</h3>
burgerBurger is a web-based burger restaurant management system and storefront, where customers can customize and purchase burger orders online, and managers can manage their product, inventory and revenue. 
<h3>1.3 Definitions, Acronyms, and Abbreviations</h3>
Manager User - The account type created for franchise owners and location managers. This account has access to managerial features such as payroll and inventory management.
Consumer User - The account type for regular customers whose only features are buying and customizing products like burgers, sides, drinks and gift cards.
SuperUser - Owner/Admin accounts
<h3>1.4 References</h3>
Specific articles (N/A)
References general design of Pizza Pizza, Burger King, and Ubereats websites.
<h3>1.5 Overview</h3>
burgerBurger has a consumer user, a manager user, and an admin user. The consumer user will be where users can browse and purchase burger products. The manager user will allow restaurant managers to view and edit their inventory, view and update all active orders, and view all revenue for the day.
<h1>2. Overall Descriptions</h1>
<h3>2.1 Product Perspective</h3>
The software will allow consumers to order food from a burger restaurant, and customize their order to fit their needs. Managers will be able to manage their inventory of their designated location, and see balance reports. Owners will be able to only see reports.
<h3>2.2 Product Functions</h3>
Food ordering service for customers.
Cart to track order contents
Purchase gift cards
Purchase food with gift cards or regular options (credit card etc…)
Track order status and location
Location management service for managers
Email/SMS notification management (thresholds, contacts)
Inventory management
Franchise Management service for owners
Status reports of locations
Dashboard with data to help improve location
<h3>2.3 User Characteristics</h3>
Anyone in highschool and above for the storefront section of the product
Restaurant Managers and Owners, not employees, for the managerial section of the product
<h3>2.4 Constraints</h3>
Short time constraint
Financial constraints
<h3>2.5 Assumptions and Dependencies</h3>
Assume development goes as well as we hope. No major roadblocks found.
Depending on everyone doing their fair share of the work.
<h3>2.6 Requirements Subsets</h3>
Being enrolled in COMP 3415 at Lakehead University in Orillia
<h1>3. Specific Requirements</h1>
<h3>3.1 Functional Requirements</h3>
<h3>3.1.1 Customer Account Management</h3>
Customer account. It will have both a login and out feature located in the header of the website. They will be able to save address and banking information in the settings of the account to save time, which will also update the account balance with gift cards. Discounts and deals will appear and be emailed to accounts that may frequently buy specific items.
<h3>3.1.2  Manager Account Management</h3>
Admin accounts for both managers and business owners. Track inventory and active orders of certain stores. Once more inventory ships to the store, the admin can add it to the inventory on the website.
<h3>3.1.3 Cart Management</h3>
The cart will encompass a comprehensive range of features typical for online food ordering systems. It will display an order summary, detailing items, quantities, prices, and accompanying images. A "suggested for you" section will recommend complementary items to enhance your order. Additionally, it will provide options for pickup or delivery addresses and payment methods. This intuitive and feature-packed cart ensures users can quickly verify their selections, minimizing errors and refunds, while the recommendations can potentially increase sales.
<h3>3.1.4 Inventory Management</h3>
The inventory system will be utilized by the restaurant manager users to see their stock of ingredients, packaging, drinks, etc. Managers will be able to enter their suppliers for each item in their inventory, along with their contact info, and track invoices from that supplier. The inventory numbers will be automatically decreased when an order comes in, and automatically increased when the manager puts in an order to a supplier for however much is ordered. Inventory will track expiry dates of items. The purpose of this is to get rid of expired items and also create promotions for items that will expire soon.
<h3>3.1.5 Order Tracker</h3>
Allow users to track their order in real time. The store administrator can update an order at any time, with useful information such as “Preparing order” or “On Delivery”. The google maps API will be used, which will display an ETA from the restaurant to the location of delivery. Order tracker will store in database commonly used statistics for restaurants such as popularity of items. This data will help to determine what items and their amounts should be ordered in the inventory.
<h3>3.1.6 Payment System</h3>
Stripe will be used to help with the payment process. Users can either pay every time they order, or add money to their account balance. Users will be able to buy gift cards, which can be redeemed with a code, and added to their account balance.
<h3>3.1.7 Group Ordering</h3>
A group ordering system is designed to streamline the process of ordering for multiple individuals by enabling them to place a collective order yet receive separate billing. Users will be able to start a group order, and invite other users to add to the order. A group leader will be the one to confirm and send out the order with the added items from the other users. This system offers a dual advantage. Firstly, it consolidates multiple orders bound for the same destination into a single dispatch, eliminating the need for multiple drivers and thus reducing transportation costs. Secondly, it alleviates the often cumbersome task of manually splitting the bill among participants. Such a system is particularly advantageous for group gatherings, such as parties or house events, where simplicity and efficiency are paramount. Group ordering will also have functionality for corporate/scheduled group orders and catering. An example of this is Lakehead University wants the same order every 1st of the month for a monthly event.
<h3>3.1.8 Email/SMS Notification</h3>
We'll employ Twilio for Email/SMS notifications, integrating it across the entire website, particularly emphasizing the management section. Managers will tailor the notification settings, determining parameters like the minimum number of burger patties in stock that will trigger a notification and the kind of alert to be dispatched. This adaptable notification setup prevents unnecessary alerts, helps concentrate on key concerns, and fosters a streamlined and stress-free management experience. Inventory will also track the expiry dates of items, and you will be notified when they are about to go bad and when they have gone bad. This data can be used to create promotions to use up stock that will expire soon.

<h3>3.1.9 Burger Builder</h3>
The Burger Builder will allow for users to create their own custom burger with types of bread, meat, sauce and toppings, specifically in that order. This part wasn’t added, but as options are added or altered, the user interface next to the web form will update to show an image of the burger being created with different images. It will be implemented in a way to update in real time to ensure the user feels like they are making it within the web form itself.
<h3>3.1.10 Balance Sheet</h3>
This will display the company's assets and liabilities, which will include: inventory shipments, direct product purchases, gift card purchases, and miscellaneous. This will help the restaurant better understand how much profit they generate during a given day. Different reports for managers / owners.
<h3>3.2 Non-functional Requirements</h3>
<h3>3.2.1 Usability</h3>
Simple and straightforward to use for the consumer users with no tutorial required to fully understand the software.
The Manager user may require a small amount of training as they are working for finances, but should be fairly easy to understand.

<h3>3.2.2 Reliability</h3>
Goal is 99.99% uptime
Minor bugs will affect user experience, such as order not saving properly
Significant bugs will affect user experience more, such as orders not going through once paid
Critical bugs will greatly affect user or admin experience, such as significant issues with stock, finance, or order tracking

<h3>3.2.3 Performance</h3>
Quick response time.
System should be able to hold thousands of users on launch.

<h3>3.2.4 Supportability</h3>
All coding standards such as name conventions, spacing, and commenting will be used.
Site uses the MVC framework
3.2.5 Design Constraint
ASP.NET C# Core
Microsoft SQL Management Studio 18
Azure Database
CSS
HTML
Twilio CLI (command line interface)
3.2.6 On-line User Documentation and Help System Requirements
Internet connection required.
Software is run on a website, so little to no software requirements are required for users. Any computer or phone made within the last 10 years will be more than enough.
3.2.7 Purchased Components
Twilio has a subscription/usage fee and will be required for email/SMS notifications.
Azure website and database will be required for software.
Stripe 
<h3>3.2.8 Interfaces</h3>
Front end of website
Home page
Cart page
Account page (admin, manager, customer)
Inventory page
Settings page
<h3>3.2.9 Licensing Requirements</h3>
Twilio free license
Visual Studio
Microsoft SQL Studio
Stripe free trial license
Azure student license
Intellij student license
Visual studio student license
<h3>3.2.10 Legal, Copyright, and Other Notices</h3>
<h3>3.2.11 Applicable Standards</h3>
Accessibility Standards:
We will try to follow AODA standards as closely as possible to ensure accessibility for people with disabilities
Security Standards:
We will be using Stripe for payments which handles secure payments for us.
Coding/Industry Standards:
We will be following the industry/coding standards for ASP.net and any other software that we implement. 
Standards for the MVC framework


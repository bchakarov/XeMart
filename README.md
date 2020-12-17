# XeMart

## Build Status

[![Build Status](https://dev.azure.com/XeMart/XeMart/_apis/build/status/bchakarov.XeMart?branchName=main)](https://dev.azure.com/XeMart/XeMart/_build/latest?definitionId=1&branchName=main)

## :bulb: Project Introduction

**XeMart** is my defense project for the [ASP.NET Core](https://softuni.bg/trainings/3177/asp-dot-net-core-october-2020) course at [SoftUni](https://softuni.bg). It is a ready-to-use ASP.NET Core application.  
**Live demo** at https://xemart.azurewebsites.net  
Admin account - user: test@example.com pass: test@example.com  
Partner account - user: partner1@example.com pass: partner1@example.com

## :pencil: Project Description
XeMart is an e-commerce platform built using ASP.NET Core 3.1 web framework. The app has a **main area** based on the [XeMart Ecommerce Template](https://wrapbootstrap.com/theme/xemart-ecommerce-template-WB048930P)
and an **admin area** based on the [SB Admin 2 Template](https://startbootstrap.com/theme/sb-admin-2).

The **main area** offers main categories, subcategories and products. Users can leave a product review, add the product to favourites or add it to their shopping cart. The cart stores its data either in
the session (**unregistered users**) or the database (**registered users**). Registered users can make orders by entering their address, choosing a supplier and a delivery type. Then they are given the choice between
two payment options - cash on delivery and [Stripe payments service](https://stripe.com). Users can view their order history at any given time. They can also contact the administration by using the contact form or
the live chat (built with **SignalR**). Furthermore, users can make **partner requests**. If approved by an admin, they can upload their company logo (**to cloudinary**) which will be displayed right above the footer.

The **admin area** has a dashboard with user statistics, user messages menu and a live chat box with all of the different chat rooms. Admins can edit the orders, suppliers, categories and products.
They can also approve partner requests and edit the home page carousel slideshow (the images are uploaded to **azure blob storage**).

The app also has a product recommender built with **ML.NET**. The model's training data is taken from the OrderProducts table and consists of ProductId and CopurchasedProductId.
The data and the model are updated daily by a **Hangfire** job and the new model is loaded in the prediction engine pool.

## :hammer: Used technologies
* [ASP.NET CORE 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* [Entity Framework CORE 3.1](https://docs.microsoft.com/en-us/ef/core/)
* [Bootstrap 4](https://getbootstrap.com/)
* [jQuery](https://github.com/jquery/jquery)
* [JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript)
* [TinyMCE](https://www.tiny.cloud/)
* [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer)
* [Google reCAPTCHA v3](https://developers.google.com/recaptcha/docs/v3)
* [Charts.js](https://www.chartjs.org/)
* [Moment.js](https://momentjs.com/)
* [FontAwesome Icon Picker](https://github.com/itsjavi/fontawesome-iconpicker)
* [ImageSharp](https://github.com/SixLabors/ImageSharp)
* [Cloudinary](https://cloudinary.com/documentation/dotnet_integration)
* [Azure Blob Storage](https://docs.microsoft.com/en-us/azure/storage/blobs/)
* [Stripe](https://stripe.com/docs)
* [SignalR](https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/introduction-to-signalr)
* [CsvHelper](https://joshclose.github.io/CsvHelper/getting-started)
* [ML.NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet)
* [Hangfire](https://www.hangfire.io/)

## Preview:

### Database Diagram
![DatabaseDiagram](https://i.imgur.com/aw0scJt.png)

### Home Page
![HomePage](https://i.imgur.com/hAqRTjU.png)

### Subcategories
![Subcategories](https://i.imgur.com/0zebTuP.png)

### Products
![Products](https://i.imgur.com/vCxWycc.png)

### Product Page
![Product Page](https://i.imgur.com/4bCQw7P.png)

### Order Details
![Order Details](https://i.imgur.com/LKG116B.png)

### Live Chat
![Live Chat](https://i.imgur.com/d4h37xw.gif)

### Admin Area
![Admin Area](https://i.imgur.com/HzW7mTs.png)


---

# 🥦 GreenCart – Online Farmers Market

**GreenCart** is a role-based online farmers market web application built with **ASP.NET Core MVC** using the **Database-First** approach. It facilitates seamless interaction between **Farmers** and **Buyers**, allowing product listings, order placement, and delivery management.

---

## 🚀 Features

### 👨‍🌾 Farmer Module

* Add/Edit/Delete farm products
* View orders placed on their products
* Manage deliveries for their orders
* View reviews given by buyers

### 🛒 Buyer Module

* Browse available products
* Place new orders
* Track orders and deliveries
* Review purchased products

### 🛠️ Admin (Optional)

* Manage users
* Monitor platform activity

---

## 🧱 Tech Stack

* **Frontend:** ASP.NET Core MVC, Bootstrap 5
* **Backend:** ASP.NET Core Web API
* **Database:** SQL Server (Database-First Approach)
* **Authentication:** Role-based Login using Identity
* **Hosting:**

  * API + Database: Azure (Optional)
  * Frontend: Vercel / Azure (Optional)

---

## 📦 Database Schema

Key tables:

* `Users` (with roles: Farmer, Buyer, Admin)
* `Products`
* `Orders`
* `OrderItems`
* `Deliveries`
* `Reviews`

---

## 📸 Screenshots

*Add screenshots here showing login, dashboards, and user flows.*

---

## 🔧 Setup Instructions

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/greencart-online-market.git
   ```

2. Open the project in Visual Studio.

3. Configure the connection string in `appsettings.json` to point to your SQL Server database.

4. Run database migration or use the existing `.mdf` file if provided.

5. Build and run the project. Access role-based dashboards after login.

---

## ✅ How to Use

1. **Register/Login** as a Farmer or Buyer.
2. Farmers can manage products and fulfill orders.
3. Buyers can browse, place orders, and review products.
4. Admin can optionally manage users and monitor activity.

---

## 🤝 Contributing

Pull requests are welcome! If you'd like to contribute:

* Fork the repo
* Create a new branch
* Commit your changes
* Open a pull request

---

## ⚠️ Note

> This project is currently in a **prototype phase** and may contain **bugs or incomplete features**.
> I will appreciate it if you identify and help resolve any issues before using it in production.

---

This is a simplified backend API for an e-commerce system, implemented in **.NET 8** using **Clean Architecture**, designed to showcase core backend engineering skills including:

- ✅ Secure API development (JWT Auth)
- ✅ Clean layered separation
- ✅ Pagination & filtering
- ✅ Role-based authorization
- ✅ Entity relationships (Products, Orders, Inventory)

---

## 🚀 Tech Stack

- **.NET 8**
- **Entity Framework Core** (SQL Server)
- **JWT Authentication**
- **FluentValidation**
- **Swagger (Swashbuckle)**
- **Clean Architecture** (API / Application / Domain / Infrastructure)

---

## 📦 Project Structure

EcommerceSystem/
│
├── API/ → API endpoints, controllers, auth
├── Application/ → Business logic, DTOs, interfaces, validation
├── Domain/ → Core entities and enums
├── Infrastructure/ → EF Core, data access, services

## 🔐 Authentication & Authorization

- Users login via `/api/auth/login` using email & password (seeded).
- On successful login, they receive a **JWT token**.
- JWT must be added as `Bearer` token in Swagger to access protected endpoints.
- Role-based protection:
  - `Admin` can access `/api/v1/admin/...`
  - Regular users can only access their own data (orders, etc.)

---

## 🧑‍💼 Seeded Users

| Email              | Password | Role   |
|-------------------|----------|--------|
| `admin@example.com` | `123456` | Admin  |
| `john@example.com`  | `123456` | User   |

---

## 🛠️ Implemented Features

### ✅ Core Business

- Create  orders
- Inventory updates with each order
- Fetch order details (secure to owner / admin)
- Daily sales reporting (admin)

### ✅ Technical

- JWT authentication
- Role-based authorization
- Global error handling
- Filtering + pagination
- DTO mapping (no raw entities exposed)
- Auto-seeding data if DB is empty

---

## 📑 Sample API Usage

### 🔐 Login
```http
POST /api/auth/login
{
  "email": "john@example.com"
}


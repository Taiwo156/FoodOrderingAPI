# Food Ordering API

A comprehensive RESTful API built with ASP.NET Core and Microsoft SQL Server for managing a complete food ordering system. This API supports multiple stores, product management, order processing, user authentication, favorites, delivery tracking, and an integrated chatbot.

## 🚀 Features

- **User Management** - Registration, authentication, and profile management
- **Store Management** - Multi-store support with store profiles
- **Product Management** - Complete CRUD operations for food items
- **Category Management** - Organize products by categories
- **Shopping Cart & Orders** - Full order lifecycle management
- **Payment Processing** - Integrated payment handling
- **Delivery Tracking** - Real-time delivery status updates
- **Favorites** - Save favorite items for quick reordering
- **Chatbot** - AI-powered customer support
- **Store-Specific Products** - Products linked to specific stores

## 🛠️ Technologies

- **Framework**: ASP.NET Core
- **Database**: Microsoft SQL Server (MSSQL)
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Swagger/OpenAPI

## 📋 Prerequisites

- .NET 6.0 SDK or higher
- Microsoft SQL Server 2019 or higher
- Visual Studio 2022 / Visual Studio Code / Rider

## ⚙️ Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/FoodOrderingApi.git
cd FoodOrderingApi
```

### 2. Configure Database Connection
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=FoodOrderingDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3. Run Database Migrations
```bash
dotnet ef database update
```

### 4. Build and Run
```bash
dotnet restore
dotnet build
dotnet run
```

The API will be available at `https://localhost:5001` (or your configured port).

## 📚 API Endpoints

### Authentication & Users
```
POST   /api/user/register          - Register new user
POST   /api/user/login             - User login
GET    /api/user/profile           - Get user profile
PUT    /api/user/profile           - Update user profile
GET    /api/user                   - Get all users (Admin)
DELETE /api/user/{id}              - Delete user (Admin)
```

### Stores
```
GET    /api/store                  - Get all stores
GET    /api/store/{id}             - Get store by ID
POST   /api/store                  - Create new store
PUT    /api/store/{id}             - Update store
DELETE /api/store/{id}             - Delete store
```

### Categories
```
GET    /api/category               - Get all categories
GET    /api/category/{id}          - Get category by ID
POST   /api/category               - Create new category
PUT    /api/category/{id}          - Update category
DELETE /api/category/{id}          - Delete category
```

### Products
```
GET    /api/product                - Get all products
GET    /api/product/{id}           - Get product by ID
GET    /api/product/category/{id}  - Get products by category
POST   /api/product                - Create new product
PUT    /api/product/{id}           - Update product
DELETE /api/product/{id}           - Delete product
```

### Products by Store
```
GET    /api/productbystore/store/{storeId}  - Get all products for a store
GET    /api/productbystore/{id}             - Get product-store mapping
POST   /api/productbystore                  - Add product to store
DELETE /api/productbystore/{id}             - Remove product from store
```

### Orders
```
GET    /api/order                  - Get all orders (Admin)
GET    /api/order/{id}             - Get order by ID
GET    /api/order/user/{userId}    - Get user orders
POST   /api/order                  - Create new order
PUT    /api/order/{id}/status      - Update order status
DELETE /api/order/{id}             - Cancel order
```

### Payments
```
GET    /api/payment/{orderId}      - Get payment by order
POST   /api/payment                - Process payment
PUT    /api/payment/{id}           - Update payment status
```

### Delivery
```
GET    /api/delivery/order/{orderId}  - Get delivery status
POST   /api/delivery                  - Create delivery
PUT    /api/delivery/{id}             - Update delivery status
GET    /api/delivery/driver/{driverId} - Get deliveries by driver
```

### Favorites
```
GET    /api/favorites/user/{userId}   - Get user favorites
POST   /api/favorites                 - Add to favorites
DELETE /api/favorites/{id}            - Remove from favorites
```

### Chatbot
```
POST   /api/chatbot/message           - Send message to chatbot
GET    /api/chatbot/history/{userId}  - Get chat history
```

## 🗄️ Database Schema

The database consists of the following main tables:

- **Users** - User accounts and authentication
- **Stores** - Restaurant/store information
- **Categories** - Product categories
- **Products** - Food items and details
- **ProductsByStore** - Product-store relationships
- **Orders** - Customer orders
- **OrderItems** - Individual items in orders
- **Payments** - Payment transactions
- **Delivery** - Delivery information and tracking
- **Favorites** - User favorite products
- **ChatMessages** - Chatbot conversation history

## 🔐 Authentication

The API uses JWT Bearer token authentication. Include the token in the Authorization header:

```
Authorization: Bearer YOUR_JWT_TOKEN
```

To obtain a token, use the login endpoint with valid credentials.

## 📱 Response Format

All responses follow a consistent JSON format:

### Success Response
```json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully"
}
```

### Error Response
```json
{
  "success": false,
  "error": "Error description",
  "message": "User-friendly error message"
}
```

## 🧪 Testing

Use Swagger UI for interactive API testing:
```
https://localhost:5001/swagger
```

Or use tools like Postman, Insomnia, or cURL.

## 📸 Screenshots

### API Documentation (Swagger)
![Swagger Documentation](screenshots/swagger.png)

### Database Schema
![Database Schema](screenshots/database-schema.png)

### Sample Requests
![API Requests](screenshots/api-requests.png)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Oluboyede Taiwo**
- GitHub: [@Taiwo156](https://github.com/Taiwo156)
- LinkedIn: [Oluboyede Taiwo](https://linkedin.com/in/oluboyede taiwo)

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Microsoft SQL Server for robust database management
- All contributors and supporters of this project

## 📧 Contact

For questions or support, please open an issue or contact [oluboyedetaiwo156@gmail.com](mailto:oluboyedetaiwo156@gmail.com)

---

**Note**: Remember to update the connection strings, URLs, and personal information before deploying to production.

# SHAPE / CONA
## SMART HEALTHY ANALYTICAL PROCESSING ENGINE (SHAPE)

## CULINARY OPTIMIZATION NUTRITION ANALYSIS (CONA)

### Project Description: SHAPE (SMART HEALTHY ANALYTICAL PROCESSING ENGINE) /  CONA (CULINARY OPTIMIZATION NUTRITION ANALYSIS)

#### Overview:

Project SHAPE / CONA is a cross-platform mobile application designed to empower users with personalized nutritional insights and optimized cooking recommendations. Leveraging advanced analysis, CONA takes a user's initial list of ingredients, analyzes their nutritional value, and provides tailored cooking suggestions to maximize nutrient absorption and promote overall health. For example, CONA will advise on the optimal cooking method for carrots to enhance beta-carotene absorption or recommend combining lentils with rice to achieve a complete protein profile.

#### Key Features:

Ingredient Analysis: Users input their ingredient list, and CONA / SHAPE provides a comprehensive nutritional breakdown.
Nutrient Optimization: The application analyzes ingredient interactions and suggests cooking methods to maximize nutrient bioavailability.
Personalized Cooking Recommendations: CONA offers tailored cooking suggestions based on nutritional analysis and user preferences.
Educational Insights: Users receive explanations for the recommended cooking methods and nutritional pairings.
Recipe Integration: Ability to store and analyze recipes.
Cross-Platform Compatibility: A seamless user experience across iOS and Android devices.

#### Technology Stack:

Frontend: React Native for cross-platform mobile application development, ensuring a consistent user experience across iOS and Android.
Backend: .NET Core API for robust and scalable data processing and business logic.
Database: MongoDB (or a similar document database) for flexible data storage and efficient querying of nutritional information and user data.
Cloud Services: (Optional) Azure/AWS for hosting, scalability, and data storage.
#### Goals:

To provide users with actionable nutritional insights and personalized cooking recommendations.
To promote healthier eating habits through informed food choices.
To develop a scalable and maintainable cross-platform application.
#### Target Audience:

Health-conscious individuals seeking to optimize their nutrition.
Individuals with dietary restrictions or specific nutritional needs.
Anyone interested in learning more about the nutritional value of their food.

# CONA Project Structure

This document outlines the structure and architecture of the CONA project, which consists of a React Native mobile application (ConaApp) and an ASP.NET Core backend API (ConaApi).

## Mobile Application (ConaApp)

The mobile application is built using React Native with TypeScript, following modern best practices and a clear separation of concerns.

### Directory Structure

```
ConaApp/
├── src/                      # Main source code directory
│   ├── components/          # Reusable UI components
│   │   ├── InputField.tsx   # Form input component
│   │   ├── NutrientChart.tsx# Nutrition data visualization
│   │   └── RecipeCard.tsx   # Recipe display component
│   ├── context/            # React Context for state management
│   │   ├── AnalysisContext.tsx # Nutrition analysis state
│   │   └── AuthContext.tsx    # Authentication state
│   ├── navigation/         # Navigation configuration
│   │   ├── AppNavigator.tsx   # Main navigation setup
│   │   └── AppNavigatorTemp.tsx
│   ├── screens/           # Main application screens
│   │   ├── AnalysisResults.tsx
│   │   ├── IngredientInput.tsx
│   │   ├── Login.tsx
│   │   ├── RecipeList.tsx
│   │   └── Settings.tsx
│   └── services/         # API and other services
│       └── api.ts       # API communication logic
├── App.tsx              # Root application component
├── package.json         # Dependencies and scripts
└── tsconfig.json       # TypeScript configuration
```

### Key Components

1. **Components**
   - Reusable UI elements
   - Follows component-based architecture
   - TypeScript interfaces for prop typing

2. **Context**
   - Global state management using React Context API
   - Handles authentication state
   - Manages nutrition analysis data

3. **Navigation**
   - React Navigation setup
   - Screen routing and navigation logic

4. **Screens**
   - Main application views
   - Feature-specific logic and UI
   - Integration with components and services

5. **Services**
   - API communication layer
   - Backend integration
   - Data transformation and handling

## Backend API (ConaApi)

The backend is built using ASP.NET Core, providing RESTful endpoints for the mobile application.

### Directory Structure

```
ConaApi/
├── Controllers/           # API endpoints
│   ├── AuthController.cs  # Authentication endpoints
│   └── IngredientController.cs # Ingredient management
├── Models/               # Data models
│   ├── CachedIngredient.cs
│   ├── IngredientAnalysis.cs
│   └── Recipe.cs
├── Services/            # Business logic services
│   ├── NutrientOptimizer.cs
│   └── NutritionService.cs
├── Program.cs           # Application entry point
├── appsettings.json    # Application configuration
└── ConaApi.csproj      # Project file
```

### Key Components

1. **Controllers**
   - Handle HTTP requests
   - Implement RESTful endpoints
   - Authentication and authorization logic

2. **Models**
   - Data representation
   - Entity definitions
   - Database schemas

3. **Services**
   - Business logic implementation
   - Data processing and analysis
   - External service integration

4. **Configuration**
   - Environment-specific settings
   - API configurations
   - Service configurations

## Communication Flow

1. The mobile app makes HTTP requests to the backend API
2. Authentication is handled via JWT tokens
   - Tokens are stored in AsyncStorage on the mobile side
   - Validated through AuthController on the backend
3. API responses are processed and displayed in the mobile UI

## Development Setup

1. Mobile App
   - Node.js and npm required
   - React Native development environment
   - TypeScript support

2. Backend API
   - .NET 9.0 SDK required
   - Visual Studio or VS Code for development
   - SQL Server for database (if applicable)

## Best Practices

1. **Code Organization**
   - Clear separation of concerns
   - Modular architecture
   - Reusable components

2. **Type Safety**
   - TypeScript in frontend
   - Strong typing in C# backend
   - Interface definitions

3. **State Management**
   - Context API for global state
   - Local state when appropriate
   - Proper data flow

4. **Security**
   - JWT authentication
   - Secure storage of sensitive data
   - API endpoint protection

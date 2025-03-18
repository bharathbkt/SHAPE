import axios from 'axios';

const API_BASE_URL = 'https://api.example.com'; // Replace with actual API URL

const api = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const authAPI = {
  login: (email: string, password: string) => 
    api.post('/auth/login', { email, password }),
  register: (email: string, password: string) => 
    api.post('/auth/register', { email, password }),
};

export const recipeAPI = {
  analyzeIngredients: (ingredients: string[]) => 
    api.post('/analyze', { ingredients }),
  getRecipes: () => 
    api.get('/recipes'),
  getRecipeById: (id: string) => 
    api.get(`/recipes/${id}`),
};

export default api;

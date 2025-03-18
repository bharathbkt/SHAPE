import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import Login from '../screens/Login';
import IngredientInput from '../screens/IngredientInput';
import AnalysisResults from '../screens/AnalysisResults';
import RecipeList from '../screens/RecipeList';
import Settings from '../screens/Settings';

import { useAuth } from '../context/AuthContext';

type RootStackParamList = {
  Login: undefined;
  MainApp: undefined;
};

type TabParamList = {
  Ingredients: undefined;
  Analysis: undefined;
  Recipes: undefined;
  Settings: undefined;
};

const Stack = createNativeStackNavigator<RootStackParamList>();
const Tab = createBottomTabNavigator<TabParamList>();

const TabNavigator = () => {
  return (
    <Tab.Navigator>
      <Tab.Screen name="Ingredients" component={IngredientInput} />
      <Tab.Screen name="Analysis" component={AnalysisResults} />
      <Tab.Screen name="Recipes" component={RecipeList} />
      <Tab.Screen name="Settings" component={Settings} />
    </Tab.Navigator>
  );
};

const AppNavigator = () => {
  const { isAuthenticated } = useAuth();

  return (
    <NavigationContainer>
      <Stack.Navigator screenOptions={{ headerShown: false }}>
        {!isAuthenticated ? (
          <Stack.Screen name="Login" component={Login} />
        ) : (
          <Stack.Screen name="MainApp" component={TabNavigator} />
        )}
      </Stack.Navigator>
    </NavigationContainer>
  );
};

export default AppNavigator;

import React, { useState, useEffect } from 'react';
import { 
  View, 
  FlatList, 
  StyleSheet, 
  TextInput,
  TouchableOpacity,
  Text
} from 'react-native';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import RecipeCard from '../components/RecipeCard';

// Types
type RootStackParamList = {
  RecipeList: undefined;
  // Add other screen names and their params here
};

type RecipeListScreenNavigationProp = NativeStackNavigationProp<RootStackParamList, 'RecipeList'>;

interface RecipeListProps {
  navigation: RecipeListScreenNavigationProp;
}

interface Recipe {
  id: string;
  title: string;
  description: string;
}

// Mock data for testing
const mockRecipes = [
  {
    id: '1',
    title: 'Spaghetti Carbonara',
    description: 'Classic Italian pasta dish with eggs, cheese, and pancetta'
  },
  {
    id: '2',
    title: 'Chicken Stir Fry',
    description: 'Quick and healthy stir-fried chicken with vegetables'
  },
  {
    id: '3',
    title: 'Greek Salad',
    description: 'Fresh Mediterranean salad with feta cheese and olives'
  }
];

const RecipeList: React.FC<RecipeListProps> = ({ navigation }) => {
  const [recipes, setRecipes] = useState<Recipe[]>(mockRecipes);
  const [searchQuery, setSearchQuery] = useState('');
  const [filteredRecipes, setFilteredRecipes] = useState<Recipe[]>(mockRecipes);

  useEffect(() => {
    // Filter recipes based on search query
    const filtered = recipes.filter(recipe =>
      recipe.title.toLowerCase().includes(searchQuery.toLowerCase())
    );
    setFilteredRecipes(filtered);
  }, [searchQuery, recipes]);

  const handleAddRecipe = () => {
    // Navigation to Add Recipe screen will be implemented later
    console.log('Navigate to Add Recipe screen');
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <TextInput
          style={styles.searchInput}
          placeholder="Search recipes..."
          value={searchQuery}
          onChangeText={setSearchQuery}
          placeholderTextColor="#666"
        />
        <TouchableOpacity 
          style={styles.addButton}
          onPress={handleAddRecipe}
        >
          <Text style={styles.addButtonText}>Add Recipe</Text>
        </TouchableOpacity>
      </View>

      <FlatList
        data={filteredRecipes}
        renderItem={({ item }) => <RecipeCard recipe={item} />}
        keyExtractor={(item) => item.id}
        style={styles.list}
        showsVerticalScrollIndicator={false}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  header: {
    padding: 16,
    backgroundColor: 'white',
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  searchInput: {
    height: 40,
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
    paddingHorizontal: 16,
    marginBottom: 12,
    fontSize: 16,
  },
  addButton: {
    backgroundColor: '#007AFF',
    padding: 12,
    borderRadius: 8,
    alignItems: 'center',
  },
  addButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: '600',
  },
  list: {
    padding: 16,
  }
});

export default RecipeList;

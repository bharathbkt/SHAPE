import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import InputField from '../components/InputField';

const IngredientInput = () => {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Enter Ingredients</Text>
      <InputField 
        placeholder="Add ingredient"
        onChangeText={(text) => console.log(text)}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
  },
});

export default IngredientInput;

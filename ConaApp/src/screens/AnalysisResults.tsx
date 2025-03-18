import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import NutrientChart from '../components/NutrientChart';

const AnalysisResults = () => {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Analysis Results</Text>
      <NutrientChart data={[]} />
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

export default AnalysisResults;

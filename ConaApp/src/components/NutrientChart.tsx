import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface NutrientChartProps {
  data: any[];
}

const NutrientChart: React.FC<NutrientChartProps> = ({ data }) => {
  return (
    <View style={styles.container}>
      <Text>Nutrient Chart</Text>
      {/* Chart implementation will go here */}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    padding: 10,
    backgroundColor: '#f5f5f5',
    borderRadius: 8,
  },
});

export default NutrientChart;

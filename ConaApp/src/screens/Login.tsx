import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import InputField from '../components/InputField';

const Login = () => {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Login</Text>
      <InputField 
        placeholder="Email"
        onChangeText={(text) => console.log(text)}
      />
      <InputField 
        placeholder="Password"
        secureTextEntry
        onChangeText={(text) => console.log(text)}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 20,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
  },
});

export default Login;

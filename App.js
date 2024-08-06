import React, { useState, useEffect } from "react";
import Navigation from "./src/routes/RouteContainer";
import { QuizProvider } from "./src/context/QuizContext";
import { ActivityIndicator, View, Text } from "react-native";
import * as Font from "expo-font";
import AppLoading from "expo-app-loading";

// Load the custom font manually
const loadFonts = async () => {
	await Font.loadAsync({
		Blaka_400Regular: require('./src/assets/fonts/Blaka_400Regular.ttf'),
	  });
	  
};

const App = () => {
  const [fontsLoaded, setFontsLoaded] = useState(false);

  useEffect(() => {
    loadFonts().then(() => setFontsLoaded(true));
  }, []);

  if (!fontsLoaded) {
    return <ActivityIndicator />;
  } else {
    return (
      <QuizProvider>
        <Navigation />
      </QuizProvider>
    );
  }
};

export default App;

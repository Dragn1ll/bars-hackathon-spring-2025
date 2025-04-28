import React from 'react';
import LoginForm from "./components/LoginForm";
import MyButton from "./components/UI/button/MyButton";
import "./styles/App.css"
import AppRouter from "./components/AppRouter";

function App() {
  return (
    <div className="App">
        <AppRouter />
    </div>
  );
}

export default App;

import { useState } from 'react';
import { Login } from './pages/Login';
import { Products } from './pages/Products';
import { api } from './api';
import './App.css';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(() => !!api.getToken());

  const handleLogin = () => {
    setIsAuthenticated(true);
  };

  return isAuthenticated ? <Products /> : <Login onLogin={handleLogin} />;
}

export default App;
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { Box, ThemeProvider, CssBaseline } from '@mui/material';
import Sidebar from './components/Sidebar';
import Asset from './components/Asset/index.jsx';
import Movement from './components/Movement/index.jsx';
import Order from './components/Order/index.jsx';
import Person from './components/Person/index.jsx';
import Revenue from './components/Revenue/index.jsx';
import Wallet from './components/Wallet/index.jsx';
import theme from './theme';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Box sx={{ display: 'flex' }}>
          <Sidebar />
          <Box
            component="main"
            sx={{ flexGrow: 1, p: 3 }}
          >
            <Routes>
              <Route path="/asset" element={<Asset />} />
              <Route path="/movement" element={<Movement />} />
              <Route path="/order" element={<Order />} />
              <Route path="/person" element={<Person />} />
              <Route path="/revenue" element={<Revenue />} />
              <Route path="/wallet" element={<Wallet />} />
            </Routes>
          </Box>
        </Box>
      </Router>
    </ThemeProvider>
  );
}

export default App;

import React from 'react';
import { Link } from 'react-router-dom';
import { Drawer, List, ListItem, ListItemIcon, ListItemText, ListItemButton } from '@mui/material';
import { AccountBalanceWallet, AttachMoney, CompareArrows, People, Receipt, ShoppingCart } from '@mui/icons-material';

const drawerWidth = 240;

const Sidebar = () => {
  const menuItems = [
    { text: 'Asset', icon: <AccountBalanceWallet />, path: '/asset' },
    { text: 'Movement', icon: <CompareArrows />, path: '/movement' },
    { text: 'Order', icon: <ShoppingCart />, path: '/order' },
    { text: 'Person', icon: <People />, path: '/person' },
    { text: 'Revenue', icon: <AttachMoney />, path: '/revenue' },
    { text: 'Wallet', icon: <Receipt />, path: '/wallet' },
  ];

  return (
    <Drawer
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: drawerWidth,
          boxSizing: 'border-box',
        },
      }}
      variant="permanent"
      anchor="left"
    >
      <List>
        {menuItems.map((item) => (
          <ListItemButton component={Link} to={item.path} key={item.text}>
            <ListItemIcon>{item.icon}</ListItemIcon>
            <ListItemText primary={item.text}  />
          </ListItemButton>
        ))}
      </List>
    </Drawer>
  );
};

export default Sidebar;

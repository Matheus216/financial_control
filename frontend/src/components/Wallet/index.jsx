import React, { useState, useEffect } from 'react';
import { DataGrid } from '@mui/x-data-grid';
import { Button, Modal, TextField, Box, CircularProgress, Alert } from '@mui/material';
import axios from 'axios';

axios.defaults.baseURL = 'http://localhost:5207/api/';

const Wallet = () => {
  const [wallets, setWallets] = useState([]);
  const [open, setOpen] = useState(false);
  const [currentWallet, setCurrentWallet] = useState({ id: null, description: '' });
  const [isUpdate, setIsUpdate] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleOpen = () => {
    setIsUpdate(false);
    setCurrentWallet({ id: null, description: '' });
    setOpen(true);
  };

  const handleEditOpen = (wallet) => {
    setIsUpdate(true);
    setCurrentWallet(wallet);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const fetchWallets = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get('/wallets'); // Adjusted endpoint
      setWallets(response.data);
    } catch (error) {
      setError('Error fetching wallets. Please try again later.');
      console.error('Error fetching wallets:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchWallets();
  }, []);

  const handleDelete = async (id) => {
    try {
      await axios.delete(`/wallets/${id}`); // Adjusted endpoint
      fetchWallets();
    } catch (error) {
      console.error('Error deleting wallet:', error);
    }
  };

  const columns = [
    { field: 'id', headerName: 'ID', width: 90 },
    { field: 'description', headerName: 'Description', width: 250 },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 150,
      renderCell: (params) => (
        <>
          <Button onClick={() => handleEditOpen(params.row)}>Edit</Button>
          <Button onClick={() => handleDelete(params.row.id)}>Delete</Button>
        </>
      ),
    },
  ];

  const handleSave = async () => {
    try {
      if (isUpdate) {
        await axios.put(`/wallets/${currentWallet.id}`, currentWallet); // Adjusted endpoint
      } else {
        await axios.post('/wallets', currentWallet); // Adjusted endpoint
      }
      fetchWallets();
      handleClose();
    } catch (error) {
      console.error('Error saving wallet:', error);
    }
  };

  if (loading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }

  return (
    <div>
      <Button variant="contained" onClick={handleOpen}>
        New Wallet
      </Button>
      <div style={{ height: 400, width: '100%' }}>
        <DataGrid rows={wallets} columns={columns} pageSize={5} />
      </div>
      <Modal open={open} onClose={handleClose}>
        <Box sx={{
          position: 'absolute',
          top: '50%',
          left: '50%',
          transform: 'translate(-50%, -50%)',
          width: 400,
          bgcolor: 'background.paper',
          border: '2px solid #000',
          boxShadow: 24,
          p: 4,
        }}>
          <h2>{isUpdate ? 'Edit Wallet' : 'New Wallet'}</h2>
          <TextField
            label="Description"
            fullWidth
            margin="normal"
            value={currentWallet.description}
            onChange={(e) => setCurrentWallet({ ...currentWallet, description: e.target.value })}
          />
          <Button variant="contained" onClick={handleSave}>
            Save
          </Button>
        </Box>
      </Modal>
    </div>
  );
};

export default Wallet;
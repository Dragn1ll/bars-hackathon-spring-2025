import React from 'react';
import { Outlet } from "react-router-dom";
import MyNavbar from "../components/UI/navbar/MyNavbar";
import MySidebar from "../components/UI/sidebar/MySidebar";
import useUsers from "../hooks/useUsers";
import Box from "@mui/material/Box";

const AdminPage = () => {
    const usersContext = useUsers();

    return (
        <div>
            <MyNavbar/>
            <Box sx={{
                display: 'flex',
                height: '100vh',
                width: '100vw',
                overflow: 'hidden'
            }}>
                <MySidebar />
                <Box sx={{
                    p: 3,
                    flex: 1,
                    display: 'flex',
                    flexDirection: 'column',
                    overflow: 'auto',
                    background: '#f5f5f5',
                }}>
                    <Outlet context={usersContext} />
                </Box>
            </Box>
        </div>
    );
};

export default AdminPage;
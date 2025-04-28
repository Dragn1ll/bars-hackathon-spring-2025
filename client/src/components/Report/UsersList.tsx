import React, {FC} from 'react';
import {
    Box,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Avatar,
    Typography,
    LinearProgress,
    CircularProgress
} from '@mui/material';
import {IUser, IUsersListProps} from "../../types/types";



const UsersList: FC<IUsersListProps> = ({ users, onSelect, loading }) => {
    const handleRowClick = (userId: number) => {
        if (typeof onSelect === 'function') {
            onSelect(userId);
        }
    };

    if (loading) {
        return (
            <Box display="flex" justifyContent="center" p={4}>
                <CircularProgress />
            </Box>
        );
    }

    if (!users || users.length === 0) {
        return (
            <Box p={2}>
                <Typography>Пользователи не найдены</Typography>
            </Box>
        );
    }

    return (
        <Box sx={{ width: '100%' }}>
            <Typography variant="h5" gutterBottom>
                Список пользователей
            </Typography>

            <TableContainer component={Paper} sx={{ maxHeight: '70vh', overflow: 'auto' }}>
                <Table stickyHeader>
                    <TableHead>
                        <TableRow>
                            <TableCell>Пользователь</TableCell>
                            <TableCell>Email</TableCell>
                            <TableCell align="right">Прогресс</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {users.map((user) => (
                            <TableRow
                                key={user.id}
                                hover
                                onClick={() => handleRowClick(user.id)}
                                sx={{
                                    cursor: 'pointer',
                                    '&:hover': {
                                        backgroundColor: 'action.hover'
                                    }
                                }}
                            >
                                <TableCell>
                                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                        <Avatar src={user.avatar} sx={{ mr: 2 }} />
                                        <Typography>{user.name} {user.surname}</Typography>
                                    </Box>
                                </TableCell>
                                <TableCell>{user.email}</TableCell>
                                <TableCell align="right">
                                    <LinearProgress
                                        variant="determinate"
                                        value={user.progress || 0}
                                        sx={{ height: 8, minWidth: 100 }}
                                    />
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
};

export default UsersList;
import React, {useRef, useState} from 'react';
import {
    DataGrid, GridActionsCellItem,
    GridColDef, GridRowId,
    GridRowsProp,
    GridToolbar,

} from '@mui/x-data-grid';
import DeleteIcon from '@mui/icons-material/Delete';
import UndoIcon from '@mui/icons-material/Undo';
import AddIcon from '@mui/icons-material/Add';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import RestoreFromTrashIcon from '@mui/icons-material/RestoreFromTrash';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import {
    Alert,
    Button,
    Dialog, DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    IconButton,
    Snackbar,
    Stack
} from "@mui/material";
import {IUser, ITableUsersContext} from "../types/types"
import UserCreateDialog from "./UserCreateDialog";
import {data, useOutletContext} from "react-router-dom";
import {ruRU} from "@mui/x-data-grid/locales";
import MySpinner from "./UI/spinner/MySpinner";

const TableUsers = () => {
    const {
        users,
        loading,
        error,
        addUser,
        softDeleteUser,
        restoreUser,
        fetchUsers,
        getDeletedUsers
    } = useOutletContext<ITableUsersContext>();

    const [showTable, setShowTable] = useState(true);
    const [openDialog, setOpenDialog] = useState(false);
    const [showDeleted, setShowDeleted] = useState(false);
    const [deletedUsers, setDeletedUsers] = useState<IUser[]>([]);
    const [snackbar, setSnackbar] = useState({
        open: false,
        message: '',
        severity: 'success' as 'success' | 'error'
    });
    const [confirmDialog, setConfirmDialog] = useState({
        open: false,
        id: null as GridRowId | null,
        action: '' as 'delete' | 'restore',
        userName: ''
    });

    const loadDeletedUsers = async () => {
        try {
            const deleted = await getDeletedUsers();
            setDeletedUsers(deleted.filter(user => user.isDeleted));
        } catch (err) {
            console.error('Error loading deleted users:', err);
        }
    };

    const handleDelete = async (id: GridRowId) => {
        const userToDelete = users.find(user => user.id === id);
        if (!userToDelete) return;

        try {
            const success = await softDeleteUser(id as number);
            if (success) {
                setDeletedUsers(prev => [...prev, userToDelete]);
                await loadDeletedUsers();
                showSnackbar(`Пользователь "${userToDelete.name}" перемещен в корзину`, 'success');
            }
        } catch (err) {
            console.error('Ошибка при удалении:', err);
            showSnackbar('Ошибка при удаления пользователя', 'error');
        }
    };

    const handleRestore = async (id: GridRowId) => {
        try {
            const success = await restoreUser(id as number);
            if (success) {
                await loadDeletedUsers();
                showSnackbar('Пользователь восстановлен', 'success');
                await fetchUsers();
            }
        } catch (err) {
            console.error('Ошибка при восстановлении:', err);
            showSnackbar('Ошибка при восстановлении пользователя', 'error');
        }
    };

    const showConfirmDialog = (id: GridRowId, action: 'delete' | 'restore', userName: string) => {
        setConfirmDialog({
            open: true,
            id,
            action,
            userName
        });
    };

    const handleConfirmAction = async () => {
        if (confirmDialog.id) {
            if (confirmDialog.action === 'delete') {
                await handleDelete(confirmDialog.id);
            } else {
                await handleRestore(confirmDialog.id);
            }
        }
        setConfirmDialog(prev => ({ ...prev, open: false }));
    };

    const showSnackbar = (message: string, severity: 'success' | 'error') => {
        setSnackbar({ open: true, message, severity });
    };

    const handleCreateUser = async (user: any) => {
        const success = await addUser(user);
        if (success) {
            console.log("Пользователь успешно добавлен: " + success)
        }
        return success;
    };

    const columns: GridColDef<IUser>[] = [
        {
            field: 'actions',
            headerName: 'Действия',
            width: 100,
            renderCell: (params) => (
                <>
                    {!params.row.isDeleted ? (
                        <IconButton
                            onClick={() => showConfirmDialog(params.id, 'delete', params.row.name)}
                            color="error"
                            size="small"
                            title="Удалить"
                        >
                            <DeleteIcon />
                        </IconButton>
                    ) : (
                        <IconButton
                            onClick={() => showConfirmDialog(params.id, 'restore', params.row.name)}
                            color="primary"
                            size="small"
                            title="Восстановить"
                        >
                            <RestoreFromTrashIcon />
                        </IconButton>
                    )}
                </>
            ),
        },
        {
            field: 'id',
            headerName: 'ID',
            width: 35,
            type: 'number',
        },
        {
            field: 'username',
            headerName: 'Имя пользователя',
            width: 150,
        },
        {
            field: 'name',
            headerName: 'Имя',
            width: 120,
        },
        {
            field: 'surname',
            headerName: 'Фамилия',
            width: 120,
        },
        {
            field: 'email',
            headerName: 'Электронная почта',
            width: 150,
        },
        {
            field: 'age',
            headerName: 'Возраст',
            type: 'number',
            width: 70,
        },
        {
            field: 'role',
            headerName: 'Роль',
            type: 'string',
            width: 120,
        },
        {
            field: 'joinDate',
            headerName: 'Присоединился',
            width: 140,
            valueFormatter: (params: { value: Date | string | null } | null) => {
                if (!params?.value) return 'Нет данных';
                return params.value;
            },
        },
        {
            field: 'lastLogin',
            headerName: 'Последний вход',
            width: 160,
            valueFormatter: (params: { value: Date | string | null } | null) => params?.value || 'Нет данных'
        },
        {
            field: 'isDeleted',
            headerName: 'Статус',
            width: 120,
            renderCell: (params) => (
                params.value ? 'Удален' : 'Активен'
            ),
        },
    ];

    if (loading) return (
        <Box
            sx={{
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',
                height: '100vh'
            }}
        >
            <MySpinner />
        </Box>
    );
    if (error) return (
        <Alert severity="error">
            {error}
        </Alert>
    )
    return (
        <Box sx={{ flex: 1, overflow: 'auto' }}>
            <Typography variant="h4" gutterBottom>
                Пользователи
            </Typography>
            <Stack direction="row" spacing={1} sx={{ mb: 2 }}>
                <Button
                    size="small"
                    onClick={() => setShowTable(!showTable)}
                    variant="outlined"
                    startIcon={showTable ? <VisibilityOffIcon /> : <VisibilityIcon />}
                >
                    {showTable ? 'Скрыть таблицу' : 'Показать таблицу'}
                </Button>
                <Button
                    size="small"
                    onClick={() => setOpenDialog(true)}
                    variant="contained"
                    startIcon={<AddIcon />}
                >
                    Создать пользователя
                </Button>
                <Button
                    size="small"
                    onClick={() => {
                        setShowDeleted(!showDeleted);
                        loadDeletedUsers();
                    }}
                    variant="outlined"
                    startIcon={<RestoreFromTrashIcon />}
                >
                    {showDeleted ? 'Скрыть удаленных' : 'Показать удаленных'}
                </Button>
            </Stack>

            <UserCreateDialog
                open={openDialog}
                onClose={() => setOpenDialog(false)}
                onSubmit={handleCreateUser}
            />

            {showTable && (
                <>
                    <Typography variant="h5" gutterBottom>
                        {showDeleted ? 'Удаленные пользователи' : 'Активные пользователи'}
                    </Typography>
                    <Box sx={{ height: 'calc(100vh - 200px)', width: '100%' }}>
                        <DataGrid
                            localeText={ruRU.components.MuiDataGrid.defaultProps.localeText}
                            rows={showDeleted ?
                                deletedUsers.filter(user => user.isDeleted) :
                                users.filter(user => !user.isDeleted)
                            }                            columns={columns}
                            pageSizeOptions={[5, 10, 25, 50, 100]}
                            slots={{ toolbar: GridToolbar }}
                            disableRowSelectionOnClick
                            getRowId={(row) => row.id}
                            sx={{
                                backgroundColor: 'white'
                            }}
                        />
                    </Box>
                </>
            )}

            <Snackbar
                open={snackbar.open}
                autoHideDuration={3000}
                onClose={() => setSnackbar(prev => ({ ...prev, open: false }))}
                anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
            >
                <Alert
                    severity={snackbar.severity}
                    onClose={() => setSnackbar(prev => ({ ...prev, open: false }))}
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>

            <Dialog
                open={confirmDialog.open}
                onClose={() => setConfirmDialog(prev => ({ ...prev, open: false }))}
            >
                <DialogTitle>
                    {confirmDialog.action === 'delete' ? 'Подтвердите удаление' : 'Подтвердите восстановление'}
                </DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {confirmDialog.action === 'delete'
                            ? `Вы уверены, что хотите удалить пользователя "${confirmDialog.userName}"?`
                            : `Вы уверены, что хотите восстановить пользователя "${confirmDialog.userName}"?`}
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setConfirmDialog(prev => ({ ...prev, open: false }))}>
                        Отмена
                    </Button>
                    <Button
                        onClick={handleConfirmAction}
                        color={confirmDialog.action === 'delete' ? 'error' : 'primary'}
                        autoFocus
                    >
                        {confirmDialog.action === 'delete' ? 'Удалить' : 'Восстановить'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default TableUsers;
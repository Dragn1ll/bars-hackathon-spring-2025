import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button } from '@mui/material';
import { IUser, IUserCreateDialogProps } from '../../types/types';
import {FC, useState} from "react";

const UserCreateDialog: FC<IUserCreateDialogProps> = ({ open, onClose, onSubmit }) => {
    const [newUser, setNewUser] = useState<Omit<IUser, 'id'>>({
        username: '',
        name: '',
        surname: '',
        email: '',
        phone: '',
        role: '',
        age: 0,
        joinDate: new Date().toISOString().split('T')[0],
        lastActive: new Date().toISOString()
    });

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setNewUser(prev => ({
            ...prev,
            [name]: name === 'age' ? Number(value) : value
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const success = await onSubmit(newUser);
        if (success) {
            onClose();
            setNewUser({
                username: '',
                name: '',
                surname: '',
                email: '',
                role: '',
                phone: '',
                age: 0,
            });
        }
    };
    const resetForm = () => {
        setNewUser({
            username: '',
            name: '',
            surname: '',
            email: '',
            role: '',
            phone: '',
            age: 0,
            joinDate: new Date().toISOString().split('T')[0],
            lastActive: new Date().toISOString()
        });
    };

    return (
        <Dialog
            open={open}
            onClose={() => {
                onClose();
                resetForm();
            }}
            aria-labelledby="create-user-dialog"
        >
            <DialogTitle id="create-user-dialog">Создать нового пользователя</DialogTitle>
            <form onSubmit={handleSubmit}>
                <DialogContent dividers>
                    <TextField
                        autoFocus
                        margin="dense"
                        name="username"
                        label="Имя пользователя"
                        type="text"
                        fullWidth
                        variant="outlined"
                        value={newUser.username}
                        onChange={handleInputChange}
                        required
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="dense"
                        name="name"
                        label="Имя"
                        type="text"
                        fullWidth
                        variant="outlined"
                        value={newUser.name}
                        onChange={handleInputChange}
                        required
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="dense"
                        name="surname"
                        label="Фамилия"
                        type="text"
                        fullWidth
                        variant="outlined"
                        value={newUser.surname}
                        onChange={handleInputChange}
                        required
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="dense"
                        name="email"
                        label="Email"
                        type="email"
                        fullWidth
                        variant="outlined"
                        value={newUser.email}
                        onChange={handleInputChange}
                        required
                        sx={{ mb: 2 }}
                    />
                    <TextField
                        margin="dense"
                        name="age"
                        label="Возраст"
                        type="number"
                        fullWidth
                        variant="outlined"
                        value={newUser.age || ''}
                        onChange={handleInputChange}
                        inputProps={{ min: 0 }}
                        sx={{ mb: 2 }}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => {
                        onClose();
                        resetForm();
                    }}>
                        Отмена
                    </Button>
                    <Button type="submit" variant="contained" color="primary">
                        Создать
                    </Button>
                </DialogActions>
            </form>
        </Dialog>
    );
};

export default UserCreateDialog;
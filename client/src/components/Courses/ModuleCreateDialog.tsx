import React, { useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    TextField,
    Button
} from '@mui/material';

interface ModuleCreateDialogProps {
    open: boolean;
    onClose: () => void;
    onAdd: (title: string) => void;
}

const ModuleCreateDialog: React.FC<ModuleCreateDialogProps> = ({
                                                                   open,
                                                                   onClose,
                                                                   onAdd
                                                               }) => {
    const [title, setTitle] = useState('');

    const handleAdd = () => {
        onAdd(title);
        setTitle('');
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Добавить новый модуль</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    label="Название модуля"
                    fullWidth
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    sx={{ mt: 1 }}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Отмена</Button>
                <Button
                    onClick={handleAdd}
                    variant="contained"
                    disabled={!title.trim()}
                >
                    Добавить
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ModuleCreateDialog;
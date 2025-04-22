import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField } from '@mui/material';
import {IModuleCreateDialogProps} from "../types/types"

const ModuleCreateDialog: React.FC<IModuleCreateDialogProps> =
    ({open, onClose, title, value, onChange, onAdd}) => {
    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>{title}</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    label="Название модуля"
                    fullWidth
                    value={value}
                    onChange={(e) => onChange(e.target.value)}
                />
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose}>Отмена</Button>
                <Button
                    onClick={onAdd}
                    disabled={!value.trim()}
                    variant="contained"
                >
                    Добавить
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ModuleCreateDialog;
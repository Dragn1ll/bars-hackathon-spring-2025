import {Dialog, DialogTitle, DialogContent, DialogActions, Button, DialogContentText} from '@mui/material';
import {IConfirmDialogProps} from "../../types/types"

const ConfirmDialog: React.FC<IConfirmDialogProps> = ({
                                                         open,
                                                         title,
                                                         message,
                                                         onConfirm,
                                                         onCancel,
                                                          confirmText = 'Подтвердить',
                                                          cancelText = 'Отмена',
                                                          confirmDisabled = false

                                                      }) => {
    return (
        <Dialog open={open} onClose={onCancel}>
            <DialogTitle>{title}</DialogTitle>
            <DialogContent>
                <DialogContentText>{message}</DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={onCancel}>{cancelText}</Button>
                <Button
                    onClick={onConfirm}
                    color="error"
                    disabled={confirmDisabled}
                >
                    {confirmText}
                </Button>
            </DialogActions>
        </Dialog>
    );
};
export default ConfirmDialog;
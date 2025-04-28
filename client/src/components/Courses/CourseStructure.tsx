import React, {FC, useState} from 'react';
import {
    Box,
    Button,
    Typography,
    IconButton,
    TextField,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle
} from '@mui/material';
import { SimpleTreeView, TreeItem } from '@mui/x-tree-view';
import {
    Add as AddIcon,
    ExpandMore as ExpandMoreIcon,
    ChevronRight as ChevronRightIcon,
    Delete as DeleteIcon
} from '@mui/icons-material';
import {ILesson, ICourse, IModule, ICourseStructureProps } from "../../types/types"


const CourseStructure: FC<ICourseStructureProps> = ({ course, onUpdateCourse }) => {
    const [openModuleDialog, setOpenModuleDialog] = useState(false);
    const [newModuleTitle, setNewModuleTitle] = useState('');

    const handleAddModule = () => {
        if (!newModuleTitle.trim()) return;

        const newModule: IModule = {
            id: `mod${Date.now()}`,
            title: newModuleTitle,
            lessons: [],
        };

        const updatedCourse = {
            ...course,
            modules: [...course.modules, newModule],
        };

        onUpdateCourse(updatedCourse);
        setNewModuleTitle('');
        setOpenModuleDialog(false);
    };

    return (
        <Box sx={{ p: 2, pl: 4 }}>
            <Typography variant="h6" gutterBottom>
                Модули курса: {course.title}
            </Typography>

            <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={() => setOpenModuleDialog(true)}
                sx={{ mb: 2 }}
            >
                Добавить модуль
            </Button>

            <SimpleTreeView
                slots={{
                    expandIcon: ChevronRightIcon,
                    collapseIcon: ExpandMoreIcon,
                }}
                sx={{ height: 'auto', maxHeight: 400, overflow: 'auto' }}
            >
                {course.modules.map(module => (
                    <TreeItem
                        key={module.id}
                        itemId={module.id}
                        label={
                            <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                <Typography sx={{ flexGrow: 1 }}>{module.title}</Typography>
                                <IconButton size="small">
                                    <DeleteIcon fontSize="small" color="error" />
                                </IconButton>
                            </Box>
                        }
                    >
                        {module.lessons.map(lesson => (
                            <TreeItem
                                key={lesson.id}
                                itemId={lesson.id}
                                label={lesson.title}
                            />
                        ))}
                    </TreeItem>
                ))}
            </SimpleTreeView>

            <Dialog open={openModuleDialog} onClose={() => setOpenModuleDialog(false)}>
                <DialogTitle>Добавить новый модуль</DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label="Название модуля"
                        fullWidth
                        value={newModuleTitle}
                        onChange={(e) => setNewModuleTitle(e.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setOpenModuleDialog(false)}>Отмена</Button>
                    <Button onClick={handleAddModule} variant="contained">
                        Добавить
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
};

export default CourseStructure;